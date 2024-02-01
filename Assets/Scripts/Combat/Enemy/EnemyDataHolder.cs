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
        goldGiven = Random.Range((int)enemyData.goldGiven.x, (int)enemyData.goldGiven.y+1);
        expGiven = Random.Range((int)enemyData.xpGiven.x, (int)enemyData.xpGiven.y+1);
    }
    
    public static void TakeDamage(int damage)
    {
        currentEnemyHealth -= damage;
    }
}
