using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement variables")] 
    public static bool frozen;
    private float _moveDuration = 0.15f;
    private float _gridSize = 0.5f;
    private bool _isMoving;
    
    [HideInInspector] public Vector2 moveVector = Vector2.zero;
    
    private void Start()
    {
        PlayerEvents.playerFrozen += FreezePlayer;
        PlayerEvents.playerUnfrozen += UnfreezePlayer;
        
        GameEvents.down += MoveDown;
        GameEvents.up += MoveUp;
        GameEvents.left += MoveLeft;
        GameEvents.right += MoveRight;
        
        GameEvents.movePlayer += StartScriptedMovement;
    }

    private void OnDestroy()
    {
        PlayerEvents.playerFrozen -= FreezePlayer;
        PlayerEvents.playerUnfrozen -= UnfreezePlayer;
        
        GameEvents.down -= MoveDown;
        GameEvents.up -= MoveUp;
        GameEvents.left -= MoveLeft;
        GameEvents.right -= MoveRight;
        
        GameEvents.movePlayer -= StartScriptedMovement; 
    }

    private void MoveUp()
    {
        if (!_isMoving && !frozen)
        {
            StartCoroutine(Move(Vector2.up));
        }
    }
    
    private void MoveDown()
    {
        if (!_isMoving && !frozen)
        {
            StartCoroutine(Move(Vector2.down));
        }   
    }
    
    private void MoveLeft()
    {
        if (!_isMoving && !frozen)
        {
            StartCoroutine(Move(Vector2.left));
        }
    }
    
    private void MoveRight()
    {
        if (!_isMoving && !frozen)
        {
            StartCoroutine(Move(Vector2.right));
        }
    }
    
    private void StartScriptedMovement(int x, int y)
    {
        StartCoroutine(ScriptedMovement(new Vector2(x, y)));
    }

    private IEnumerator ScriptedMovement(Vector2 direction)
    {
        for (int i = 0; i < direction.magnitude; i++)
        {
            yield return StartCoroutine(Move(direction.normalized));
        }
    }
    
    private IEnumerator Move(Vector2 direction)
    {
        _isMoving = true;
        moveVector = direction;
        
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (direction * _gridSize);
        
        if (CheckForCollision(endPosition))
        {
            _isMoving = false;
            StartCoroutine(TurnOffMovement());
            yield break;
        }
        
        float elapsedTime = 0;
        while (elapsedTime < _moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, (elapsedTime / _moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endPosition;
        
        StartCoroutine(TurnOffMovement());
        _isMoving = false; 
    }

    private bool CheckForCollision(Vector2 direction)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(direction, 0.1f);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Player") && !hit.isTrigger)
            {
                return true;
            }
        }
        return false;
    }
    
    private IEnumerator TurnOffMovement()
    {
        yield return new WaitForSeconds(0.05f);
        if (_isMoving)
        {
            yield break;
        }
        moveVector = Vector2.zero;
    }
    
    private void FreezePlayer()
    {
        frozen = true;
    }

    private void UnfreezePlayer()
    {
        frozen = false;
    }
}
