using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataHolder
{
    public static Dictionary<string, int> playerData = new Dictionary<string, int>();
    
    public static void SetPlayerData()
    {
        Dictionary<string, object> tempDict = SaveFileHandler.GetSaveFile("Player");  
        
        foreach (var key in tempDict.Keys)
        {
            playerData.Add(key, (int)tempDict[key]);
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
        playerData["ExpToNextLevel"] += amount;
    }
}
