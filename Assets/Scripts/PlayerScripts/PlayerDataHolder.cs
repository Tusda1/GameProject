using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class PlayerDataHolder
{
    public static Dictionary<string, int> playerData = new Dictionary<string, int>();
    public static List<string> unlockedPlayerSkills = new List<string>();
    public static List<string> EquippedItems = new List<string>();
    public static List<string> InventoryItems = new List<string>();
    public static Dictionary<StatusEffectScrub, int> playerStatusEffects = new Dictionary<StatusEffectScrub, int>();
    private const float _expToNextLevelMultiplier = 1.5f;

    public static void InitializePlayerData(PlayerSkills[] allAttackMoves)
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
        playerData.Clear();
        
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
    
    private static void AddUnlockedAttackMoves(PlayerSkills[] allAttackMoves)
    {
        // Add all unlocked attack moves to the list
        foreach (var attackMove in allAttackMoves)
        {
            if (attackMove.levelUnlocked <= playerData["Level"] && unlockedPlayerSkills.All(skill => skill != attackMove.attackMoveName))
            {
                unlockedPlayerSkills.Add(attackMove.attackMoveName);
            }
        }
    }
    
    public static void TakeDamage(int damage)
    {
        int trueDamage = damage * (1 - (playerData["Defense"] / 100));
        playerData["Health"] -= trueDamage;
        if (playerData["Health"] < 0)
        {
            playerData["Health"] = 0;
        }
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
        GameEvents.levelUp?.Invoke();
    }
    
    public static void IncreaseStat(string statName, int amount)
    {
        playerData[statName] += amount;
    }
    
    public static void AddItemToInventory(string itemName)
    {
        bool itemExists = false;

        foreach (var item in DataBase.allItems)
        {
            if (item.itemName == itemName)
            {
                itemExists = true;
                break;
            }
        }
        
        if (!itemExists)
        {
            Debug.LogError("Item does not exist");
            return;
        }

        
        InventoryItems.Add(itemName);
        AddEquippedItem(itemName);
    }
    
    public static void RemoveItemFromInventory(string itemName)
    {
        InventoryItems.Remove(itemName);
    }
    
    public static void AddEquippedItem(string itemName)
    {
        bool itemIsInInventory = false;

        foreach (var item in InventoryItems)
        {
            if (item == itemName)
            {
                itemIsInInventory = true;
                break;
            }
        }
        
        if (!itemIsInInventory)
            return;
        
        EquippedItems.Add(itemName);
    }
    
    public static void RemoveEquippedItem(string itemName)
    {
        EquippedItems.Remove(itemName);
    }
    
    public static void WriteToSaveFile(Vector3 playerPosition)
    {
        //Convert playerData to a dictionary of objects then save
        Dictionary<string, object> tempDict = new Dictionary<string, object>();
        
        foreach (var key in playerData.Keys)
        {
            tempDict.Add(key, playerData[key]);
        }
        
        SaveFileHandler.UpdateDataType("Player", tempDict);
        tempDict.Clear();
        
        //Convert unlockedPlayerSkills to a list of objects then save
        SaveFileHandler.UpdateSaveData("AttackMoves", "UnlockedMoves", unlockedPlayerSkills);
        
        //Convert EquippedItems to a list of objects then save
        SaveFileHandler.UpdateSaveData("Equipped", "Equipment", EquippedItems);
        
        //Convert InventoryItems to a list of objects then save
        SaveFileHandler.UpdateSaveData("Inventory", "InventoryContent", InventoryItems);
        
        //Add scene to tempdict
        tempDict.Add("Scene", SceneController.nextScene);
        tempDict.Add("X", playerPosition.x);
        tempDict.Add("Y", playerPosition.y);
        
        SaveFileHandler.UpdateDataType("PlayerPosition", tempDict);
        
        NpcDialogueHandler.UpdateAllWhichDialogues(DataBase.whichDialogueDictionary);
    }
}
