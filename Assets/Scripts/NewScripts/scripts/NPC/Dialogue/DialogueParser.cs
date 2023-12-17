using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueParser
    {
        private const string commandRegexPattern = @"\w*[^\s]\(";
        public static DIALOGUE_LINES Parse(string rawLine)
        {
            Debug.Log($"Parsing line - '{rawLine}'");

            (string dialogue, string commands) = RipContent(rawLine);
            
            Debug.Log($"Dialogue = '{dialogue}'\nCommands = '{commands}'");

            return new DIALOGUE_LINES(dialogue, commands);
        }

        private static (string, string) RipContent(string rawLine)
        {
            string dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                    isEscaped = !isEscaped;
                else if (current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        dialogueEnd = i;
                }
                else
                    isEscaped = false;
            }

            Regex commandRegex = new Regex(commandRegexPattern);
            Match match = commandRegex.Match(rawLine);
            int commandStart = -1;
            if (match.Success)
            {
                commandStart = match.Index;
                
                if (dialogueStart == -1 && dialogueEnd == -1)
                    return ("", rawLine.Trim());
            }

            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //DIALOGUEEEE
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"","\"");
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                commands = rawLine;

            return (dialogue, commands);
        }
    }
}