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

    public void Pressed(int buttonPressed)
    {
        //This is a workaround as unity wont allow a method with an enum as a parameter to be attached to a button
        if (buttonPressed == 1)
            ButtonManager.buttonPressed = ButtonManager.buttons.Button1;
        if (buttonPressed == 2)
            ButtonManager.buttonPressed = ButtonManager.buttons.Button2;
        if (buttonPressed == 3)
            ButtonManager.buttonPressed = ButtonManager.buttons.Button3;
        if (buttonPressed == 4)
            ButtonManager.buttonPressed = ButtonManager.buttons.Button4;
        
        Debug.Log(ButtonManager.buttonPressed);
        NpcSpeaker.CurrentSpeaker.waitingForResponse = false;
        ButtonManager.HideButtons();
    }
}
