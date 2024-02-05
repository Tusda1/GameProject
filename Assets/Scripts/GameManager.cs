using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public AllEquipment allEquipment;
    public AllAttackMoves allAttackMoves;
    public AllEnemies allEnemies;

    private GameObject _dialogueUI;

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
        
        StartCoroutine(TurnOffDialogueScriptsAfterInitalized());
        
        CombatEvents.combatEnd += EndCombat;
    }

    public void InitateCombat()
    {
        GameEvents.playerFrozen?.Invoke();
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    private void EndCombat()
    {
        SceneManager.UnloadSceneAsync("Combat");
        GameEvents.playerUnfrozen?.Invoke();
    }

    private IEnumerator TurnOffDialogueScriptsAfterInitalized()
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
