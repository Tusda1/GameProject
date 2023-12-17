using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonManager
{
    public static int currentButtonIndex = 0; // Index of the currently selected button
    
    public static int buttonPressed;
    public static Button[] buttons; // Array to store all the buttons
    
    public static void DisplayButtons(string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            ButtonContainer.Instance.buttonList[i].SetActive(true);
            ButtonContainer.Instance.buttonTextList[i].text = options[i];
        }
        
        buttons = new Button[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            buttons[i] = ButtonContainer.Instance.buttonList[i].GetComponent<Button>();
        }

        currentButtonIndex = 0;
        HighlightButton(currentButtonIndex);
    }

    public static void HideButtons()
    {
        foreach (GameObject button in ButtonContainer.Instance.buttonList)
        {
            button.SetActive(false);
        }
    }
    
    public static void HighlightButton(int index)
    {
        // Unhighlight all buttons
        foreach (Button button in buttons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white; // Change this color to suit your needs
            button.colors = colors;
        }

        // Highlight the selected button
        ColorBlock selectedColors = buttons[index].colors;
        selectedColors.normalColor = Color.blue; // Change this color to suit your needs
        buttons[index].colors = selectedColors;
    }
}
