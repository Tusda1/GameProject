using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class NpcDialogueHandler
{
    //This method is used to get the needed dialogue for the npc you are talking to
    public static List<List<string>> GetDialogue(string keyName)
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return null;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JObject npc = jsonObject["npcs"].FirstOrDefault(n => (string)n["name"] == keyName) as JObject;

        if (npc == null)
        {
            Debug.LogError("NPC not found with the given name: " + keyName);
            return null;
        }

        JObject dialogueObject = (JObject)npc["dialogue"];

        if (dialogueObject == null)
        {
            Debug.LogError("Dialogue object not found in NPC.");
            return null;
        }

        List<List<string>> allDialogues = new List<List<string>>();

        foreach (var dialogueList in dialogueObject)
        {
            JArray dialogues = (JArray)dialogueList.Value;
            List<string> dialogueStrings = dialogues.ToObject<List<string>>();
            allDialogues.Add(dialogueStrings);
        }

        return allDialogues;
    }

    
    //This method is used to get a variable called whichdialogue which keeps track of what dialogue you are meant to hear next
    public static int GetWhichDialogue(string npcName)
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return -1; // Return a default value or handle error cases as needed
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JObject npc = jsonObject["npcs"].FirstOrDefault(n => (string)n["name"] == npcName) as JObject;

        if (npc == null)
        {
            Debug.LogError("NPC not found with the given name: " + npcName);
            return -1; // Return a default value or handle error cases as needed
        }

        if (!npc.TryGetValue("WhichDialogue", out JToken currentDialogToken))
        {
            Debug.LogError("WhichDialogue key not found for NPC: " + npcName);
            return -1; // Return a default value or handle error cases as needed
        }

        if (currentDialogToken.Type != JTokenType.Integer)
        {
            Debug.LogError("Unexpected data type for WhichDialogue for NPC: " + npcName);
            return -1; // Return a default value or handle error cases as needed
        }

        return (int)currentDialogToken;
    }
    

    public static void ChangeWhichDialogue(string npcName, int newValue)
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JObject npc = jsonObject["npcs"].FirstOrDefault(n => (string)n["name"] == npcName) as JObject;

        if (npc == null)
        {
            Debug.LogError("NPC not found with the given name: " + npcName);
            return;
        }

        if (!npc.TryGetValue("WhichDialogue", out JToken currentDialogToken))
        {
            Debug.LogError("WhichDialogue key not found for NPC: " + npcName);
            return;
        }

        if (currentDialogToken.Type != JTokenType.Integer)
        {
            Debug.LogError("Unexpected data type for WhichDialogue for NPC: " + npcName);
            return;
        }

        int currentDialog = (int)currentDialogToken;
        int updatedDialog = currentDialog + newValue;
        npc["WhichDialogue"] = updatedDialog;

        // Write the modified JSON back to the file
        File.WriteAllText(filePath, jsonObject.ToString());

        Debug.Log("Updated WhichDialogue for NPC: " + npcName + " to " + updatedDialog);
    }
    
    
    public static void ResetDialogue()
    {
        string filePath = Path.Combine(Application.dataPath, "TXTFiles/NpcDialogue.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("File does not exist at the specified path: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        JObject jsonObject = JObject.Parse(json);

        JArray npcs = (JArray)jsonObject["npcs"];

        foreach (JObject npc in npcs)
        {
            npc["WhichDialogue"] = 0; // Assigning an integer value instead of a string
        }

        // Write the modified JSON back to the file
        File.WriteAllText(filePath, jsonObject.ToString());

        Debug.Log("Updated WhichDialogue for all NPCs to 0");
    }
    
    public static void DeleteLines(int deleteStart, int deleteAmount)
    {
        deleteStart += NpcSpeaker.CurrentSpeaker.currentDialogueLine;

        NpcSpeaker.CurrentSpeaker.dialogueDict[NpcSpeaker.CurrentSpeaker.whichDialogue].RemoveRange(deleteStart, deleteAmount);
    }
}