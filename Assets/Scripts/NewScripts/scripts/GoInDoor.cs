using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, Interactable
{
    private bool _isInHouse;

    public PlayableDirector director; 
    public void Interact()
    {
        if (_isInHouse)
        {
            
            _isInHouse = true;
        }
        else if(_isInHouse)
        {
            print("goOutOfHouse");
            _isInHouse = false;
        }
        
    }
}
