using System;
using UnityEngine;


public abstract class EnemyScrubBase : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    
    public int maxHealth;
    public bool isBoss;
    public float chanceToRun;
    public float chanceToTalk;
 
    public abstract Tuple<string, int> EnemyAttack();
}
