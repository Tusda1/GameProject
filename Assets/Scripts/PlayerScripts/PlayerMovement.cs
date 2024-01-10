using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Input_Controller _input;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    private float _moveSpeed = 10;

    private void Awake()
    {
        _input = GetComponent<Input_Controller>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _moveVector = _input.moveDirection;
        
        _rb.velocity = _moveVector * _moveSpeed;
    }
}
