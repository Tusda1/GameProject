using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject DialogueUI;

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
        TurnOffDialogueScriptsAfterInitalized();
    }

    public void InitateCombat()
    {
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
    }

    private IEnumerator TurnOffDialogueScriptsAfterInitalized()
    {
        yield return new WaitForSeconds(0.5f);
        DialogueUI.SetActive(false);
    }
    
    public void DialogueScriptsOff()
    {
        DialogueUI.SetActive(false);
    }
    
    public void DialogueScriptsOn()
    {
        DialogueUI.SetActive(true);
    }
}
