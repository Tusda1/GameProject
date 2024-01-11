using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManouverer : MonoBehaviour
{
    public static MenuManouverer Instance { get; private set; }
    
    public Button[] buttons;
    private int _currentButtonIndex; // Index of the currently selected button
    
    private bool _highlighted;
    
    private Dictionary<string, Action> _choiceDictionary = new Dictionary<string, Action>();
    private int _choiceIndex;
    private int _choiceCount;

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
    }
    
    private void Start()
    {
        HighlightButton(_currentButtonIndex);
    }

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
        if (_currentButtonIndex > 0)
        {
            _currentButtonIndex = (_currentButtonIndex - 1);
            HighlightButton(_currentButtonIndex);
        }
    }

    private void MoveToNextButton()
    {
        // Move to the next button in the array
        if (_currentButtonIndex < buttons.Length)
        {
            _currentButtonIndex = (_currentButtonIndex + 1);
            HighlightButton(_currentButtonIndex);
        }
    }
    
    private void MoveToDownButton()
    {
        // Move to the previous button in the array
        if (_currentButtonIndex < buttons.Length - 1)
        {
            _currentButtonIndex = (_currentButtonIndex + 2);
            HighlightButton(_currentButtonIndex);
        }
    }
    
    private void MoveToUpButton()
    {
        // Move to the next button in the array
        if (_currentButtonIndex > 1)
        {
            _currentButtonIndex = (_currentButtonIndex - 2);
            HighlightButton(_currentButtonIndex);
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
        buttons[index].onClick.Invoke();
    }
    
    public void ClearChoices()
    {
        _choiceDictionary.Clear();
    }
    
    public void AddChoice(string choiceName, Action choiceAction)
    {
        _choiceDictionary.Add(choiceName, choiceAction);
    }
    
    public void LoadNewMenu()
    {
        _choiceCount = _choiceDictionary.Count;
        
        foreach (var key in _choiceDictionary.Keys)
        {
            buttons[_choiceIndex].GetComponentInChildren<Text>().text = key;
            _choiceIndex++;
        }
    }
}