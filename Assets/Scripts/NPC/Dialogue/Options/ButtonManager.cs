using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }
    
    [SerializeField] private GameObject[] buttonList; //
    [SerializeField] private TextMeshProUGUI[] buttonTextList; 
    private Button[] _buttons; // Array to store all the buttons
    
    
    private ButtonManouverer _buttonManouverer;
    
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
    
    public void DisplayButtons(string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            buttonList[i].SetActive(true);
            buttonTextList[i].text = options[i];
        }
        
        _buttons = new Button[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            _buttons[i] = buttonList[i].GetComponent<Button>();
        }

        _buttonManouverer = new ButtonManouverer(_buttons, this);
    }

    public void HideButtons()
    {
        foreach (GameObject button in buttonList)
        {
            button.SetActive(false);
        }
        
        if (_buttonManouverer != null)
        {
            _buttonManouverer.CleanUp();
            _buttonManouverer = null;
        }
    }
}
