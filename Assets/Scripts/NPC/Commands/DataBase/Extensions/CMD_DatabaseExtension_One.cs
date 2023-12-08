using System;
using DIALOGUE;
using UnityEngine;

public class CMD_DatabaseExtension_One : CMD_Database_Extension
{
    new public static void Extend(CommandDatabase database)
    {
        //Add COmmand with no parameters
        database.AddCommand("Print",new Action(PrintDefaultMessage));
        
        database.AddCommand("Choices", new Action<string[]>(Choices));
        
        database.AddCommand("NextDialogue",new Action(NextDialogue));
        
        database.AddCommand("NextScene", new Action<string>(NextScene));
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing message to console");
    }

    private static void Choices(string[] options)
    {
        
    }

    private static void NextDialogue()
    {
        NpcDialogueHandler.ChangeWhichDialogue(NpcSpeaker.CurrentSpeaker.npc.npcName, 1);
    }

    private static void NextScene(string nextSceneInGame)
    {

    }
}