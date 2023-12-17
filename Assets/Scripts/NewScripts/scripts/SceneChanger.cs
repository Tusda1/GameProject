using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }
    public int sceneNumber;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Rest of your initialization code...
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance
        }
        
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(sceneNumber+1, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(sceneBuildIndex:sceneNumber);
        sceneNumber++;
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Player");
        SceneManager.LoadScene("farm", LoadSceneMode.Additive);
        SceneManager.LoadScene("ChurchLeter", LoadSceneMode.Additive);
        yield break;
    }

    public void GoInHouse()
    {
  
    }
    
    
    public void Exit()
    {
        Application.Quit();
    }
}
