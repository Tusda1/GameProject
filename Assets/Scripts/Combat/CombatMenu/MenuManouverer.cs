using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManouverer : MonoBehaviour
{
    public Button[] buttons;
    private int currentButtonIndex = 0; // Index of the currently selected button
    
    private bool _highlighted;

    private void Update()
    {
        // Keyboard input to navigate through buttons
        if (Input_Controller.Instance.upMenu)
        {
            MoveToUpButton();
        }
        else if (Input_Controller.Instance.downMenu)
        {
            MoveToDownButton();
        }
        else if (Input_Controller.Instance.leftMenu)
        {
            MoveToPreviousButton();
        }
        else if (Input_Controller.Instance.rightMenu)
        {
            MoveToNextButton();
        }
        else if (Input_Controller.Instance.interactPressed) // Enter key to select the current button
        {
            SelectButton(ButtonManager.currentButtonIndex);
        }
    }

    private void MoveToPreviousButton()
    {
        // Move to the previous button in the array
        if (currentButtonIndex > 0)
        {
            currentButtonIndex = (currentButtonIndex - 1);
            HighlightButton(currentButtonIndex);
        }
    }

    private void MoveToNextButton()
    {
        // Move to the next button in the array
        if (currentButtonIndex < buttons.Length)
        {
            currentButtonIndex = (currentButtonIndex + 1);
            HighlightButton(currentButtonIndex);
        }
    }
    
    private void MoveToDownButton()
    {
        // Move to the previous button in the array
        if (currentButtonIndex < buttons.Length - 1)
        {
            currentButtonIndex = (currentButtonIndex + 2);
            HighlightButton(currentButtonIndex);
        }
    }
    
    private void MoveToUpButton()
    {
        // Move to the next button in the array
        if (currentButtonIndex > 1)
        {
            currentButtonIndex = (currentButtonIndex - 2);
            HighlightButton(currentButtonIndex);
        }
    }
    
    public void HighlightButton(int index)
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

    private void SelectButton(int index)
    {
        // Perform the action associated with the selected button
        ButtonManager.buttons[index].onClick.Invoke();
    }
}