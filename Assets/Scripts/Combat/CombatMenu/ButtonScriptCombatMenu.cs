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
        MenuManouverer.Instance.AddChoice("Back", BackToMain, false);
        
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
                MenuManouverer.Instance.AddChoice(key, PlayerAttackMoves.attackMoves[key], true);
            }
        }
        
        MenuManouverer.Instance.LoadNewMenu();
    }
    
    public static void InventoryButton()
    {
        
    }
    
    public static void RunButton()
    {
        
    }

    public static void TalkButton()
    {
        
    }

    public static void BackToMain()
    {
        MenuManouverer.Instance.ClearChoices();
        
        MenuManouverer.Instance.AddChoice("Attack", AttackMoveButton, false);
        MenuManouverer.Instance.AddChoice("Inventory", InventoryButton, false);
        MenuManouverer.Instance.AddChoice("Talk", TalkButton, true);
        MenuManouverer.Instance.AddChoice("Run", RunButton, true);
        
        MenuManouverer.Instance.LoadNewMenu();
    }
}
