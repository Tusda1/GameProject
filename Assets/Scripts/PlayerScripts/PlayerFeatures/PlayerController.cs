using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Interaction variables")]
    private bool _interactable;
    private Vector2 _inputAxis;
    private Vector3 _targetPos;
    
    [Header("Scripts")]
    private PlayerAnimation _playerAnimation;
    private PlayerMovement _playerMovement;
    private Input_Controller _inputController;


    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        // Subscribe to events
        GameEvents.interactPressed += Interact;
    }

    private void Update()
    {
        _playerAnimation.UpdateAnimation(_playerMovement.moveVector);
    }

    void Interact()
    {
        if (PlayerMovement.frozen)
            return;
        
        var facingDir = _playerAnimation.GetFacingDirection()/1.5f;
        var interactPos = transform.position + facingDir;
        
        // Trigger interactable interact in every object in overlapcircle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactPos, 0.1f);
        foreach (var collider in colliders)
        {
            var interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                break;
            }
        }
    }
}