using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneTester : MonoBehaviour
{
    public EnemyScrubBase enemy;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyDataHolder.SetEnemyData(enemy);
            StartCombat();
        }
    }

    private void StartCombat()
    {
        GameManager.Instance.InitateCombat();
    }
}
