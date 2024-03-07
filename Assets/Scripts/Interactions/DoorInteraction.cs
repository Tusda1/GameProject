using UnityEngine;

public class DoorInteraction : FindDoor, Interactable
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool additive;
    [SerializeField] private bool locked;

    public void Interact()
    {
        if (locked)
            return;
        
        PlayerEvents.playerCantInteract?.Invoke();
        PlayerEvents.playerFrozen?.Invoke();
        GameEvents.loadScene?.Invoke(sceneName, additive, doorName);
    }
}
