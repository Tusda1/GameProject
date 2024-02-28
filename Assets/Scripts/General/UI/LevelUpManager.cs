using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private ButtonManouverer _buttonManouverer;
    
    private Dictionary<StatEnum, int> _statDictionary = new Dictionary<StatEnum, int>();
    private int _totalStatPoints = 5;
    private int _remainingStatPoints = 5;
    [SerializeField] private TextMeshProUGUI remainingPointsText;

    private void Start()
    {
        _statDictionary.Add(StatEnum.Attack, 0);
        _statDictionary.Add(StatEnum.Magic, 0);
        _statDictionary.Add(StatEnum.Speed, 0);
        
        _buttonManouverer = new ButtonManouverer(buttons, this);
        
        //Set all text values to their respective values
        foreach (var element in _statDictionary)
        {
            buttons[(int) element.Key].GetComponentInChildren<TextMeshProUGUI>().text = element.Value.ToString();
        }
        
        remainingPointsText.text = _totalStatPoints.ToString();
    }
    
    public void IncreaseStat(int buttonIndex)
    {
        if (_remainingStatPoints > 0)
        {
            StatEnum stat = (StatEnum)buttonIndex;

            _statDictionary[stat]++;

            _remainingStatPoints--;

            remainingPointsText.text = _remainingStatPoints.ToString();
            buttons[_buttonManouverer.currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>().text =
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
        _remainingStatPoints = _totalStatPoints;
        remainingPointsText.text = _remainingStatPoints.ToString();
    }
    
    public void ExitLevelUp()
    {
        foreach (var stat in _statDictionary.Keys)
        {
            PlayerDataHolder.IncreaseStat(stat.ToString(), _statDictionary[stat]);
        }
        
        _buttonManouverer.CleanUp();
        _buttonManouverer = null;
        
        GameEvents.endLevelUp?.Invoke();
    }
}
