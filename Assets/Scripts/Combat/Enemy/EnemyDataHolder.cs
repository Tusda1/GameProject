using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDataHolder
{
    public static EnemyScrubBase enemyData;
    public static int currentEnemyHealth;
    public static int goldGiven;
    public static int expGiven;
    
    public static void SetEnemyData(EnemyScrubBase enemy)
    {
        enemyData = enemy;
        currentEnemyHealth = enemy.maxHealth;
        goldGiven = Random.Range(enemyData.goldGiven[0], enemyData.goldGiven[1]+1);
        expGiven = Random.Range(enemyData.xpGiven[0], enemyData.xpGiven[1]+1);
    }
    
    public static void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
    }
}
