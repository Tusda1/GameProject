using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class NpcDialogueHandler
{
    public static List<List<string>> GetDialogue(string keyName)
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            JObject jsonObject = JObject.Parse(json);

            if (jsonObject != null)
            {
                // Find the NPC with the matching name
                JObject npc = jsonObject["npcs"].FirstOrDefault(n => (string)n["name"] == keyName) as JObject;

                if (npc != null)
                {
                    JObject dialogueObject = (JObject)npc["dialogue"];

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
                        Debug.LogError("Dialogue object not found in NPC.");
                    }
                }
                else
                {
                    Debug.LogError("NPC not found with the given name: " + keyName);
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

    public static void ChangeWhichDialogue(string npcName, string newValue)
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            JObject jsonObject = JObject.Parse(json);

            if (jsonObject != null)
            {
                // Find the NPC with the matching name
                JObject npc = jsonObject["npcs"].FirstOrDefault(n => (string)n["name"] == npcName) as JObject;

                if (npc != null)
                {
                    npc["WhichDialogue"] = newValue;

                    // Write the modified JSON back to the file
                    File.WriteAllText(filePath, jsonObject.ToString());

                    Debug.Log("Updated WhichDialogue for NPC: " + npcName + " to " + newValue);
                }
                else
                {
                    Debug.LogError("NPC not found with the given name: " + npcName);
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
    }
    
    public static void ResetDialogue()
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            JObject jsonObject = JObject.Parse(json);

            if (jsonObject != null)
            {
                JArray npcs = (JArray)jsonObject["npcs"];

                foreach (JObject npc in npcs)
                {
                    npc["WhichDialogue"] = "0";
                }

                // Write the modified JSON back to the file
                File.WriteAllText(filePath, jsonObject.ToString());

                Debug.Log("Updated WhichDialogue for all NPCs to 0");
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
    }
}