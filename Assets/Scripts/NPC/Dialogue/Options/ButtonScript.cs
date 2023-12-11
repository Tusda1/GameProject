using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private void Start()
    {
        ButtonManager.HideButtons();
    }

    //This is the script that is going to be put on the buttons. The buttons need to be marked 1-4 by using the parameters.
    //This means depending on the button pressed the parameter "buttonPressed" will be a number 1-4 including 1 and 4
    public void Pressed(int buttonPressed)
    {
        //This is a workaround as unity wont allow a method with an enum as a parameter to be attached to a button
        ButtonManager.buttonPressed = buttonPressed;
        
        Debug.Log(ButtonManager.buttonPressed);
        NpcSpeaker.CurrentSpeaker.waitingForResponse = false;
        NpcSpeaker.CurrentSpeaker.userInput = true;
        ButtonManager.HideButtons();
    }
}
