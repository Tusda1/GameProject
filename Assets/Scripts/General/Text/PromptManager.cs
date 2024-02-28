using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private GameObject promptPanel;
    
    private void Start()
    {
        GameEvents.showPrompt += ShowPrompt;
        GameEvents.hidePrompt += HidePrompt;
        
        HidePrompt("");
    }

    private void ShowPrompt(string prompt)
    {
        GameEvents.playerFrozen?.Invoke();
        promptText.text = prompt;
        promptPanel.SetActive(true);
    }
    
    private void HidePrompt(string prompt)
    {
        GameEvents.playerUnfrozen?.Invoke();
        promptPanel.SetActive(false);
    }
}
