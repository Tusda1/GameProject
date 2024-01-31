using UnityEngine;
using UnityEngine.Serialization;

public class Input_Controller : MonoBehaviour
{
    public static Input_Controller Instance { get; private set; }
    private  Controls _input;

    [HideInInspector] public Vector2 moveDirection;
    
    private void Update()
    {
        moveDirection = _input.Player.Movement.ReadValue<Vector2>();

        if (_input.Player.Interact.WasPressedThisFrame())
            GameEvents.interactPressed?.Invoke();
        if (_input.Player.MenuDown.WasPressedThisFrame())
            GameEvents.downMenu?.Invoke();
        if (_input.Player.MenuUp.WasPressedThisFrame())
            GameEvents.upMenu?.Invoke();
        if (_input.Player.MenuLeft.WasPressedThisFrame())
            GameEvents.leftMenu?.Invoke();
        if (_input.Player.MenuRight.WasPressedThisFrame())
            GameEvents.rightMenu?.Invoke();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Rest of your initialization code...
        }
        else
        {
            Destroy(gameObject); // Ensures there's only one instance
        }
        
        _input = new Controls();
    }
    
    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}