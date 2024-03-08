using UnityEngine;

public class FindDoor : MonoBehaviour
{
    [SerializeField] protected string doorName;
    [SerializeField] protected GameObject spawnPosition;
    
    protected virtual void OnEnable()
    {
        GameEvents.lookForObject += CheckForDoor;
    }
    
    protected virtual void OnDisable()
    {
        GameEvents.lookForObject -= CheckForDoor;
    }
    
    protected virtual void CheckForDoor(string doorName)
    {
        if (doorName == this.doorName)
        {
            GameEvents.returnDoor?.Invoke(spawnPosition.transform.position);
            PlayerEvents.playerUnfrozen?.Invoke();
            PlayerEvents.playerCanInteract?.Invoke();
        }
    }
}
