using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Slime", menuName = "New Enemy/Slime")]
public class SlimeEnemy : EnemyScrubBase
{
    public override Tuple<string, int> EnemyAttack()
    {
        int minDamage = 0;
        int maxDamage = 1;
        int damage = Random.Range(minDamage, maxDamage + 1);
        PlayerDataHolder.TakeDamage(damage);
        
        Debug.Log("Slime Enemy Attack");
        
        return Tuple.Create("Slime slimed you", damage);
    }
}
