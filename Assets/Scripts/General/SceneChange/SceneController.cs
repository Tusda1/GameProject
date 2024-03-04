using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            StartCoroutine(LoadRegularScene(sceneName));
        }
    }
    
    private IEnumerator LoadRegularScene(string sceneName)
    {
        GameEvents.SceneFadeIn?.Invoke();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.2f);
        GameEvents.lookForDoor?.Invoke(_currentDoor);
        yield return new WaitForSeconds(0.3f);
        GameEvents.SceneFadeOut?.Invoke();
    }
    
    private void ReturnDoor(Vector2 position)
    {
        player.transform.position = position;
        GameEvents.playerUnfrozen?.Invoke();
    }
}
