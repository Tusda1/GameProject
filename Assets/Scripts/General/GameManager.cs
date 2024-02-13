using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameObject _dialogueUI;

    [Header("Variables")] 
    private bool _levelingUp;
    
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

    private void Start()
    {
        _dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        
        StartCoroutine(TurnOffDialogueScriptsAfterInitialized());
        
        CombatEvents.combatEnd += EndCombat;
        GameEvents.levelUp += LevelUp;
        GameEvents.endLevelUp += EndLevelUp;
    }

    private void OnDestroy()
    {
        CombatEvents.combatEnd -= EndCombat;
        GameEvents.levelUp -= LevelUp;
        GameEvents.endLevelUp -= EndLevelUp;
    }

    public void InitiateCombat()
    {
        GameEvents.playerFrozen?.Invoke();
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    private void EndCombat()
    {
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

    private IEnumerator TurnOffDialogueScriptsAfterInitialized()
    {
        yield return new WaitForSeconds(0.01f);
        _dialogueUI.SetActive(false);
    }
    
    public void DialogueScriptsOff()
    {
        _dialogueUI.SetActive(false);
        
    }
    
    public void DialogueScriptsOn()
    {
        _dialogueUI.SetActive(true);
    }
}
