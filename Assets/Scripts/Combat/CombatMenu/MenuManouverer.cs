using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManouverer : MonoBehaviour
{
    public static MenuManouverer Instance { get; private set; }
    
    public GameObject[] buttonGameObjects;
    private Button[] _buttons;
    private int _currentButtonIndex; // Index of the currently selected button
    
    private bool _highlighted;
    
    private Dictionary<string, Action> _choiceDictionary = new Dictionary<string, Action>();
    private List<string> _keys = new List<string>();
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
        // Initialize the array of Buttons using the GameObject references
        _buttons = new Button[buttonGameObjects.Length];

        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            // Get the Button component from each GameObject
            _buttons[i] = buttonGameObjects[i].GetComponent<Button>();
        }
        
        // Sets up the initial menu buttons and then highlights one of them
        ButtonScriptCombatMenu.BackToMain();
        
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
            SelectButton(_currentButtonIndex);
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
        if (_currentButtonIndex < _buttons.Length)
        {
            _currentButtonIndex = (_currentButtonIndex + 1);
            HighlightButton(_currentButtonIndex);
        }
    }
    
    private void MoveToDownButton()
    {
        // Move to the previous button in the array
        if (_currentButtonIndex < _buttons.Length - 1)
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
        foreach (Button button in _buttons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white; // Change this color to suit your needs
            button.colors = colors;
        }

        // Highlight the selected button
        ColorBlock selectedColors = _buttons[index].colors;
        selectedColors.normalColor = Color.blue; // Change this color to suit your needs
        _buttons[index].colors = selectedColors;
    }

    private void SelectButton(int index)
    {
        // Perform the action associated with the selected button
        if (index >= 0 && index < _buttons.Length)
        {
            _buttons[index].onClick.Invoke();
        }
    }

    
    public void ClearChoices()
    {
        _choiceDictionary.Clear();
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
    
    public void AddChoice(string choiceName, Action choiceAction)
    {
        _choiceDictionary.Add(choiceName, choiceAction);
    }
    
    private void ClearMenuButtons()
    {
        for (int i = 0; i < buttonGameObjects.Length; i++)
        {
            buttonGameObjects[i].SetActive(false);
        }
    }
    
    public void ShowMenuButtons()
    {
        ClearMenuButtons();
        
        _choiceCount = _choiceDictionary.Count;
        
        for (int i = 0; i < _choiceCount; i++)
        {
            if (i == 4)
            {
                break;
            }
            buttonGameObjects[i].SetActive(true);
        }
    }
    
    public void LoadNewMenu()
    {
        _keys = _choiceDictionary.Keys.ToList();
        
        for (int i = 0; i < _choiceCount; i++)
        {
            if (i == 4)
            {
                break;
            }
            
            string key = _keys[i];
            buttonGameObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = _keys[i];
            _buttons[i].onClick.AddListener(() => AddActionToButton(key));
        }
    }

    private void AddActionToButton(string actionKey)
    {
        if (_choiceDictionary.ContainsKey(actionKey))
        {
            Action selectedAction = _choiceDictionary[actionKey];
            selectedAction.Invoke();
        }
    }
}