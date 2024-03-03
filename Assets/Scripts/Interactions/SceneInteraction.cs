using UnityEngine;

public class SceneInteraction : MonoBehaviour, Interactable
{
    [SerializeField] private string sceneName;
    [SerializeField] private string doorName;
    [SerializeField] private bool additive;
    [SerializeField] private GameObject spawnPosition;

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
