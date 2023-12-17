using UnityEngine;

public class Input_Controler : MonoBehaviour
{
    public static Input_Controler Instance { get; private set; }
    private  Controles _input;

    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public bool interactPressed;
    [HideInInspector] public bool openNoteBook;
    [HideInInspector] public bool nextOption;
    [HideInInspector] public bool previousOption;
    [HideInInspector] public bool bookDirection;
    
    private void Update()
    {
        moveDirection = _input.Player.move.ReadValue<Vector2>();
        bookDirection = _input.Player.move.triggered;
        interactPressed = _input.Player.Interackt.WasPressedThisFrame();
        openNoteBook = _input.Player.openNotebook.WasPressedThisFrame();
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
        
        _input = new Controles();
    }
    
    private void OnEnable() { _input.Enable(); }
    private void OnDisable() { _input.Disable(); }
}