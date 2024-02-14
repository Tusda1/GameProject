using UnityEngine;
using UnityEngine.Serialization;

public class Input_Controller : MonoBehaviour
{
    private  Controls _input;

    [HideInInspector] public Vector2 moveDirection;
    
    private void Update()
    {
        moveDirection = _input.Player.Movement.ReadValue<Vector2>();

        if (_input.Player.Interact.WasPressedThisFrame())
            GameEvents.interactPressed?.Invoke();
        
        if (_input.Player.Down.WasPressedThisFrame())
            GameEvents.downMenu?.Invoke();
        if (_input.Player.Up.WasPressedThisFrame())
            GameEvents.upMenu?.Invoke();
        if (_input.Player.Left.WasPressedThisFrame())
            GameEvents.leftMenu?.Invoke();
        if (_input.Player.Right.WasPressedThisFrame())
            GameEvents.rightMenu?.Invoke();
        
        if (_input.Player.Down.IsPressed())
            GameEvents.down?.Invoke();
        if (_input.Player.Up.IsPressed())
            GameEvents.up?.Invoke();
        if (_input.Player.Left.IsPressed())
            GameEvents.left?.Invoke();
        if (_input.Player.Right.IsPressed())
            GameEvents.right?.Invoke();
    }
    private void Awake()
    {
        _input = new Controls();
    }
    
    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}