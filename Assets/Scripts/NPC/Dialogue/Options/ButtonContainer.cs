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

    public List<GameObject> buttonList;
    public List<TextMeshProUGUI> buttonTextList;

    private void Start()
    {
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
