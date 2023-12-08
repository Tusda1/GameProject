using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDialogueTest : MonoBehaviour
{
    private bool _playerInRange = false;
    
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
        }
    }

    private void Update()
    {
        if (_playerInRange & PlayerInteraction.Instance.IsInteractPressed)
        {
            NpcDialogueHandler.ResetDialogue();
        }
    }
}
