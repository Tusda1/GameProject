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
        currentEnemyHealth = enemy.maxHealth;
        enemyName = enemy.enemyName;
        enemySprite = enemy.enemySprite;
    }
    
    public static void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
        if (currentEnemyHealth <= 0)
        {
            Debug.Log("battle over");
            GameManager.Instance.EndCombat();
        }
    }
}
