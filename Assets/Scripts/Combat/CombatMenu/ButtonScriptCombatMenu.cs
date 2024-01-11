using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonScriptCombatMenu : MonoBehaviour
{
    public void AttackMoveButton()
    {
        MenuManouverer.Instance.ClearChoices();
        
        Dictionary<string, object> attackMovesDict = SaveFileHandler.GetSaveFile("AttackMoves");
        
        if (attackMovesDict.Count > 0)
        {
            var firstKey = attackMovesDict.Keys.First();
            attackMovesDict.Remove(firstKey);
        }
        
        foreach (var key in attackMovesDict.Keys)
        {
            Debug.Log(key);
        }
        
        foreach (var key in attackMovesDict.Keys)
        {
            if ((bool)attackMovesDict[key] == true)
            {
                MenuManouverer.Instance.AddChoice(key, PlayerAttackMoves.attackMoves[key]);
            }
        }
        
        MenuManouverer.Instance.ShowMenuButtons();
        MenuManouverer.Instance.LoadNewMenu();
    }
    
    public void InventoryButton()
    {
        
    }
    
    public void RunButton()
    {
        
    }

    public void TalkButton()
    {
        
    }
}
