using UnityEngine;

public class SceneController : MonoBehaviour
{
    private string _currentDoor;
    [SerializeField] private GameObject player;
    
    private void OnEnable()
    {
        GameEvents.loadScene += LoadScene;
        GameEvents.returnDoor += ReturnDoor;
    }

    private void OnDisable()
    {
        GameEvents.loadScene -= LoadScene;
        GameEvents.returnDoor -= ReturnDoor;
    }

    private void LoadScene(string sceneName, bool additive, string doorName)
    {
        _currentDoor = doorName;
        
        if (additive)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        GameEvents.lookForDoor?.Invoke(_currentDoor);
    }
    
    private void ReturnDoor(Vector2 position)
    {
        player.transform.position = position;
    }
}
