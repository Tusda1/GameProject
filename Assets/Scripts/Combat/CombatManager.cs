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
    
    // Start is called before the first frame update
    void Start()
    {
        //Make sure text box is empty on start
        combatTextBox.text = "";
        
        //Add all the possible attack moves the player could have access to to a dictionary
        PlayerAttackMoves.PutAttackMovesInDictionary();
        
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
    
    private void PlayerTurnEnd(ActionType actionType)
    {
        _playerTurn = false;
        
        StartCoroutine(RunTurns(actionType));
    }

    #region Turns Running
    private IEnumerator RunTurns(ActionType actionType)
    {
        yield return StartCoroutine(PlayerTurn(actionType));
        
        
        if (EnemyDataHolder.currentEnemyHealth > 0)
        {
            StartCoroutine(EnemyTurn());  
            
        }
    }
    
    private IEnumerator PlayerTurn(ActionType actionType)
    {
        if (actionType == ActionType.Attack)
        {
            PlayerAttackMove attackMove = ScriptableObject.CreateInstance<PlayerAttackMove>();
            
            foreach (var attack in allAttackMoves.allAttackMoves)
            {
                if (attack.attackMoveName == PlayerAttackMoves.currentAttackMove)
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
        
        else if (actionType == ActionType.Item)
        {
            
        }
        
        else if (actionType == ActionType.Run)
        {
            
        }
        
        else if (actionType == ActionType.Talk)
        {
            
        }
        
        ButtonScriptCombatMenu.BackToMain();
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
