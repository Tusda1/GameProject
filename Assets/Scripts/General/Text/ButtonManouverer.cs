using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManouverer
{
    private bool _highlighted;
    
    public Button[] buttons;
    public int currentButtonIndex; // Index of the currently selected button
    
    private bool _canMove = true;
    
    private MonoBehaviour _monoBehaviour;
    
    public ButtonManouverer(Button[] buttons, MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        this.buttons = buttons;
        
        GameEvents.up += MoveToPreviousButton;
        GameEvents.down += MoveToNextButton;
        GameEvents.interactPressed += SelectButton;
        
        HighlightButton();
    }
    
    public void CleanUp()
    {
        GameEvents.up -= MoveToPreviousButton;
        GameEvents.down -= MoveToNextButton;
        GameEvents.interactPressed -= SelectButton;
    }
    
    private IEnumerator MoveCooldown()
    {
        _canMove = false;
        yield return new WaitForSeconds(0.2f);
        _canMove = true;
    }

    private void RunCoroutine(MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(MoveCooldown());
    }

    private void MoveToPreviousButton()
    {
        if (!_canMove)
        {
            return;
        }
        
        // Move to the previous button in the array
        currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
        HighlightButton();
        
        RunCoroutine(_monoBehaviour);
    }

    private void MoveToNextButton()
    { 
        if (!_canMove)
        {
            return;
        }
        
        // Move to the next button in the array
        currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
        HighlightButton();
        
        RunCoroutine(_monoBehaviour);
    }

    private void SelectButton()
    {
        // Perform the action associated with the selected button
        buttons[currentButtonIndex].onClick.Invoke();
    }
    
    private void HighlightButton()
    {
        // Unhighlight all buttons
        foreach (Button button in buttons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white; // Change this color to suit your needs
            button.colors = colors;
        }

        // Highlight the selected button
        ColorBlock selectedColors = buttons[currentButtonIndex].colors;
        selectedColors.normalColor = Color.blue; // Change this color to suit your needs
        buttons[currentButtonIndex].colors = selectedColors;
    }
}