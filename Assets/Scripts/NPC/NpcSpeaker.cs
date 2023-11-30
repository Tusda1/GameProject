using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NpcSpeaker : MonoBehaviour
{
    private List<List<string>> _dialogueDict;
    private bool _playerInRange = false;
    private int _whichDialogue;
    private bool _conversationRunning;
    private bool _userInput;
    
    private TextArchitect _architect;
    
    private Coroutine speakingCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        _dialogueDict = NpcDialogueHandler.GetDialogue();
        _architect = new TextArchitect(NpcManager.instance.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInteraction.Instance.IsInteractPressed)
            _userInput = true;
        else if (speakingCoroutine == null)
            _userInput = false;
        
        if (_playerInRange & _userInput & !_conversationRunning)
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

            if (speakingCoroutine != null) // If a conversation is in progress when the player leaves the zone
            {
                StopCoroutine(speakingCoroutine);
                NpcManager.instance.text.text = "";
                speakingCoroutine = null;
                _conversationRunning = false;
            }
        }
    }
    
    private void StartSpeaking()
    {
        _conversationRunning = true;
        if (speakingCoroutine == null)
        {
            speakingCoroutine = StartCoroutine(Speaking());
        }
    }

    private IEnumerator Speaking()
    {
        int howManyStrings = _dialogueDict[_whichDialogue].Count;
        for (int i = 0; i < howManyStrings; i++)
        {
            _architect.Build(_dialogueDict[_whichDialogue][i]);
            yield return WaitForUserInput();
            
            if (!_playerInRange) // Check if the player has left the NPC range during speaking
            {
                break; // Exit the coroutine if the player is no longer in range
            }
        }
    }

    private IEnumerator WaitForUserInput()
    {
        while (!_userInput)
            yield return null;
        
        _userInput = false;
    }
}
