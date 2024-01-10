using UnityEngine;
using UnityEngine.Serialization;

public class Input_Controller : MonoBehaviour
{
    public static Input_Controller Instance { get; private set; }
    private  Controls _input;

    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public bool interactPressed;
    [HideInInspector] public bool downMenu; 
    [HideInInspector] public bool upMenu;
    [HideInInspector] public bool leftMenu;
    [HideInInspector] public bool rightMenu;
    
    private void Update()
    {
        moveDirection = _input.Player.Movement.ReadValue<Vector2>();
        interactPressed = _input.Player.Interact.WasPressedThisFrame();
        downMenu = _input.Player.MenuDown.WasPressedThisFrame();
        upMenu = _input.Player.MenuUp.WasPressedThisFrame();
        leftMenu = _input.Player.MenuLeft.WasPressedThisFrame();
        rightMenu = _input.Player.MenuRight.WasPressedThisFrame();
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