using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerDataHolder
{
    public static Dictionary<string, int> playerData = new Dictionary<string, int>();
    public static float expToNextLevelMultiplier = 1.5f;
    
    public static void SetPlayerData()
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
    
    public static void TakeDamage(int damage)
    {
        Debug.Log("Took " + damage + " damage");
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
        playerData["ExpToNextLevel"] = Mathf.RoundToInt(playerData["ExpToNextLevel"] * expToNextLevelMultiplier);
    }
}
