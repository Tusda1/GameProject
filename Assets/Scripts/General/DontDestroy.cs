using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnEnable()
    {
        GameEvents.backToMenu += UndoDontDestroyOnLoad;
    }
    
    private void OnDisable()
    {
        GameEvents.backToMenu -= UndoDontDestroyOnLoad;
    }
    
    private void UndoDontDestroyOnLoad()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }
}
