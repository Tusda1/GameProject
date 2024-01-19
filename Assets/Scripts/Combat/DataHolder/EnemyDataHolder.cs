using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDataHolder
{
    public static EnemyScrubBase enemyData;
    public static int currentEnemyHealth;
    
    public static void SetEnemyData(EnemyScrubBase enemy)
    {
        enemyData = enemy;
        currentEnemyHealth = enemy.maxHealth;
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
