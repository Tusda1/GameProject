using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("Player")]
    private bool _playerTurn = true;
    [SerializeField] private AllAttackMoves allAttackMoves;
    
    [Header("Text Box")]
    [SerializeField] private TextMeshProUGUI combatTextBox;
    
    [Header("Sprites")]
    [SerializeField] private Image enemyImage;

    [Header("Semi temporary frequently changed variables")]
    private ActionType _actionType;
    private string _actionName;
    
    // Start is called before the first frame update
    void Start()
    {
        //Make sure text box is empty on start
        combatTextBox.text = "";
        
        //Initialize the enemy data
        enemyImage.sprite = EnemyDataHolder.enemyData.enemySprite;
        float aspectRatio = enemyImage.sprite.rect.height / enemyImage.sprite.rect.width;
        // Set the height to maintain aspect ratio while changing width
        var sizeDelta = enemyImage.rectTransform.sizeDelta;
        sizeDelta = new Vector2(sizeDelta.y / aspectRatio, sizeDelta.y);
        enemyImage.rectTransform.sizeDelta = sizeDelta;

        //Subscribe to the playerTurnOver event
        CombatEvents.playerTurnEnd += PlayerTurnEnd;
    }
    
    private void PlayerTurnEnd(ActionType actionType, string actionName)
    {
        _playerTurn = false;
        _actionType = actionType;
        _actionName = actionName;
        
        StartCoroutine(RunTurns());
    }

    #region Turns Running
    private IEnumerator RunTurns()
    {
        yield return StartCoroutine(PlayerTurn());
        
        
        if (EnemyDataHolder.currentEnemyHealth > 0)
        {
            StartCoroutine(EnemyTurn());  
        }
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
            
            EnemyDataHolder.TakeDamage(attackMove.damage);

            if (attackMove.statusEffect != StatusEffects.None)
            {
                Debug.Log("status effect");
            }
            
            yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, attackMove.narration + " and dealt " + attackMove.damage + " damage!"));
            yield return new WaitForSeconds(2f);
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
                yield return new WaitForSeconds(2f);
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "Couldn't run away..."));
                yield return new WaitForSeconds(2f);
            }
        }
        
        else if (_actionType == ActionType.Talk)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToTalk)
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "You're charisma convinced them!"));
                yield return new WaitForSeconds(2f);
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, "They're not convinced"));
                yield return new WaitForSeconds(2f);
            }
        }
    }
    
    private IEnumerator EnemyTurn()
    {
        Tuple<string, int> enemyMoveData = EnemyDataHolder.enemyData.EnemyAttack();
        string attackNarration = enemyMoveData.Item1 + " and dealt " + enemyMoveData.Item2 + " damage!";
        
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, attackNarration));
        yield return new WaitForSeconds(2f);    
        combatTextBox.text = "";
        
        ButtonScriptCombatMenu.BackToMain();
    }
    #endregion
}
