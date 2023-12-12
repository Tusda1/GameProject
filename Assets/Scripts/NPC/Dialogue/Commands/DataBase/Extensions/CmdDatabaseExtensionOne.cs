using System;
using DIALOGUE;
using UnityEngine;

public class CmdDatabaseExtensionOne : CMD_Database_Extension
{
    new public static void Extend(CommandDatabase database)
    {
        //Add Command with no parameters
        database.AddCommand("Print",new Action(PrintDefaultMessage));
        
        database.AddCommand("Choices", new Action<string[]>(Choices));
        
        database.AddCommand("DeleteLines",new Action<string[]>(DeleteLines));
        
        database.AddCommand("Leave", new Action(LeaveConversation));
        
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
    
    //Used for deleting the route that wasn't chosen after a choice.
    //The parameter is an array of the starts and lengths of the different routes. It will use this to delete the routes that weren't chosen.
    private static void DeleteLines(string[] deleteStartAndAmount)
    {
        //Decrements buttonPressed by 1 so that the first button becomes 0 in order to match the index of an array.
        int optionChosen = ButtonManager.buttonPressed - 1;
        
        //For loops that deletes all routes except the one chosen
        for (int i = 0; i < (deleteStartAndAmount.Length/2); i++)
        {
            //Makes sure i is not equal to optionChosen. If it is equal it will not delete that route.
            if (i != optionChosen)
            {
                NpcDialogueHandler.DeleteLines(int.Parse(deleteStartAndAmount[i * 2]), int.Parse(deleteStartAndAmount[(i * 2) + 1]));
            }
        }
    }

    private static void LeaveConversation()
    {
        NpcSpeaker.CurrentSpeaker.EndConversation();
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