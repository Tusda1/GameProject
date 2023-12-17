using System;
using System.Collections.Generic;
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
        
        database.AddCommand("CheckRequirements",new Action<string[]>(CheckRequirements));
        
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
    //Parameters are the choices you want to have. Max 4
    private static void Choices(string[] options)
    {
        if (NpcSpeaker.CurrentSpeaker.lockedOptions > 0)
        {
            int newSize = options.Length - NpcSpeaker.CurrentSpeaker.lockedOptions;

            string[] limitedOptions = new string[newSize];
            Array.Copy(options, limitedOptions, newSize);

            // Use truncatedOptions for further processing or assign it back to options if needed
            ButtonManager.DisplayButtons(limitedOptions);

            NpcSpeaker.CurrentSpeaker.lockedOptions = 0;
        }
        else
        {
            ButtonManager.DisplayButtons(options);
        }
        
        NpcSpeaker.CurrentSpeaker.text.text = "";
        NpcSpeaker.CurrentSpeaker.waitingForResponse = true;
        NpcSpeaker.CurrentSpeaker.treatCommandAsDialogue = true;
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

    //The arguments are as follows arg1 = Datatype you want to grab from Savegame.json, arg2 = key you want to grab the value from
    //arg3 = operator you want to use to check the datavalue, arg4 = the integer you want to compare the datavalue to
    //arg5 = the amount of options you want to lock out based on this.
    private static void CheckRequirements(string[] dataCheckparameters)
    {
        foreach (string dataParameter in dataCheckparameters)
        {
            Debug.Log(dataParameter);
        }
        Dictionary<string, object> dataDict = SaveFileHandler.GetSaveFile(dataCheckparameters[0]);

        int requiredVariable = int.Parse(dataDict[dataCheckparameters[1]].ToString());

        bool conditionMet = false;

        switch (dataCheckparameters[2])
        {
            case "==":
                conditionMet = requiredVariable == int.Parse(dataCheckparameters[3]);
                break;
            case "!=":
                conditionMet = requiredVariable != int.Parse(dataCheckparameters[3]);
                break;
            case ">":
                conditionMet = requiredVariable > int.Parse(dataCheckparameters[3]);
                break;
            case "<":
                conditionMet = requiredVariable < int.Parse(dataCheckparameters[3]);
                break;
            case ">=":
                conditionMet = requiredVariable >= int.Parse(dataCheckparameters[3]);
                break;
            case "<=":
                conditionMet = requiredVariable <= int.Parse(dataCheckparameters[3]);
                break;
            default:
                break;
        }

        if (!conditionMet)
        {
            NpcSpeaker.CurrentSpeaker.lockedOptions = int.Parse(dataCheckparameters[4]);
        }
    }

    //Ends the conversation allowing you to move around again
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