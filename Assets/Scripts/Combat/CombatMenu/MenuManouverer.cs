using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManouverer : MonoBehaviour
{
    public static MenuManouverer Instance { get; private set; }
    
    [SerializeField] private GameObject[] buttonGameObjects;
    private Button[] _buttons;
    private int _currentButtonIndex; // Index of the currently selected button
    
    private bool _highlighted;
    
    private Dictionary<string, ActionInfo> _choiceDictionary = new Dictionary<string, ActionInfo>();
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
        
        HighlightButton();
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

    #region MoveCurrentButton
    private void MoveToPreviousButton()
    {
        // Move to the previous button in the array
        if (_currentButtonIndex > 0)
        {
            _currentButtonIndex -= 1;
            HighlightButton();
        }
        else if (_choiceIndex > 0)
        {
            _choiceIndex -= 4;
            LoadNewMenu();
            _currentButtonIndex = 3;
        }
    }

    private void MoveToNextButton()
    {
        // Move to the next button in the array
        if (_currentButtonIndex < _buttons.Length - 1)
        {
            _currentButtonIndex += 1;
            HighlightButton();
        }
        else if (_choiceIndex + 4 < _choiceCount)
        {
            _choiceIndex += 4;
            LoadNewMenu();
        }
    }
    
    private void MoveToDownButton()
    {
        // Move to the previous button in the array
        if (_currentButtonIndex < _buttons.Length - 2)
        {
            _currentButtonIndex += 2;
            HighlightButton();
        }
        else if (_choiceIndex + 5 < _choiceCount)
        {
            _choiceIndex += 4;
            LoadNewMenu();
            if (_currentButtonIndex == 3)
                _currentButtonIndex = 1;
        }
        else if (_choiceIndex + 4 < _choiceCount)
        {
            _choiceIndex += 4;
            LoadNewMenu();
        }
    }
    
    private void MoveToUpButton()
    {
        // Move to the next button in the array
        if (_currentButtonIndex > 1)
        {
            _currentButtonIndex -= 2;
            HighlightButton();
        }
        else if (_choiceIndex > 0)
        {
            _choiceIndex -= 4;
            LoadNewMenu();
            if (_currentButtonIndex == 0)
                _currentButtonIndex = 2;
            else if (_currentButtonIndex == 1)
                _currentButtonIndex = 3;
        }
    }
    #endregion

    private void HighlightButton()
    {
        // Unhighlight all buttons
        foreach (Button button in _buttons)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white; // Change this color to suit your needs
            button.colors = colors;
        }

        // Highlight the selected button
        ColorBlock selectedColors = _buttons[_currentButtonIndex].colors;
        selectedColors.normalColor = Color.blue; // Change this color to suit your needs
        _buttons[_currentButtonIndex].colors = selectedColors;
    }

    private void SelectButton(int index)
    {
        // Perform the action associated with the selected button
        if (index >= 0 && index < _buttons.Length)
        {
            _buttons[index].onClick.Invoke();
        }
    }

    #region ClearAndAddChoices
    //Needs to be called before adding new choices
    public void ClearChoices()
    {
        _choiceDictionary.Clear();
    }
    
    public void AddChoice(string choiceName, Action choiceAction, bool isCombatMove, ActionType actionType)
    {
        _choiceDictionary.Add(choiceName, new ActionInfo(choiceAction, isCombatMove, actionType));
    }
    #endregion

    #region Show new menu
    //Called in ShowMenuButtons
    private void ClearMenuButtons()
    {
        foreach (var buttonGameObject in buttonGameObjects)
        {
            buttonGameObject.SetActive(false);
        }
    }
    
    //Shows the amount of buttons needed up to a maximum of 4
    private void ShowMenuButtons()
    {
        ClearMenuButtons();
        
        _choiceCount = _choiceDictionary.Count;
        
        for (int i = 0; i < _choiceCount - _choiceIndex; i++)
        {
            if (i == 4)
            {
                break;
            }
            buttonGameObjects[i].SetActive(true);
        }
        
        _currentButtonIndex = 0;
        HighlightButton();
    }
    
    //Called in LoadNewMenu
    private void ClearListeners()
    {
        foreach (Button button in _buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void AddNewMenuChoices()
    {
        ClearListeners();
        _keys = _choiceDictionary.Keys.ToList();
        
        for (int i = 0; i < _choiceCount - _choiceIndex; i++)
        {
            if (i == 4)
            {
                break;
            }
            
            string key = _keys[_choiceIndex + i];
            buttonGameObjects[i].GetComponentInChildren<TextMeshProUGUI>().text = _keys[_choiceIndex + i];
            _buttons[_choiceIndex + i].onClick.AddListener(() => AddActionToButton(key));
        }
    }

    //Called in LoadNewMenu
    private void AddActionToButton(string actionKey)
    {
        if (_choiceDictionary.TryGetValue(actionKey, out var ActionInfo))
        {
            Action selectedAction = ActionInfo.action; 
            bool isCombatMove = ActionInfo.isCombatMove;
            
            selectedAction.Invoke();

            if (isCombatMove)
            {
                ClearListeners();
                ClearMenuButtons();
                CombatEvents.playerTurnEnd.Invoke();
            }
        }
    }
    
    public void LoadNewMenu()
    {
        ShowMenuButtons();
        AddNewMenuChoices();
    }
    #endregion
}