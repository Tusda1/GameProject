using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawning : MonoBehaviour
{
    [SerializeField] private float spawnChance = 0.2f;
    
    public bool playerInTrigger;

    private void OnEnable()
    {
        PlayerEvents.playerMoved += RollForSpawn;
    }
    
    private void OnDisable()
    {
        PlayerEvents.playerMoved -= RollForSpawn;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
    
    private void RollForSpawn()
    {
        if (playerInTrigger && Random.value < spawnChance)
        {
            GameManager.Instance.SpawnEnemy();
        }
    }
}
