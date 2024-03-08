using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private bool _saveFileExists;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private Button[] buttons;
    private ButtonManouverer _buttonManouverer;
    
    // Start is called before the first frame update
    void Start()
    {
        DataBase.InitializeData();
        
        if (File.Exists(Path.Combine(Application.persistentDataPath, "SavegameCopy.json")))
        {
            _saveFileExists = true;
            Debug.Log("Save file exists.");

            //Sets the alpha of the continue button text to full. Removes transparency to signal that button is clickable
            var textmeshColor = continueText.color;
            textmeshColor.a = 1f;
            continueText.color = textmeshColor;
        }
        
        _buttonManouverer = new ButtonManouverer(buttons, this);
    }

    public void Continue()
    {
        if (_saveFileExists)
        {
            StartGame();
        }
    }
    
    public void NewGame()
    {
        SaveFileHandler.CopySaveDataToPersistentPath();
        NpcDialogueHandler.CopyDialogueDataToPersistentPath();
        
        StartGame();
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    private void StartGame()
    {
        _buttonManouverer.CleanUp();
        _buttonManouverer = null;
        DataBase.InitializeData();
        PlayerDataHolder.InitializePlayerData(DataBase.allPlayerSkills);
        
        Dictionary<string, object> tempLoadingDict = SaveFileHandler.GetSaveFile("PlayerPosition");
        
        print((int)tempLoadingDict["X"].GetType());
        
        GameEvents.loadFirstScene?.Invoke(tempLoadingDict["Scene"].ToString(), new Vector2((float)tempLoadingDict["X"], (float)tempLoadingDict["Y"]));
    }
}
