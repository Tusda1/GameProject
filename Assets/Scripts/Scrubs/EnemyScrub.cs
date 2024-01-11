using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "new Enemy")]
public class EnemyScrub : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;
    
    public int maxHealth;
}
