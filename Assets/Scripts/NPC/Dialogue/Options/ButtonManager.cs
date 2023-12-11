using System.Collections.Generic;
using UnityEngine;

public static class ButtonManager
{
    public static int buttonPressed;
    
    public static void DisplayButtons(string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            ButtonContainer.Instance.buttonList[i].SetActive(true);
            ButtonContainer.Instance.buttonTextList[i].text = options[i];
        }
    }

    public static void HideButtons()
    {
        foreach (GameObject button in ButtonContainer.Instance.buttonList)
        {
            button.SetActive(false);
        }
    }
    

}
