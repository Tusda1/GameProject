using UnityEngine;

public class SceneController : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.loadScene += LoadScene;
    }

    private void OnDisable()
    {
        GameEvents.loadScene -= LoadScene;
    }

    private void LoadScene(string sceneName, bool additive)
    {
        if (additive)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
