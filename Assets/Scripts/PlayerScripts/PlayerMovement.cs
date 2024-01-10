using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Controles _input = null;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    private float _moveSpeed = 10;

    private void Awake()
    {
        _input = new Controles();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += OnMovementPerformed;
        _input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= OnMovementPerformed;
        _input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveVector * _moveSpeed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        _moveVector = Vector2.zero;
    }
}
