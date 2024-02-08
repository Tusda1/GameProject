using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    private bool _highlighted;
    
    [SerializeField] private Button[] buttons;
    private int _currentButtonIndex; // Index of the currently selected button
    
    private Dictionary<StatEnum, int> _statDictionary = new Dictionary<StatEnum, int>();
    private int _remainingStatPoints = 5;
    [SerializeField] private TextMeshProUGUI remainingPointsText;

    private void Start()
    {
        _statDictionary.Add(StatEnum.Attack, 0);
        _statDictionary.Add(StatEnum.Magic, 0);
        _statDictionary.Add(StatEnum.Speed, 0);
        _statDictionary.Add(StatEnum.Defense, 0);
        
        HighlightButton();
        
        //Set all textvalues to their respective values
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _statDictionary[(StatEnum)i].ToString();
            if (i == 3)
            {
                break;
            }
        }
        
        remainingPointsText.text = _remainingStatPoints.ToString();
    }

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
        _currentButtonIndex = (_currentButtonIndex - 1 + buttons.Length) % buttons.Length;
        HighlightButton();
    }

    private void MoveToNextButton()
    {
        // Move to the next button in the array
        _currentButtonIndex = (_currentButtonIndex + 1) % buttons.Length;
        HighlightButton();
    }

    private void SelectButton()
    {
        // Perform the action associated with the selected button
        buttons[_currentButtonIndex].onClick.Invoke();
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
        ColorBlock selectedColors = buttons[_currentButtonIndex].colors;
        selectedColors.normalColor = Color.blue; // Change this color to suit your needs
        buttons[_currentButtonIndex].colors = selectedColors;
    }
    
    public void IncreaseStat(int buttonIndex)
    {
        if (_remainingStatPoints > 0)
        {
            StatEnum stat = (StatEnum)buttonIndex;

            _statDictionary[stat]++;

            _remainingStatPoints--;

            remainingPointsText.text = _remainingStatPoints.ToString();
            buttons[_currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>().text =
                _statDictionary[stat].ToString();
        }
    }
    
    public void ResetPoints()
    {
        foreach (var stat in _statDictionary.Keys.ToArray())
        {
            _statDictionary[stat] = 0;
            buttons[(int) stat].GetComponentInChildren<TextMeshProUGUI>().text = "0";
        }
        _remainingStatPoints = 10;
        remainingPointsText.text = _remainingStatPoints.ToString();
    }
    
    public void ExitLevelUp()
    {
        foreach (var stat in _statDictionary.Keys)
        {
            PlayerDataHolder.IncreaseStat(stat.ToString(), _statDictionary[stat]);
        }
        
        GameEvents.endLevelUp?.Invoke();
    }
}
