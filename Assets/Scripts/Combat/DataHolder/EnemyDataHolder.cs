using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDataHolder
{
    public static EnemyScrub enemyData;
    public static int currentEnemyHealth;
    public static string enemyName;
    public static Sprite enemySprite;
    
    public static void SetEnemyData(EnemyScrub enemy)
    {
        enemyData = enemy;
        currentEnemyHealth = enemyData.maxHealth;
        enemyName = enemyData.enemyName;
        enemySprite = enemyData.enemySprite;
    }
    
    public static void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        if (currentEnemyHealth <= 0)
        {
            Debug.Log("battle over");
        }
    }
}
