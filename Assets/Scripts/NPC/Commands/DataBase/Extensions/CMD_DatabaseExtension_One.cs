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
        
        database.AddCommand("DeleteLines",new Action<string[]>(DeleteLines));
        
        database.AddCommand("NextDialogue",new Action(NextDialogue));
        
        database.AddCommand("NextScene", new Action<string>(NextScene));
    }

    //Used only for testing
    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing message to console");
    }

    //Used for activating a choice.
    private static void Choices(string[] options)
    {
        ButtonManager.DisplayButtons(options);
        NpcSpeaker.CurrentSpeaker.waitingForResponse = true;
    }
    
    //Used for deleting the route that wasnt chosen after a choice.
    private static void DeleteLines(string[] deleteStartAndAmount)
    {
        
    }

    //Used at the end of a dialogue to move on to the next dialogue.
    private static void NextDialogue()
    {
        NpcDialogueHandler.ChangeWhichDialogue(NpcSpeaker.CurrentSpeaker.npc.npcName, 1);
    }

    //Used if you want to move to a different scene.
    private static void NextScene(string nextSceneInGame)
    {

    }
}