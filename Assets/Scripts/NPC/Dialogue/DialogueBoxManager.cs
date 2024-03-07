using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

public class DialogueBoxManager : MonoBehaviour
{
    public static DialogueBoxManager Instance { get; private set; }
    
    private List<GameObject> _dialogueUI = new List<GameObject>();
    public Image portraitNpcImage;
    public TextMeshProUGUI nameTextBox;
    public TextMeshProUGUI dialogueBox;

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
        GameObject[] uiObjects = GameObject.FindGameObjectsWithTag("DialogueVisualBoxes");
        _dialogueUI.AddRange(uiObjects);
        
        HideDialogueContainers();
    }

    public void ShowDialogueContainers()
    {
        foreach (GameObject uiElement in _dialogueUI)
        {
            uiElement.SetActive(true);
        }

        nameTextBox.text = NpcSpeaker.CurrentSpeaker.npc.characterName;
        portraitNpcImage.sprite = NpcSpeaker.CurrentSpeaker.npc.portrait;

        if (NpcSpeaker.CurrentSpeaker.npc.portrait != null)
        {
            float aspectRatio = portraitNpcImage.sprite.rect.height / portraitNpcImage.sprite.rect.width;

            // Set the height to maintain aspect ratio while changing width
            portraitNpcImage.rectTransform.sizeDelta = new Vector2(portraitNpcImage.rectTransform.sizeDelta.y / aspectRatio, portraitNpcImage.rectTransform.sizeDelta.y);   
        }
        else
        {
            portraitNpcImage.gameObject.SetActive(false);
        }
    }
    
    public void DisplayName(string currentName)
    {
        nameTextBox.text = currentName;
    }
    
    public void DisplayPortrait(Sprite portrait)
    {
        portraitNpcImage.sprite = portrait;
    }

    public void HideDialogueContainers()
    {
        foreach (GameObject uiElement in _dialogueUI)
        {
            uiElement.SetActive(false);
        }
    }
}
