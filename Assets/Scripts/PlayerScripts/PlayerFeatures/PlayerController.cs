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
        var facingDir = _playerAnimation.GetFacingDirection()/2;
        var interactPos = transform.position + facingDir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}