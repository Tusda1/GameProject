using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    
    [Header("Movement variables")] 
    private static bool _frozen = false;
    private float _moveSpeed = 3;
    [HideInInspector] public Vector2 moveVector = Vector2.zero;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        GameEvents.playerFrozen += FreezePlayer;
        GameEvents.playerUnfrozen += UnfreezePlayer;
    }
    
    public void UpdateMovement(Vector2 moveDirection)
    {
        if (_frozen)
        {
            moveVector = Vector2.zero;
            _rb.velocity = moveVector * _moveSpeed;
        }
        else
        {
            moveVector = moveDirection;
            _rb.velocity = moveVector * _moveSpeed;
        }
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
