using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject _dialogueUI;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Variables")] 
    private bool _levelingUp;
    public bool inCombat;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Rest of your initialization code...
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance
        }
    }

    private void OnEnable()
    {
        CombatEvents.combatEnd += EndCombat;
        GameEvents.levelUp += LevelUp;
        GameEvents.endLevelUp += EndLevelUp;
    }
    
    private void OnDisable()
    {
        CombatEvents.combatEnd -= EndCombat;
        GameEvents.levelUp -= LevelUp;
        GameEvents.endLevelUp -= EndLevelUp;
    }

    public void SpawnEnemy()
    {
        //Create a combined enemy spawn chance that is the sum of all the spawn chances of the enemies in the region
        float combinedSpawnChance = 0f;
        foreach (var enemy in RegionScript.Instance.region.enemies)
        {
            combinedSpawnChance += enemy.spawnChance;
        }
        
        //Select a random number between 0 and the combined spawn chance
        float randomSpawnChance = Random.Range(0f, combinedSpawnChance);
        
        //Select the enemy to spawn based on the random number
        foreach (var regionalEnemy in RegionScript.Instance.region.enemies)
        {
            if (randomSpawnChance < regionalEnemy.spawnChance)
            {
                InstantiateEnemy(regionalEnemy.enemy);
                break;
            }
            randomSpawnChance -= regionalEnemy.spawnChance;
        }
    }
    
    public void SpawnEnemyByName(string enemyName)
    {
        foreach (var enemy in DataBase.allEnemies)
        {
            if (enemy.name == enemyName)
            {
                InstantiateEnemy(enemy);
                break;
            }
        }
    }
    
    private void InstantiateEnemy(EnemyScrub enemy)
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        
        NpcSpeaker enemySpeaker = newEnemy.GetComponent<NpcSpeaker>();
        
        enemySpeaker.npc = enemy;
    }

    public void InitiateCombat()
    {
        inCombat = true;
        GameEvents.playerFrozen?.Invoke();
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    private void EndCombat()
    {
        inCombat = false;
        SceneManager.UnloadSceneAsync("Combat");
        if (!_levelingUp)
        {
            GameEvents.playerUnfrozen?.Invoke();
        }
    }
    
    private void LevelUp()
    {
        SceneManager.LoadSceneAsync("LevelUpScene", LoadSceneMode.Additive);
        _levelingUp = true;
        GameEvents.playerFrozen?.Invoke();
    }

    private void EndLevelUp()
    {
        SceneManager.UnloadSceneAsync("LevelUpScene");
        _levelingUp = false;
        GameEvents.playerUnfrozen?.Invoke();
    }
}
