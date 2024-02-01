using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wolf", menuName = "New Enemy/Wolf")]
public class WolfEnemy : EnemyScrubBase
{
    public override Tuple<string, int> EnemyAttack()
    {
        Debug.Log("Wolf Attack");
        return Tuple.Create("hi", 2);
    }
}
