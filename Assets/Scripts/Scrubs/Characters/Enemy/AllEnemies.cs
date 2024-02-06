using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllEnemies", menuName = "Combat/AllEnemies")]
public class AllEnemies : ScriptableObject
{
    public EnemyScrub[] allEnemies;
}
