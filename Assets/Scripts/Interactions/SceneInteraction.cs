using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteraction : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool additive;
    
    public void Interact()
    {
        GameEvents.loadScene?.Invoke(sceneName, additive);
    }
}
