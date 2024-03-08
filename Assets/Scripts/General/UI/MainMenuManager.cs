using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private bool _saveFileExists;
    [SerializeField] private TextMeshProUGUI continueText;
    
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
        
        StartGame();
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    private void StartGame()
    {
        DataBase.InitializeData();
        PlayerDataHolder.InitializePlayerData(DataBase.allPlayerSkills);
        SceneManager.LoadScene("Bedroom");
    }
}
