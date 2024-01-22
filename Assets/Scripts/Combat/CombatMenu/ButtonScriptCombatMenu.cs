using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ButtonScriptCombatMenu
{
    public static void AttackMoveButton()
    {
        MenuManouverer.Instance.ClearChoices();
        MenuManouverer.Instance.AddChoice("Back", BackToMain, false, ActionType.Menu);
        
        Dictionary<string, object> attackMovesDict = SaveFileHandler.GetSaveFile("AttackMoves");
        
        if (attackMovesDict.Count > 0)
        {
            var firstKey = attackMovesDict.Keys.First();
            attackMovesDict.Remove(firstKey);
        }
        
        foreach (var key in attackMovesDict.Keys)
        {
            if ((bool)attackMovesDict[key] == true)
            {
                MenuManouverer.Instance.AddChoice(key, PlayerAttackMoves.attackMoves[key], true, ActionType.Attack);
            }
        }
        
        MenuManouverer.Instance.LoadNewMenu();
    }
    
    public static void InventoryButton()
    {
        
    }
    
    public static void RunButton()
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random < EnemyDataHolder.enemyData.chanceToRun)
        {
            Debug.Log("You ran away");
            GameManager.Instance.EndCombat();
        }
        else
        {
            Debug.Log("You failed to run away");
        }
    }

    public static void TalkButton()
    {
        
    }

    public static void BackToMain()
    {
        MenuManouverer.Instance.ClearChoices();
        
        MenuManouverer.Instance.AddChoice("Attack", AttackMoveButton, false, ActionType.Menu);
        MenuManouverer.Instance.AddChoice("Inventory", InventoryButton, false, ActionType.Menu);
        MenuManouverer.Instance.AddChoice("Talk", TalkButton, true, ActionType.Talk);
        MenuManouverer.Instance.AddChoice("Run", RunButton, true, ActionType.Run);
        
        MenuManouverer.Instance.LoadNewMenu();
    }
}
