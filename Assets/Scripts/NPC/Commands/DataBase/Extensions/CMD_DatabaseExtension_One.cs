using System;
using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CMD_DatabaseExtension_One : CMD_Database_Extension
{
    new public static void Extend(CommandDatabase database)
    {
        //Add COmmand with no parameters
        database.AddCommand("Print",new Action(PrintDefaultMessage));
        
        database.AddCommand("Choices", new Action<string[]>(Choices));
        
        database.AddCommand("OneChoice", new Action<string>(OneChoice));
        
        database.AddCommand("NextScene", new Action<string>(NextScene));
    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing message to console");
    }

    private static void Choices(string[] options)
    {

    }

    private static void OneChoice(string text)
    {

    }

    private static void NextScene(string nextSceneInGame)
    {

    }
}