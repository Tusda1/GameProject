using System;
using UnityEngine;

public class UselessInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string uselessText;
    private bool _interacted;

    public void Interact()
    {
        if (!_interacted)
            GameEvents.showPrompt?.Invoke(uselessText);
        else
            GameEvents.hidePrompt?.Invoke();
        
        _interacted = !_interacted;
    }
}
