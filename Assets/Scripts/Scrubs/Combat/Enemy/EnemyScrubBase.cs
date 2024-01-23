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
    public int[] xpGiven;
    public int[] goldGiven;
    public int level;
    
 
    public abstract Tuple<string, int> EnemyAttack();
}
