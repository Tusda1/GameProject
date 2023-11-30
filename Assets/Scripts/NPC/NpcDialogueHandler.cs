using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class NpcDialogueHandler
{
    public static List<List<string>> GetDialogue()
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            JObject jsonObject = JObject.Parse(json);

            if (jsonObject != null)
            {
                // Accessing the "dialogue" object
                JObject dialogueObject = (JObject)jsonObject["npcs"][0]["dialogue"]; // Change index as needed

                if (dialogueObject != null)
                {
                    List<List<string>> allDialogues = new List<List<string>>();

                    foreach (var dialogueList in dialogueObject)
                    {
                        JArray dialogues = (JArray)dialogueList.Value;
                        List<string> dialogueStrings = dialogues.ToObject<List<string>>();
                        allDialogues.Add(dialogueStrings);
                    }

                    return allDialogues;
                }
                else
                {
                    Debug.LogError("Dialogue object not found in JSON.");
                }
            }
            else
            {
                Debug.LogError("Failed to parse JSON.");
            }
        }
        else
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
        }

        return null;
    }
}