using UnityEngine;

public class FindDoor : MonoBehaviour
{
    [SerializeField] private string doorName;
    [SerializeField] private GameObject spawnPosition;
    
    private void OnEnable()
    {
        GameEvents.lookForDoor += CheckForDoor;
    }
    
    private void OnDisable()
    {
        GameEvents.lookForDoor -= CheckForDoor;
    }
    
    private void CheckForDoor(string doorName)
    {
        if (doorName == this.doorName)
        {
            GameEvents.returnDoor?.Invoke(spawnPosition.transform.position);
        }
    }
}
