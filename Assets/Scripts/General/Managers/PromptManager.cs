using System;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject promptPanel;
    private bool _interacted = true;
    
    private void Start()
    {
        HidePrompt();
    }

    private void OnEnable()
    {
        GameEvents.showPrompt += ShowPrompt;
        GameEvents.interactPressed += HidePrompt;
    }
    
    private void OnDisable()
    {
        GameEvents.showPrompt -= ShowPrompt;
        GameEvents.interactPressed -= HidePrompt;
    }

    private void ShowPrompt(string prompt)
    {
        if (_interacted)
            return;
        
        _interacted = true;
        PlayerEvents.playerFrozen?.Invoke();
        promptText.text = prompt;
        promptPanel.SetActive(true);
    }
    
    private void HidePrompt()
    {
        if(!_interacted)
            return;
        
        _interacted = false;
        PlayerEvents.playerUnfrozen?.Invoke();
        promptPanel.SetActive(false);
    }
}
