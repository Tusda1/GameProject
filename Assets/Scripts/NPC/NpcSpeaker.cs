using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;

public class NpcSpeaker : MonoBehaviour, Interactable
{
    public static NpcSpeaker CurrentSpeaker { get; private set; }

    [Header("Conversation Management")]
    public List<List<string>> dialogueDict;
    [HideInInspector]
    public int whichDialogue;
    [HideInInspector]
    public bool userInput;
    private bool _waitingForUserInput;
    [HideInInspector]
    public bool waitingForSignal;
    [HideInInspector]
    public int currentDialogueLine;
    [HideInInspector]
    public bool treatCommandAsDialogue;
    [HideInInspector] 
    public int lockedOptions;
    
    private bool _conversationRunning;
    
    [Header("Components")]
    private SpriteRenderer _spriteRenderer;
    private TextArchitect _architect;
    
    [Header("Coroutines")]
    private Coroutine _speakingCoroutine;
    
    [Header("External References")]
    public CharacterBase npc;
    public TextMeshProUGUI text;
    
    [Header("OtherScripts")]
    [HideInInspector] public NpcMovement npcMovement;
    // Start is called before the first frame update
    void Start()
    {
        npcMovement = GetComponent<NpcMovement>();
        
        NpcDialogueHandler.ResetDialogue();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = npc.sprite;
        }
        
        text = DialogueBoxManager.Instance.dialogueBox;
        
        _architect = new TextArchitect(text);
    }

    // Update is called once per frame
    void Update()
    {
        if (userInput && !_conversationRunning)
        {
            userInput = false;
            StartSpeaking();
        }
    }

    public void Interact()
    {
        if (!_conversationRunning)
        {
            userInput = true;
        }
    }
    
    private void ProgressDialogue()
    {
        if (waitingForSignal)
            return;
        if (_waitingForUserInput)
            userInput = true;
    }

    public void EndConversation()
    {
        StopCoroutine(_speakingCoroutine);
        text.text = "";
        _speakingCoroutine = null;
        _conversationRunning = false;
        CurrentSpeaker = null;
        DialogueBoxManager.Instance.HideDialogueContainers();
        GameEvents.playerUnfrozen?.Invoke();
        GameManager.Instance.DialogueScriptsOff();
        GameEvents.interactPressed -= ProgressDialogue;
    }
    
    private void StartSpeaking()
    {
        CurrentSpeaker = this;
        GameManager.Instance.DialogueScriptsOn();
        DialogueBoxManager.Instance.ShowDialogueContainers();
        
        GameEvents.playerFrozen?.Invoke();
        
        whichDialogue = NpcDialogueHandler.GetWhichDialogue(npc.characterName);
        dialogueDict = NpcDialogueHandler.GetDialogue(npc.characterName);
        
        _conversationRunning = true;
        if (_speakingCoroutine == null)
        {
            _speakingCoroutine = StartCoroutine(Speaking());
        }
        
        GameEvents.interactPressed += ProgressDialogue;
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

            if (line.hasDialogue || treatCommandAsDialogue)
            {
                yield return WaitForUserInput();
                treatCommandAsDialogue = false;
            }
        }
        
        EndConversation();
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
            
        while (_architect.IsBuilding)
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
        _waitingForUserInput = true;
        while (!userInput || waitingForSignal)
        {
            yield return null;
        }
        _waitingForUserInput = false;
        userInput = false;
    }
}
