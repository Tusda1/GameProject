using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slime", menuName = "New Enemy/Slime")]
public class SlimeEnemy : EnemyScrubBase
{
    public override Tuple<string, int> EnemyAttack()
    {
        int damage = 1;
        PlayerDataHolder.TakeDamage(damage);
        
        Debug.Log("Slime Enemy Attack");
        
        return Tuple.Create("Slime slimed you", damage);
    }
}
