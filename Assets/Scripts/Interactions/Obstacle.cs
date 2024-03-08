using UnityEngine;

public class Obstacle : MonoBehaviour, Interactable
{
    [SerializeField] private string obstacleName;
    private bool _interacted;
    
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
        if (!_interacted)
            GameEvents.showPrompt?.Invoke("I'll need to find a way to get past this.");
        else
            GameEvents.hidePrompt?.Invoke();
    }
}
