using System.Collections;
using TMPro;
using UnityEngine;

public static class GeneralWritingScript
{
    //[SerializeField] private TextMeshProUGUI textBox;
    //private TextArchitect _architect;
    
    public static IEnumerator WriteText(TextMeshProUGUI textBox, string text)
    {
        TextArchitect architect = new TextArchitect(textBox);
        
        architect.Build(text);
            
        while (architect.isBuilding)
        {
            yield return null;
        }
    }
}
