using UnityEngine;

public class Obstacle : MonoBehaviour, Interactable
{
    [SerializeField] private string obstacleName;
    
    private void OnEnable()
    {
        GameEvents.lookForObject += LookForObstacle;
    }
    
    private void OnDisable()
    {
        GameEvents.lookForObject -= LookForObstacle;
    }
    
    private void LookForObstacle(string obstacleName)
    {
        if (obstacleName == this.obstacleName)
        {
            Destroy(gameObject);
        }
    }
    
    public void Interact()
    {
        GameEvents.showPrompt?.Invoke("I'll need to find a way to get past this.");
    }
}
