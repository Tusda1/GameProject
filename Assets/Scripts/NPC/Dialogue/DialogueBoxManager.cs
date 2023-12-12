using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

public class DialogueBoxManager : MonoBehaviour
{
    public static DialogueBoxManager Instance { get; private set; }
    
    private List<GameObject> dialogueUI = new List<GameObject>();
    public Image portraitNpcImage;

    private void Start()
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
        
        GameObject[] UIObjects = GameObject.FindGameObjectsWithTag("DialogueUI");
        dialogueUI.AddRange(UIObjects);
        
        HideDialogueContainers();
    }

    public void ShowDialogueContainers(Sprite npcPortrait)
    {
        foreach (GameObject UIElement in dialogueUI)
        {
            UIElement.SetActive(true);
        }

        portraitNpcImage.sprite = npcPortrait;
    }

    public void HideDialogueContainers()
    {
        foreach (GameObject UIElement in dialogueUI)
        {
            UIElement.SetActive(false);
        }
    }
}
