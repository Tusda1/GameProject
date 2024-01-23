using UnityEngine;

public class Player : MonoBehaviour
{
    
    [Header("Components")]
    private Rigidbody2D _rb;
    private Animator _animator;
    
    private bool _interactable;
    public static bool frozen = false;
    private float _moveSpeed = 10;
    private Vector2 _moveVector = Vector2.zero;
    private bool _isMoving;

    private Vector2 _inputAxis;
    private Vector3 _targetPos;

    public Vector2 facing;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input_Controller.Instance.interactPressed) Interact();
    }
    
    private void FixedUpdate()
    {
        if (frozen)
            _moveVector = Vector2.zero;
        else
        {
            _moveVector = Input_Controller.Instance.moveDirection;
            _rb.velocity = _moveVector * _moveSpeed;
        }

        if (_moveVector != Vector2.zero)
        {
            _animator.SetFloat("Horizontal", _moveVector.x);
            _animator.SetFloat("Vertical", _moveVector.y);;
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
        
        _animator.SetBool("IsMoving", _isMoving);
    }
    
    void Interact()
    {
        var faceingDir = new Vector3(_animator.GetFloat("Horizontal"),_animator.GetFloat("Vertical"));
        facing = faceingDir;
        var interactPos = transform.position + faceingDir;
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}