using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

public class NpcSpeaker : MonoBehaviour
{
    public static NpcSpeaker CurrentSpeaker { get; private set; }
    

    public List<List<string>> dialogueDict;
    [HideInInspector]
    public int whichDialogue;
    [HideInInspector]
    public bool userInput;
    
    private bool _playerInRange;
    private bool _conversationRunning;
    public bool waitingForResponse;
    public int currentDialogueLine;
    
    private TextArchitect _architect;
    
    private Coroutine _speakingCoroutine;
    
    public NpcScrub npc;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        NpcDialogueHandler.ResetDialogue();
        _architect = new TextArchitect(text);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInteraction.Instance.IsInteractPressed & _playerInRange)
            userInput = true;
        
        if (userInput & !_conversationRunning & _playerInRange )
        {
            userInput = false;
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
                text.text = "";
                _speakingCoroutine = null;
                _conversationRunning = false;
                CurrentSpeaker = null;
            }
        }
    }

    public void EndConversation()
    {
        StopCoroutine(_speakingCoroutine);
        text.text = "";
        _speakingCoroutine = null;
        _conversationRunning = false;
        CurrentSpeaker = null;
    }
    
    private void StartSpeaking()
    {
        whichDialogue = NpcDialogueHandler.GetWhichDialogue(npc.npcName);
        dialogueDict = NpcDialogueHandler.GetDialogue(npc.npcName);
        CurrentSpeaker = this;
        _conversationRunning = true;
        if (_speakingCoroutine == null)
        {
            _speakingCoroutine = StartCoroutine(Speaking());
        }
    }

    private IEnumerator Speaking()
    {
        for (int i = 0; i < dialogueDict[whichDialogue].Count; i++)
        {
            currentDialogueLine = i;
            DIALOGUE_LINES line = DialogueParser.Parse(dialogueDict[whichDialogue][i]);

            if (line.hasDialogue)
                yield return Line_RunDialogue(line);

            if (line.hasCommands)
                yield return Line_RunCommands(line);
                
            if (line.hasDialogue)
                yield return WaitForUserInput();
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
        while (!userInput || waitingForResponse)
            yield return null;
        
        userInput = false;
    }
}
