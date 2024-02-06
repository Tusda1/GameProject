using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class PlayerDataHolder
{
    public static Dictionary<string, int> playerData = new Dictionary<string, int>();
    public static List<string> unlockedPlayerSkills;
    public static List<string> EquippedItems;
    private const float _expToNextLevelMultiplier = 1.5f;

    public static void InitializePlayerData(AllAttackMoves allAttackMoves)
    {
        // Load Unlocked attack moves from save file
        Dictionary<string, object> attackMovesDict = SaveFileHandler.GetSaveFile("AttackMoves");
        
        if (attackMovesDict.Count > 0)
        {
            var firstKey = attackMovesDict.Keys.First();
            attackMovesDict.Remove(firstKey);
        }
        
        JArray jArray = (JArray)attackMovesDict[attackMovesDict.Keys.First()];
        unlockedPlayerSkills = jArray.ToObject<List<string>>();
        
        // Load Equipped items from save file
        Dictionary<string, object> equippedItemsDict = SaveFileHandler.GetSaveFile("Equipped");
        
        if (equippedItemsDict.Count > 0)
        {
            var firstKey = equippedItemsDict.Keys.First();
            equippedItemsDict.Remove(firstKey);
        }
        
        jArray = (JArray)equippedItemsDict[equippedItemsDict.Keys.First()];
        EquippedItems = jArray.ToObject<List<string>>();
        
        SetPlayerData();
        
        AddUnlockedAttackMoves(allAttackMoves);
    }
    
    private static void SetPlayerData()
    {
        Dictionary<string, object> tempDict = SaveFileHandler.GetSaveFile("Player");  
        
        foreach (var key in tempDict.Keys)
        {
            if (key == "DataType")
            {
                // Skip the DataType property
                continue;
            }
            
            playerData.Add(key, int.TryParse(tempDict[key].ToString(), out int value) ? value : 0);
        }
    }
    
    private static void AddUnlockedAttackMoves(AllAttackMoves allAttackMoves)
    {
        // Add all unlocked attack moves to the list
        foreach (var attackMove in allAttackMoves.allPlayerSkills)
        {
            if (attackMove.levelUnlocked <= playerData["Level"] && unlockedPlayerSkills.All(skill => skill != attackMove.attackMoveName))
            {
                unlockedPlayerSkills.Add(attackMove.attackMoveName);
            }
        }
    }
    
    public static void TakeDamage(int damage)
    {
        playerData["Health"] -= damage;
    }
    
    public static void GetGold(int amount)
    {
        playerData["Gold"] += amount;
    }
    
    public static void GetExp(int amount)
    {
        playerData["Exp"] += amount;

        if (playerData["Exp"] >= playerData["ExpToNextLevel"])
        {
            LevelUp();
        }
    }
    
    private static void LevelUp()
    {
        playerData["Level"] += 1;
        playerData["Exp"] -= playerData["ExpToNextLevel"];
        playerData["ExpToNextLevel"] = Mathf.RoundToInt(playerData["ExpToNextLevel"] * _expToNextLevelMultiplier);
    }
    
    public static void WriteToSaveFile()
    {
        //Convert playerData to a dictionary of objects then save
        Dictionary<string, object> tempDict = new Dictionary<string, object>();
        
        foreach (var key in playerData.Keys)
        {
            tempDict.Add(key, playerData[key]);
        }
        
        SaveFileHandler.UpdateDataType("Player", tempDict);
    }
}
