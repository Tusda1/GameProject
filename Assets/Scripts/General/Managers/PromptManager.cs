using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject promptPanel;
    private bool _interacted = true;
    
    [SerializeField] private Button[] choiceButtons;
    private ButtonManouverer _buttonManouverer;
    
    private void Start()
    {
        HidePrompt();
    }

    private void OnEnable()
    {
        GameEvents.showPrompt += ShowPrompt;
        GameEvents.showChoicePrompt += ShowChoicePrompt;
        GameEvents.interactPressed += HidePrompt;
    }
    
    private void OnDisable()
    {
        GameEvents.showPrompt -= ShowPrompt;
        GameEvents.showChoicePrompt -= ShowChoicePrompt;
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

    private void ShowChoicePrompt(string prompt)
    {
        if (_interacted)
            return;
        
        _interacted = true;
        PlayerEvents.playerFrozen?.Invoke();
        promptText.text = prompt;
        promptPanel.SetActive(true);
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(true);
        }
        
        _buttonManouverer = new ButtonManouverer(choiceButtons, this);
    }
    
    private void HidePrompt()
    {
        if(!_interacted)
            return;
        
        _interacted = false;
        PlayerEvents.playerUnfrozen?.Invoke();
        promptPanel.SetActive(false);
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }

        if (_buttonManouverer != null)
        {
            _buttonManouverer.CleanUp();   
            _buttonManouverer = null;
        }
    }

    public void YesChoice()
    {
        GameEvents.YesChoice?.Invoke();
        HidePrompt();
    }
    
    public void NoChoice()
    {
        GameEvents.NoChoice?.Invoke();
        HidePrompt();
    }
}
