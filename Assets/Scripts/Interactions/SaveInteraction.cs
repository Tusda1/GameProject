using UnityEngine;

public class SaveInteraction : MonoBehaviour, Interactable
{
    public void Interact()
    {
        GameEvents.showPrompt?.Invoke("Game saved", false);
        GameEvents.saveGame?.Invoke();
    }
}
