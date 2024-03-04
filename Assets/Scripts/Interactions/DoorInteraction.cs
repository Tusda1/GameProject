using UnityEngine;

public class DoorInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string sceneName;
    [SerializeField] private string doorName;
    [SerializeField] private bool additive;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private bool locked;

    private void OnEnable()
    {
        GameEvents.lookForDoor += CheckForDoor;
    }
    
    private void OnDisable()
    {
        GameEvents.lookForDoor -= CheckForDoor;
    }

    public void Interact()
    {
        if (locked)
            return;
        
        GameEvents.playerFrozen?.Invoke();
        GameEvents.loadScene?.Invoke(sceneName, additive, doorName);
    }

    private void CheckForDoor(string doorName)
    {
        if (doorName == this.doorName)
        {
            GameEvents.returnDoor?.Invoke(spawnPosition.transform.position);
        }
    }
}
