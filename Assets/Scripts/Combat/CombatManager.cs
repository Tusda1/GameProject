using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        yield return StartCoroutine(PlayerTurn());


        StartCoroutine(EnemyDataHolder.currentEnemyHealth > 0 ? EnemyTurn() : OnWin());
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
            
            //Applies attack to enemy
            float modifier = Random.Range(attackMove.attackModifier.modifier.x, attackMove.attackModifier.modifier.y);
            int damage = (int)Math.Round(modifier * PlayerDataHolder.playerData[attackMove.attackModifier.statName.ToString()]);
            EnemyDataHolder.TakeDamage(damage);

            if (attackMove.statusEffect != StatusEffects.None)
            {
                Debug.Log("status effect");
            }
            
            //Writes the attack narration to the combat text box
            string attackNarration = attackMove.narration + " " + EnemyDataHolder.enemyData.characterName + " and dealt " + damage + " <color=#ff0000>damage</color>!";
            yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));
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
    }
    
    private IEnumerator EnemyTurn()
    {   
        //Gets necessary information about enemy attack move
        AttackMove enemyMoveData = EnemyDataHolder.enemyData.EnemyAttack();
        float modifier = Random.Range(enemyMoveData.attackModifier.modifier.x, enemyMoveData.attackModifier.modifier.y);
        int damage = (int)Math.Round(modifier * EnemyDataHolder.enemyStats[enemyMoveData.attackModifier.statName]);
        
        //Applies attack to player
        PlayerDataHolder.TakeDamage(damage);
        
        //Writes the attack narration to the combat text box
        string attackNarration = EnemyDataHolder.enemyData.characterName + " " + enemyMoveData.narration + " and dealt " + damage + " <color=#ff0000>damage</color>!";
        yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));
        combatTextBox.text = "";

        //If player is alive go back to main menu, if player is dead be defeated
        if (PlayerDataHolder.playerData["Health"] <= 0)
        {
            yield return StartCoroutine(WriteTextAndWaitForUserInput("You died..."));
            OnLoss();
        }
        else
        {
            ButtonScriptCombatMenu.BackToMain();
        }
    }
    #endregion
    
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
