using UnityEngine;

[System.Serializable]
public class RegionalEnemy
{
    public EnemyScrub enemy;
    public float spawnChance;
}

[CreateAssetMenu(fileName = "Region", menuName = "Locations/Region")]
public class RegionScrub : ScriptableObject
{
    public RegionalEnemy[] enemies;
}
