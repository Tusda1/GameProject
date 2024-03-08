using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteraction : MonoBehaviour, Interactable
{
    public void Interact()
    {
        GameEvents.saveGame?.Invoke();
        
        GameEvents.showPrompt?.Invoke("Game saved");
    }
}
