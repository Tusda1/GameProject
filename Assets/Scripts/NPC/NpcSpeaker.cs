using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DIALOGUE;

public class NpcSpeaker : MonoBehaviour
{
    private List<List<string>> _dialogueDict;
    private bool _playerInRange;
    private int _whichDialogue;
    private bool _conversationRunning;
    private bool _userInput;
    private bool _waitingForResponse;
    
    private TextArchitect _architect;
    
    private Coroutine _speakingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _dialogueDict = NpcDialogueHandler.GetDialogue(NpcManager.instance.npc.npcName);
        _architect = new TextArchitect(NpcManager.instance.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInteraction.Instance.IsInteractPressed)
            _userInput = true;
        else
            _userInput = false;
        
        if (_userInput & !_conversationRunning & _playerInRange )
        {
            _userInput = false;
            StartSpeaking();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            _playerInRange = false;

            if (_speakingCoroutine != null) // If a conversation is in progress when the player leaves the zone
            {
                StopCoroutine(_speakingCoroutine);
                NpcManager.instance.text.text = "";
                _speakingCoroutine = null;
                _conversationRunning = false;
            }
        }
    }
    
    private void StartSpeaking()
    {
        _conversationRunning = true;
        if (_speakingCoroutine == null)
        {
            _speakingCoroutine = StartCoroutine(Speaking());
        }
    }

    private IEnumerator Speaking()
    {
        for (int i = 0; i < _dialogueDict[_whichDialogue].Count; i++)
        {
            DIALOGUE_LINES line = DialogueParser.Parse(_dialogueDict[_whichDialogue][i]);

            if (line.hasDialogue)
                yield return Line_RunDialogue(line);

            if (line.hasCommands)
                yield return Line_RunCommands(line);
                
            if (line.hasDialogue)
                yield return WaitForUserInput();
            
            if (!_playerInRange) // Check if the player has left the NPC range during speaking
            {
                break; // Exit the coroutine if the player is no longer in range
            }
        }
    }
    
    IEnumerator Line_RunDialogue(DIALOGUE_LINES line)
    {
        yield return BuildLineSegments(line.dialogueData);
    }
    
    IEnumerator BuildLineSegments(DL_DialogueData line)
    {
        for (int i = 0; i < line.Segments.Count; i++)
        {
            DL_DialogueData.DIALOGUE_SEGMENT segment = line.Segments[i];

            yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

            yield return BuildDialogue(segment.dialogue, segment.appendText);
        }
    }
    
    IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DialogueData.DIALOGUE_SEGMENT segment)
    {
        switch (segment.startSignal)
        {
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.C:
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.A:
                yield return WaitForUserInput();
                break;
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WC:
            case DL_DialogueData.DIALOGUE_SEGMENT.StartSignal.WA:
                yield return new WaitForSeconds(segment.signalDelay);
                break;
            default:
                break;
        }
    }
    
    IEnumerator BuildDialogue(string dialogue, bool append = false)
    {
        if (!append)
            _architect.Build(dialogue);
        else
            _architect.Append(dialogue);
            
        while (_architect.isBuilding)
        {
            yield return null;
        }
    }
    
    IEnumerator Line_RunCommands(DIALOGUE_LINES line)
    {
        List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

        foreach (DL_COMMAND_DATA.Command command in commands)
        {
            CommandManager.instance.Execute(command.name, command.arguments);
        }
        yield return null;
    }

    private IEnumerator WaitForUserInput()
    {
        while (!_userInput)
            yield return null;
        
        _userInput = false;
    }

    private void ChoiceMade()
    {
        
    }
}
