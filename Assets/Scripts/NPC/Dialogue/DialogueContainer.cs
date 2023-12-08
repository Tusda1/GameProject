using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public TextMeshProUGUI dialogueText;
        public NameContainer NameContainer = new NameContainer();
    }
}