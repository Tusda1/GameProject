using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnChance = 0.2f;
    [SerializeField] private float spawnCheckInterval = 1f;
    private float _timer;

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
        enemySpeaker.npc = GameManager.Instance.allEnemies.allEnemies[Random.Range(0, GameManager.Instance.allEnemies.allEnemies.Length)];
    }
}
