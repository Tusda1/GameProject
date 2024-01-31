using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonContainer : MonoBehaviour
{
    public static ButtonContainer Instance { get; private set; }

    public GameObject buttonSelector;
    
    public GameObject[] buttonList; //
    public TextMeshProUGUI[] buttonTextList; 

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
        
        buttonSelector.SetActive(false);
    }
}
