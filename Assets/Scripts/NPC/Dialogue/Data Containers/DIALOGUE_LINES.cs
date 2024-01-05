using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DIALOGUE_LINES
    {
        public DL_DialogueData dialogueData;
        public DL_COMMAND_DATA commandData;
        
        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandData != null;

        public DIALOGUE_LINES(string dialogue, string commands)
        {
            this.dialogueData = (string.IsNullOrWhiteSpace(dialogue) ? null : new DL_DialogueData(dialogue));
            this.commandData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands));
        }
    }
}