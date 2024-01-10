using UnityEngine;

public class Input_Controller : MonoBehaviour
{
    public static Input_Controller Instance { get; private set; }
    private  Controls _input;

    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public bool interactPressed;
    [HideInInspector] public bool nextOption;
    [HideInInspector] public bool previousOption;
    
    private void Update()
    {
        moveDirection = _input.Player.Movement.ReadValue<Vector2>();
        interactPressed = _input.Player.Interact.WasPressedThisFrame();
        nextOption = _input.Player.NextDialogueOption.WasPressedThisFrame();
        previousOption = _input.Player.PreviousDialogueOption.WasPressedThisFrame();
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