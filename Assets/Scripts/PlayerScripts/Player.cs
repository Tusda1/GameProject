using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _animator;
    
    [Header("Property ID's")]
    private int _horizontalParamID;
    private int _verticalParamID;
    private int _isMovingParamID;

    [Header("Movement variables")]
    public static bool frozen = false;
    private float _moveSpeed = 3;
    private Vector2 _moveVector = Vector2.zero;
    private bool _isMoving;
    
    [Header("Interaction variables")]
    private bool _interactable;
    private Vector2 _inputAxis;
    private Vector3 _targetPos;
    public Vector2 facing;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        // Cache parameter IDs
        _horizontalParamID = Animator.StringToHash("Horizontal");
        _verticalParamID = Animator.StringToHash("Vertical");
        _isMovingParamID = Animator.StringToHash("IsMoving");
        
        // Subscribe to events
        GameEvents.interactPressed += Interact;
    }
    
    private void FixedUpdate()
    {
        if (frozen)
        {
            _moveVector = Vector2.zero;
            _rb.velocity = _moveVector * _moveSpeed;
        }
        
        else
        {
            _moveVector = Input_Controller.Instance.moveDirection;
            _rb.velocity = _moveVector * _moveSpeed;
        }

        if (_moveVector != Vector2.zero)
        {
            _animator.SetFloat(_horizontalParamID, _moveVector.x);
            _animator.SetFloat(_verticalParamID, _moveVector.y);
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
        
        _animator.SetBool(_isMovingParamID, _isMoving);
    }
    
    void Interact()
    {
        var facingDir = new Vector3(_animator.GetFloat(_horizontalParamID),_animator.GetFloat(_verticalParamID));
        facing = facingDir;
        var interactPos = transform.position + facingDir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}