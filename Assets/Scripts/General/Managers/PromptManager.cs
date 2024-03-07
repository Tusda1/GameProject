using System;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject promptPanel;
    
    private void Start()
    {
        HidePrompt();
    }

    private void OnEnable()
    {
        GameEvents.showPrompt += ShowPrompt;
        GameEvents.hidePrompt += HidePrompt;
    }
    
    private void OnDisable()
    {
        GameEvents.showPrompt -= ShowPrompt;
        GameEvents.hidePrompt -= HidePrompt;
    }

    private void ShowPrompt(string prompt)
    {
        PlayerEvents.playerFrozen?.Invoke();
        promptText.text = prompt;
        promptPanel.SetActive(true);
    }
    
    private void HidePrompt()
    {
        PlayerEvents.playerUnfrozen?.Invoke();
        promptPanel.SetActive(false);
    }
}
