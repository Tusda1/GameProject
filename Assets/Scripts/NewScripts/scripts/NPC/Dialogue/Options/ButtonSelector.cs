using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    private bool _highlighted;

    private void Update()
    {
        if (NpcSpeaker.CurrentSpeaker != null)
        {
            if (NpcSpeaker.CurrentSpeaker.waitingForResponse == true)
            {
                // Keyboard input to navigate through buttons
                if (Input_Controler.Instance.previousOption)
                {
                    MoveToPreviousButton();
                }
                else if (Input_Controler.Instance.nextOption)
                {
                    MoveToNextButton();
                }
                else if (Input_Controler.Instance.interactPressed) // Enter key to select the current button
                {
                    SelectButton(ButtonManager.currentButtonIndex);
                }
            }
        }
    }

    private void MoveToPreviousButton()
    {
        // Move to the previous button in the array
        ButtonManager.currentButtonIndex = (ButtonManager.currentButtonIndex - 1 + ButtonManager.buttons.Length) % ButtonManager.buttons.Length;
        ButtonManager.HighlightButton(ButtonManager.currentButtonIndex);
    }

    private void MoveToNextButton()
    {
        // Move to the next button in the array
        ButtonManager.currentButtonIndex = (ButtonManager.currentButtonIndex + 1) % ButtonManager.buttons.Length;
        ButtonManager.HighlightButton(ButtonManager.currentButtonIndex);
    }

    private void SelectButton(int index)
    {
        // Perform the action associated with the selected button
        ButtonManager.buttons[index].onClick.Invoke();
    }
}
