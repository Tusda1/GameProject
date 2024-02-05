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
        _inputController = GetComponent<Input_Controller>();
        
        // Subscribe to events
        GameEvents.interactPressed += Interact;
    }

    private void Update()
    {
        _playerAnimation.UpdateAnimation(_playerMovement.moveVector);
    }

    private void FixedUpdate()
    {
        _playerMovement.UpdateMovement(_inputController.moveDirection);
    }

    void Interact()
    {
        var facingDir = _playerAnimation.GetFacingDirection();
        var interactPos = transform.position + facingDir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}