using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        
        PlayerDataHolder.SetPlayerData();
        
        StartCoroutine(TurnOffDialogueScriptsAfterInitalized());
    }

    public void InitateCombat()
    {
        Player.frozen = true;
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    public void EndCombat()
    {
        SceneManager.UnloadSceneAsync("Combat");
        Player.frozen = false;
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
