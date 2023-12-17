using UnityEngine;
using UnityEngine.SceneManagement;

public class LeterSceneCahnger : MonoBehaviour
{
    private void Start()
    {
        SceneManager.UnloadSceneAsync("ChurchLeter");
        Player.frozen = false;
    }
}
