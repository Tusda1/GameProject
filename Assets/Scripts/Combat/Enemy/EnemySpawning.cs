using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawning : MonoBehaviour
{
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
                GameManager.Instance.SpawnEnemy();
            }
        }
    }
}
