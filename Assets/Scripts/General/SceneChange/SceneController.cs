using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string _currentDoor;
    public static string nextScene;
    private GameObject player;
    
    private void OnEnable()
    {
        PlayerEvents.returnPlayer += GetPlayer;
        GameEvents.loadScene += LoadScene;
        GameEvents.loadFirstScene += LoadFirstSceneMethod;
        GameEvents.returnDoor += ReturnDoor;
    }

    private void OnDisable()
    {
        PlayerEvents.returnPlayer -= GetPlayer;
        GameEvents.loadScene -= LoadScene;
        GameEvents.loadFirstScene -= LoadFirstSceneMethod;
        GameEvents.returnDoor -= ReturnDoor;
    }
    
    private void GetPlayer(GameObject player)
    {
        this.player = player;
    }
    
    private void LoadFirstSceneMethod(string sceneName, Vector2 position)
    {
        nextScene = sceneName;
        StartCoroutine(LoadFirstScene(sceneName, position));
    }
    
    private IEnumerator LoadFirstScene(string sceneName, Vector2 position)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameEvents.SceneFadeIn?.Invoke();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        while (currentScene == SceneManager.GetActiveScene().name)
        {
            yield return null;
        }
        player.transform.position = position;
        GameEvents.SceneFadeOut?.Invoke();
        PlayerEvents.playerUnfrozen?.Invoke();
        PlayerEvents.playerCanInteract?.Invoke();
    }

    private void LoadScene(string sceneName, bool additive, string doorName)
    {
        _currentDoor = doorName;
        nextScene = sceneName;
        
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
        string currentScene = SceneManager.GetActiveScene().name;
        GameEvents.SceneFadeIn?.Invoke();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        while (currentScene == SceneManager.GetActiveScene().name)
        {
            yield return null;
        }
        GameEvents.lookForObject?.Invoke(_currentDoor);
        yield return new WaitForSeconds(0.1f);
        GameEvents.SceneFadeOut?.Invoke();
    }
    
    private void ReturnDoor(Vector2 position)
    {
        player.transform.position = position;
    }
}
