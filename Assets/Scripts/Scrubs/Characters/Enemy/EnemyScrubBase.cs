using System;
using UnityEngine;


public abstract class EnemyScrubBase : CharacterBase
{
    public RuntimeAnimatorController animatorController;
    public int maxHealth;
    public bool isBoss;
    public float chanceToRun;
    public float chanceToTalk;
    public Vector2 xpGiven;
    public Vector2 goldGiven;
    public int level;
    
 
    public abstract Tuple<string, int> EnemyAttack();
}
