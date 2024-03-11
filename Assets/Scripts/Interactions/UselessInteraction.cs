using System;
using UnityEngine;

public class UselessInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string uselessText;

    public void Interact()
    {
        GameEvents.showPrompt?.Invoke(uselessText, false);
    }
}
