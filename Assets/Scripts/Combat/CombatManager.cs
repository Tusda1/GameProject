using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [Header("Player")]
    private bool _playerInput;
    private bool _waitingForUserInput;
    
    [Header("Text Box")]
    [SerializeField] private TextMeshProUGUI combatTextBox;
    
    [Header("Sprites")]
    [SerializeField] private Animator enemyAnimator;

    [Header("Semi temporary frequently changed variables")]
    private ActionType _actionType;
    private string _actionName;
    private bool _playerCanMove;
    private bool _enemyCanMove;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Make sure text box is empty on start
        combatTextBox.text = "";
        
        enemyAnimator.runtimeAnimatorController = EnemyDataHolder.enemyData.animatorController;

        //Subscribe to the playerTurnOver event
        CombatEvents.playerTurnEnd += PlayerTurnEnd;
        GameEvents.interactPressed += UserInput;
    }

    private void OnDestroy()
    {
        CombatEvents.playerTurnEnd -= PlayerTurnEnd;
        GameEvents.interactPressed -= UserInput;
    }

    private void UserInput()
    {
        if (_waitingForUserInput)
            _playerInput = true;
    }
    
    private void PlayerTurnEnd(ActionType actionType, string actionName)
    {
        _actionType = actionType;
        _actionName = actionName;
        
        StartCoroutine(RunTurns());
    }

    #region Turns Running
    private IEnumerator RunTurns()
    {
        if (PlayerDataHolder.playerData["Speed"] > EnemyDataHolder.enemyStats[StatEnum.Speed])
        {
            yield return StartCoroutine(PlayerTurn());
            yield return StartCoroutine(EnemyTurn());
        }
        else
        {
            yield return StartCoroutine(EnemyTurn());
            yield return StartCoroutine(PlayerTurn());
        }
        
        ButtonScriptCombatMenu.BackToMain();
    }
    
    private IEnumerator PlayerTurn()
    {
        if (_actionType == ActionType.Attack)
        {
            //Iterates through all the players innate attack moves and finds the attack move that matches the action name
            AttackMove attackMove = ScriptableObject.CreateInstance<AttackMove>();

            foreach (var attack in ScrubHolder.Instance.allAttackMoves.allPlayerSkills)
            {
                if (attack.attackMoveName == _actionName)
                {
                    attackMove = attack;
                }
            }

            //Iterates through all the player's equipped items and finds the attack move that matches the action name
            foreach (var equipped in PlayerDataHolder.EquippedItems)
            {
                foreach (var equipment in ScrubHolder.Instance.allEquipment.allEquipment)
                {
                    if (equipped == equipment.name)
                    {
                        foreach (var weaponSkill in equipment.weaponSkills)
                        {
                            if (weaponSkill.attackMoveName == _actionName)
                            {
                                attackMove = weaponSkill;
                            }
                        }
                    }
                }
            }
            
            //Calculate if attack hits
            bool attackHit = DidAttackHit(attackMove.accuracy, PlayerDataHolder.playerData["Speed"], EnemyDataHolder.enemyStats[StatEnum.Speed], attackMove.accuracyScalesWithSpeed);
            
            //Applies attack to enemy
            if (attackHit)
            {
                int damage = ExecuteAttack(attackMove, PlayerDataHolder.playerData[attackMove.attackModifier.statName.ToString()]);
                PlayerDataHolder.playerData["Mana"] -= attackMove.manaCost;
                
                //Calculate if status effects are applied then applies them
                List<StatusEffect> statusEffectsToApply = ApplyStatusEffects(attackMove.statusEffects);
                foreach (var statusEffect in statusEffectsToApply)
                {
                    EnemyDataHolder.enemyStatusEffects.Add(statusEffect.statusEffectScrub, statusEffect.duration);
                }

                EnemyDataHolder.TakeDamage(damage);
                CombatEvents.updateHealthUI?.Invoke();
                
                //Writes the attack narration to the combat text box
                string attackNarration = attackMove.narration + " " + EnemyDataHolder.enemyData.characterName + " and dealt " + damage + " <color=#ff0000>damage</color>!";
                yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));

                //Narrate status effects applied to enemy
                foreach (var statusEffect in EnemyDataHolder.enemyStatusEffects)
                {
                    yield return StartCoroutine(WriteTextAndWaitForUserInput(statusEffect.Key.statusEffectName.ToString() + " applied to " + EnemyDataHolder.enemyData.characterName));
                }
            }
            else
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("Attack missed!"));
            }
            
            combatTextBox.text = "";
        }
        
        else if (_actionType == ActionType.Item)
        {
            
        }
        
        else if (_actionType == ActionType.Run)
        {
            int random = Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToRun)
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("You ran away!"));
                CombatEnd();
            }
            else
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("Couldn't run away..."));
            }
        }
        
        else if (_actionType == ActionType.Talk)
        {
            int random = Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToTalk)
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("You're charisma convinced them!"));
                Enemy.CurrentEnemy.TriggerDialogue();
                CombatEnd();
            }
            else
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("They're not convinced"));
            }
        }

        if (EnemyDataHolder.currentEnemyHealth <= 0)
        {
            yield return StartCoroutine(OnWin());
        }
    }
    
    private IEnumerator EnemyTurn()
    {   
        //Gets necessary information about enemy attack move
        AttackMove enemyMoveData = EnemyDataHolder.enemyData.EnemyAttack();
        
        //Calculate if attack hits
        bool attackHit = DidAttackHit(enemyMoveData.accuracy, EnemyDataHolder.enemyStats[StatEnum.Speed], PlayerDataHolder.playerData["Speed"], enemyMoveData.accuracyScalesWithSpeed);

        //Commence attack if attack hit
        if (attackHit)
        {
            //Calculates damage
            int damage = ExecuteAttack(enemyMoveData, EnemyDataHolder.enemyStats[enemyMoveData.attackModifier.statName]);
            
            //Calculate if status effects are applied then applies them
            List<StatusEffect> statusEffectsToApply = ApplyStatusEffects(enemyMoveData.statusEffects);
            foreach (var statusEffect in statusEffectsToApply)
            {
                PlayerDataHolder.playerStatusEffects.Add(statusEffect.statusEffectScrub, statusEffect.duration);
            }
            
            //Applies attack to player
            PlayerDataHolder.TakeDamage(damage);
            CombatEvents.updateHealthUI?.Invoke();
            
            //Writes the attack narration to the combat text box
            string attackNarration = EnemyDataHolder.enemyData.characterName + " " + enemyMoveData.narration + " and dealt " + damage + " <color=#ff0000>damage</color>!";
            yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));
            
            //Narrate status effects applied to player
            foreach (var statusEffect in PlayerDataHolder.playerStatusEffects)
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput(statusEffect.Key.statusEffectName.ToString() + " applied to " + "you"));
            }
        }
        else
        {
            yield return StartCoroutine(WriteTextAndWaitForUserInput("Enemy missed!"));
        }
        
        combatTextBox.text = "";

        //If player is alive go back to main menu, if player is dead be defeated
        if (PlayerDataHolder.playerData["Health"] <= 0)
        {
            yield return StartCoroutine(WriteTextAndWaitForUserInput("You died..."));
            OnLoss();
        }
    }
    #endregion
    
    private bool DidAttackHit(int accuracy, int speed, int hostileSpeed, bool accuracyScalesWithSpeed)
    {
        int attackAccuracy;

        attackAccuracy = accuracyScalesWithSpeed ? speed : accuracy;
        
        int hostileSpeedRandom = Random.Range(0, hostileSpeed);
        
        //Rolls for attackAccuracy 4 times to give advantage to attacker
        int accuracyRandom = 0;
        int attackAccuracyRolls = 10;
        
        for (int i = 0; i < attackAccuracyRolls; i++)
        {
            int tempAccuracyRandom = Random.Range(0, attackAccuracy);
            if (tempAccuracyRandom > accuracyRandom)
            {
                accuracyRandom = tempAccuracyRandom;
            }
        }
        
        return accuracyRandom > hostileSpeedRandom;
    }
    
    private int ExecuteAttack(AttackMove attackMove, int scalingStat)
    {
        float modifier = Random.Range(attackMove.attackModifier.modifier.x, attackMove.attackModifier.modifier.y);
        int damage = (int)Math.Round(modifier * scalingStat);
        
        //Rolls for crit chance
        int critRandom = Random.Range(0, 100);
        if (critRandom < attackMove.critChance)
        {
            damage *= attackMove.critMultiplier;
        }
        
        return damage;
    }

    private List<StatusEffect> ApplyStatusEffects(StatusEffect[] statusEffects)
    {
        List<StatusEffect> appliedStatusEffects = new List<StatusEffect>();
        
        foreach (var statusEffect in statusEffects)
        {
            int random = Random.Range(0, 100);
            if (random < statusEffect.chanceToApply)
            {
                appliedStatusEffects.Add(statusEffect);
            }
        }
        
        return appliedStatusEffects;
    }
    
    private void TriggerStatusEffect(Dictionary<StatusEffectScrub, int> statusEffects, bool targetIsPlayer)
    {
        bool canMove = true;
        
        foreach (var statusEffect in statusEffects)
        {
            int random = Random.Range(0, 100);
            if (random < statusEffect.Key.chanceToStun)
            {
                canMove = false;
            }
            
            if (targetIsPlayer)
            {
                _playerCanMove = canMove;
                PlayerDataHolder.TakeDamage(statusEffect.Key.damage 
                                            + (statusEffect.Key.maxHealthPercentDamage/100) 
                                            * PlayerDataHolder.playerData["MaxHealth"]);
            }
            else
            {
                _enemyCanMove = canMove;
                EnemyDataHolder.TakeDamage(statusEffect.Key.damage 
                                           + (statusEffect.Key.maxHealthPercentDamage/100) 
                                           * EnemyDataHolder.enemyData.maxHealth);
            }
        }
    }
    
    private void DecreaseStatusEffectDuration(Dictionary<StatusEffectScrub, int> statusEffects)
    {
        List<StatusEffectScrub> statusEffectToRemove = new List<StatusEffectScrub>();
        
        foreach (var statusEffect in statusEffects)
        {
            if (statusEffect.Value <= 0)
            {
                statusEffectToRemove.Add(statusEffect.Key);
            }
            else
            {
                statusEffects[statusEffect.Key] = statusEffect.Value - 1;
            }
        }
        
        foreach (var statusEffect in statusEffectToRemove)
        {
            statusEffects.Remove(statusEffect);
        }
    }
    
    private IEnumerator OnWin()
    {
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You Won!"));
        
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You earned " + EnemyDataHolder.goldGiven +  " gold!"));
        
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You earned " + EnemyDataHolder.expGiven + " xp!"));
        
        PlayerDataHolder.GetGold(EnemyDataHolder.goldGiven);
        PlayerDataHolder.GetExp(EnemyDataHolder.expGiven);
        
        CombatEnd();
    }
    
    private void OnLoss()
    {
        CombatEnd();
    }
    
    private void CombatEnd()
    {
        StopAllCoroutines();
        EnemyDataHolder.ClearEnemyData();
        CombatEvents.combatEnd?.Invoke();
    }
    
    private IEnumerator WriteTextAndWaitForUserInput(string text)
    {
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, text));
        yield return StartCoroutine(WaitForUserInput());
    }
    
    private IEnumerator WaitForUserInput()
    {
        _waitingForUserInput = true;
        while (!_playerInput)
        {
            yield return null;
        }
        _waitingForUserInput = false;
        _playerInput = false;
    }
}
