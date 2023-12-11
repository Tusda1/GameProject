using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonContainer : MonoBehaviour
{
    public static ButtonContainer Instance { get; private set; }
    
    public GameObject buttonOne;
    public GameObject buttonTwo;
    public GameObject buttonThree;
    public GameObject buttonFour;

    public TextMeshProUGUI buttonOneText;
    public TextMeshProUGUI buttonTwoText;
    public TextMeshProUGUI buttonThreeText;
    public TextMeshProUGUI buttonFourText;

    public List<GameObject> buttonList = new List<GameObject>(); //
    public List<TextMeshProUGUI> buttonTextList = new List<TextMeshProUGUI>(); 

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
        
        buttonList.Add(buttonOne);
        buttonList.Add(buttonTwo);
        buttonList.Add(buttonThree);
        buttonList.Add(buttonFour);
        
        buttonTextList.Add(buttonOneText);
        buttonTextList.Add(buttonTwoText);
        buttonTextList.Add(buttonThreeText);
        buttonTextList.Add(buttonFourText);
    }
}
