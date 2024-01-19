using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataHolder
{
    public static int playerHealth = 10;
    
    public static void TakeDamage(int damage)
    {
        Debug.Log("Took " + damage + " damage");
        playerHealth -= damage;
    }
}
