using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private string _currentDoor;
    public static string currentScene;
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
        currentScene = sceneName;
        StartCoroutine(LoadFirstScene(sceneName, position));
    }
    
    private IEnumerator LoadFirstScene(string sceneName, Vector2 position)
    {
        GameEvents.SceneFadeIn?.Invoke();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.2f);
        GameEvents.returnDoor?.Invoke(position);
        yield return new WaitForSeconds(0.3f);
        GameEvents.SceneFadeOut?.Invoke();
    }

    private void LoadScene(string sceneName, bool additive, string doorName)
    {
        _currentDoor = doorName;
        currentScene = sceneName;
        
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
        GameEvents.lookForObject?.Invoke(_currentDoor);
        yield return new WaitForSeconds(0.3f);
        GameEvents.SceneFadeOut?.Invoke();
    }
    
    private void ReturnDoor(Vector2 position)
    {
        player.transform.position = position;
    }
}
