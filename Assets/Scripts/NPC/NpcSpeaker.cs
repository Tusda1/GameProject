using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using TMPro;
using UnityEngine.Serialization;

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
    [HideInInspector]
    public int keepLinesValue;
    [HideInInspector] 
    public bool waitingForCombatEnd;
    [HideInInspector] 
    public bool DialogueUIStatusLocked;
    
    private bool _conversationRunning;
    
    [Header("Components")]
    private SpriteRenderer _spriteRenderer;
    private TextArchitect _architect;
    
    [Header("Coroutines")]
    private Coroutine _speakingCoroutine;
    
    [Header("External References")]
    public CharacterBase npc;
    [HideInInspector]
    public TextMeshProUGUI text;
    
    [Header("OtherScripts")]
    [HideInInspector] public NpcMovement npcMovement;
    // Start is called before the first frame update
    void Start()
    {
        npcMovement = GetComponent<NpcMovement>();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _spriteRenderer.sprite = npc.sprite;
        }
        
        text = DialogueBoxManager.Instance.dialogueBox;
        
        _architect = new TextArchitect(text);
    }

    private void OnEnable()
    {
        CombatEvents.combatEnd += OnCombatEnd;
    }
    
    private void OnDisable()
    {
        CombatEvents.combatEnd -= OnCombatEnd;
    }

    public void Interact()
    {
        if (!_conversationRunning)
        {
            StartConversation();
        }
    }
    
    public void ProgressDialogue()
    {
        if (waitingForSignal)
            return;
        if (_waitingForUserInput)
            userInput = true;
    }
    
    public void DisplayDialogueUI()
    {
        if (DialogueUIStatusLocked)
            return;
        
        DialogueBoxManager.Instance.ShowDialogueContainers();
    }
    
    public void HideDialogueUI()
    {
        if (DialogueUIStatusLocked)
            return;
        
        DialogueBoxManager.Instance.HideDialogueContainers();
    }
    
    private void StartConversation()
    {
        CurrentSpeaker = this;
        DisplayDialogueUI();
        
        PlayerEvents.playerFrozen?.Invoke();
        
        whichDialogue = NpcDialogueHandler.GetWhichDialogue(npc.characterName);
        dialogueDict = NpcDialogueHandler.GetDialogue(npc.characterName);
        
        _conversationRunning = true;
        _speakingCoroutine ??= StartCoroutine(Speaking());
        
        GameEvents.interactPressed += ProgressDialogue;
    }
    
    public void EndConversation()
    {
        print("Ending Conversation");
        StopCoroutine(_speakingCoroutine);
        text.text = "";
        _speakingCoroutine = null;
        _conversationRunning = false;
        CurrentSpeaker = null;
        HideDialogueUI();
        GameEvents.interactPressed -= ProgressDialogue;
        print("Unfreezing player");
        if (!GameManager.Instance.inCombat)
            PlayerEvents.playerUnfrozen?.Invoke();
    }
    
    public void StartNextDialogue()
    {
        StopCoroutine(_speakingCoroutine);
        text.text = "";
        _speakingCoroutine = null;
        
        whichDialogue = NpcDialogueHandler.GetWhichDialogue(npc.characterName);
        dialogueDict = NpcDialogueHandler.GetDialogue(npc.characterName);
        _speakingCoroutine ??= StartCoroutine(Speaking());
    }
    
    private void OnCombatEnd()
    {
        if (!waitingForCombatEnd)
            return;
        
        waitingForCombatEnd = false;
        StartConversation();
    }
    
    public void DestroyNpc()
    {
        print("Destroying Npc");
        EndConversation();
        print("Finalizing destruction");
        Destroy(gameObject);
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
        foreach (var segment in line.Segments)
        {
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
