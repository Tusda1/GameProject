using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnChance = 0.2f;
    [SerializeField] private float spawnCheckInterval = 1f;
    private float _timer = 0.5f;

    void OnTriggerStay2D(Collider2D other)
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer + spawnCheckInterval <= spawnCheckInterval)
        {
            _timer = spawnCheckInterval;
            if (other.CompareTag("Player") && Random.value < spawnChance)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        
        NpcSpeaker enemySpeaker = newEnemy.GetComponent<NpcSpeaker>();
        
        //Create a combined enemy spawn chance that is the sum of all the spawn chances of the enemies in the region
        float combinedSpawnChance = 0f;
        foreach (var enemy in RegionScript.Instance.region.enemies)
        {
            combinedSpawnChance += enemy.spawnChance;
        }
        
        //Select a random number between 0 and the combined spawn chance
        float randomSpawnChance = Random.Range(0f, combinedSpawnChance);
        
        //Select the enemy to spawn based on the random number
        foreach (var enemy in RegionScript.Instance.region.enemies)
        {
            if (randomSpawnChance < enemy.spawnChance)
            {
                enemySpeaker.npc = enemy.enemy;
                break;
            }
            randomSpawnChance -= enemy.spawnChance;
        }
    }
}
