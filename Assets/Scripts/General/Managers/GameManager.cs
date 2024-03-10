using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject _dialogueUI;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Variables")] 
    public bool levelingUp;
    [HideInInspector]
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
        
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        CombatEvents.combatEnd += EndCombat;
        GameEvents.levelUp += LevelUp;
        GameEvents.endLevelUp += EndLevelUp;
        GameEvents.playerDeath += playerDied;
        GameEvents.escapePressed += BackToMenu;
    }
    
    private void OnDisable()
    {
        CombatEvents.combatEnd -= EndCombat;
        GameEvents.levelUp -= LevelUp;
        GameEvents.endLevelUp -= EndLevelUp;
        GameEvents.playerDeath -= playerDied;
        GameEvents.escapePressed -= BackToMenu;
    }
    
    private void BackToMenu()
    {
        //Unload all scenes then load main menu
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != SceneManager.GetActiveScene().name)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        
        GameEvents.backToMenu?.Invoke();
        SceneManager.LoadScene("MainMenu");
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
            if (enemy.characterName == enemyName)
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
        PlayerEvents.playerFrozen?.Invoke();
        PlayerEvents.playerCantInteract?.Invoke();
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    private void EndCombat()
    {
        inCombat = false;
        SceneManager.UnloadSceneAsync("Combat");
        PlayerEvents.playerUnfrozen?.Invoke();
        PlayerEvents.playerCanInteract?.Invoke();
    }
    
    private void playerDied()
    {
        EndCombat();
        PlayerEvents.playerFrozen?.Invoke();
        PlayerEvents.playerCantInteract?.Invoke();
        
        DataBase.InitializeData();
        PlayerDataHolder.InitializePlayerData(DataBase.allPlayerSkills);
        
        Dictionary<string, object> tempLoadingDict = SaveFileHandler.GetSaveFile("PlayerPosition");
        
        GameEvents.loadFirstScene?.Invoke(tempLoadingDict["Scene"].ToString(), new Vector2(Convert.ToSingle(tempLoadingDict["X"]), Convert.ToSingle(tempLoadingDict["Y"])));
    }
    
    private void LevelUp()
    {
        levelingUp = true;
        SceneManager.LoadScene("LevelUpScene", LoadSceneMode.Additive);
        PlayerEvents.playerFrozen?.Invoke();
        PlayerEvents.playerCantInteract?.Invoke();
    }

    private void EndLevelUp()
    {
        SceneManager.UnloadSceneAsync("LevelUpScene");
        levelingUp = false;
        PlayerEvents.playerUnfrozen?.Invoke();
        PlayerEvents.playerCanInteract?.Invoke();
    }
}
