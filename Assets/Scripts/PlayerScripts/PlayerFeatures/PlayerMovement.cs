using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement variables")] 
    private static bool _frozen = false;
    private float _moveDuration = 0.15f;
    private float _gridSize = 0.5f;
    private bool _isMoving;
    
    [HideInInspector] public Vector2 moveVector = Vector2.zero;
    
    private void Start()
    {
        GameEvents.playerFrozen += FreezePlayer;
        GameEvents.playerUnfrozen += UnfreezePlayer;
        
        GameEvents.up += MoveUp;
        GameEvents.down += MoveDown;
        GameEvents.left += MoveLeft;
        GameEvents.right += MoveRight;
    }

    private void OnDestroy()
    {
        GameEvents.playerFrozen -= FreezePlayer;
        GameEvents.playerUnfrozen -= UnfreezePlayer;
        
        GameEvents.up -= MoveUp;
        GameEvents.down -= MoveDown;
        GameEvents.left -= MoveLeft;
        GameEvents.right -= MoveRight;
    }

    private void MoveUp()
    {
        if (!_isMoving && !_frozen)
        {
            StartCoroutine(Move(Vector2.up));
        }
    }
    
    private void MoveDown()
    {
        if (!_isMoving && !_frozen)
        {
            StartCoroutine(Move(Vector2.down));
        }   
    }
    
    private void MoveLeft()
    {
        if (!_isMoving && !_frozen)
        {
            StartCoroutine(Move(Vector2.left));
        }
    }
    
    private void MoveRight()
    {
        if (!_isMoving && !_frozen)
        {
            StartCoroutine(Move(Vector2.right));
        }
    }
    
    private IEnumerator Move(Vector2 direction)
    {
        _isMoving = true;
        moveVector = direction;
        
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * _gridSize);
        
        float elapsedTime = 0;
        while (elapsedTime < _moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, (elapsedTime / _moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition;
        
        moveVector = Vector2.zero;
        _isMoving = false; 
    }

    private void FreezePlayer()
    {
        _frozen = true;
    }

    private void UnfreezePlayer()
    {
        _frozen = false;
    }
}
