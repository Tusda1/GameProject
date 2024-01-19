using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private bool _playerTurn = true;
    
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

    private void PlayerTurnEnd()
    {
        _playerTurn = false;
        if (EnemyDataHolder.currentEnemyHealth > 0)
        {
            StartCoroutine(EnemyTurn());  
            
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
}
