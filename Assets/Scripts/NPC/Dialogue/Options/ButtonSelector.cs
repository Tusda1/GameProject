using UnityEngine;

public class ButtonSelector : MonoBehaviour
{
    private bool _highlighted;

    private void OnEnable()
    {
        GameEvents.upMenu += MoveToPreviousButton;
        GameEvents.downMenu += MoveToNextButton;
        GameEvents.interactPressed += SelectButton;
    }
    
    private void OnDisable()
    {
        GameEvents.upMenu -= MoveToPreviousButton;
        GameEvents.downMenu -= MoveToNextButton;
        GameEvents.interactPressed -= SelectButton;
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

    private void SelectButton()
    {
        // Perform the action associated with the selected button
        ButtonManager.buttons[ButtonManager.currentButtonIndex].onClick.Invoke();
    }
}
