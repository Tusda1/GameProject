using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyDataHolder
{
    public static EnemyScrub enemyData;
    public static int currentEnemyHealth;
    public static int goldGiven;
    public static int expGiven;
    public static Dictionary<StatEnum, int> enemyStats = new Dictionary<StatEnum, int>();
    public static Dictionary<StatusEffectScrub, int> enemyStatusEffects = new Dictionary<StatusEffectScrub, int>();
    
    public static void SetEnemyData(EnemyScrub enemy)
    {
        enemyStats.Clear();
        enemyData = enemy;
        currentEnemyHealth = enemy.maxHealth;
        goldGiven = Random.Range((int)enemyData.goldGiven.x, (int)enemyData.goldGiven.y+1);
        expGiven = Random.Range((int)enemyData.xpGiven.x, (int)enemyData.xpGiven.y+1);
        foreach (var stat in enemy.enemyStats)
        {
            enemyStats.Add(stat.statName, stat.modifier);
        }
    }
    
    public static void ClearEnemyData()
    {
        enemyData = null;
        currentEnemyHealth = 0;
        goldGiven = 0;
        expGiven = 0;
        enemyStats.Clear();
    }
    
    public static void TakeDamage(int damage)
    {
        int trueDamage = damage * (1 - (enemyStats[StatEnum.Defense] / 100));
        currentEnemyHealth -= trueDamage;
        if (currentEnemyHealth < 0)
        {
            currentEnemyHealth = 0;
        }
    }
}
