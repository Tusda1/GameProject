using UnityEngine;

public class SaveInteraction : MonoBehaviour, Interactable
{
    private bool _interacted;
    
    public void Interact()
    {
        if (_interacted)
            return;
            
        GameEvents.showPrompt?.Invoke("Game saved");
        GameEvents.saveGame?.Invoke();
        
        _interacted = !_interacted;
    }
}
