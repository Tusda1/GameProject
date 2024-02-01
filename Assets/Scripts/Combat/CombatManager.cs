using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [Header("Player")]
    private bool _playerInput;
    private bool _waitingForUserInput;
    
    [SerializeField] private AllAttackMoves allAttackMoves;
    
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
            PlayerAttackMove attackMove = ScriptableObject.CreateInstance<PlayerAttackMove>();
            
            foreach (var attack in allAttackMoves.allAttackMoves)
            {
                if (attack.attackMoveName == _actionName)
                {
                    attackMove = attack;
                }
            }
            
            //Determines damage dealt based on the range of the attack move
            int damage = Random.Range((int)attackMove.damageRange.x, (int)attackMove.damageRange.y + 1);
            EnemyDataHolder.TakeDamage(damage);

            if (attackMove.statusEffect != StatusEffects.None)
            {
                Debug.Log("status effect");
            }
            
            yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, attackMove.narration + " and dealt " + damage + " damage!"));
            yield return StartCoroutine(WaitForUserInput());
            combatTextBox.text = "";
        }
        
        else if (_actionType == ActionType.Item)
        {
            
        }
        
        else if (_actionType == ActionType.Run)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToRun)
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You ran away!"));
                yield return StartCoroutine(WaitForUserInput());
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "Couldn't run away..."));
                yield return StartCoroutine(WaitForUserInput());
            }
        }
        
        else if (_actionType == ActionType.Talk)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToTalk)
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You're charisma convinced them!"));
                yield return StartCoroutine(WaitForUserInput());
                Enemy.CurrentEnemy.TriggerDialogue();
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "They're not convinced"));
                yield return StartCoroutine(WaitForUserInput());
            }
        }
    }
    
    private IEnumerator EnemyTurn()
    {
        Tuple<string, int> enemyMoveData = EnemyDataHolder.enemyData.EnemyAttack();
        string attackNarration = enemyMoveData.Item1 + " and dealt " + enemyMoveData.Item2 + " damage!";
        
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, attackNarration));
        yield return StartCoroutine(WaitForUserInput());
        combatTextBox.text = "";

        //If player is alive go back to main menu, if player is dead be defeated
        if (PlayerDataHolder.playerData["Health"] <= 0)
        {
            yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You died..."));
            yield return StartCoroutine(WaitForUserInput());
            GameManager.Instance.EndCombat();
        }
        else
        {
            ButtonScriptCombatMenu.BackToMain();
        }
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
    #endregion
    
    private IEnumerator OnWin()
    {
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You Won!"));
        yield return StartCoroutine(WaitForUserInput());
        
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You earned " + EnemyDataHolder.goldGiven +  " amount of gold!"));
        yield return StartCoroutine(WaitForUserInput());
        
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You earned " + EnemyDataHolder.expGiven + " amount of xp!"));
        yield return StartCoroutine(WaitForUserInput());
        
        //Gold and exp given here
        GameManager.Instance.EndCombat();
    }
}
