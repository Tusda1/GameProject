using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    [Header("Components")]
    private Rigidbody2D _rb;
    private Input_Controller _input;
    private Animator _animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interacebleLayer;
    
    private bool _interactable;
    [HideInInspector] public static bool frozen = false;
    private float _moveSpeed = 10;
    private Vector2 _moveVector = Vector2.zero;

    private Vector2 _inputAxis;
    private Vector3 targetPos;

    public Vector2 facing;


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<Input_Controller>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_input.interactPressed) Interact();
    }
    
    private void FixedUpdate()
    {
        _moveVector = _input.moveDirection;
        _rb.velocity = _moveVector * _moveSpeed;

        if (_moveVector != Vector2.zero)
        {
            _animator.SetFloat("Horizontal", _moveVector.x);
            _animator.SetFloat("Vertical", _moveVector.y);;
        }
    }
    
    void Interact()
    {
        var faceingDir = new Vector3(_animator.GetFloat("Horizontal"),_animator.GetFloat("Vertical"));
        facing = faceingDir;
        var interactPos = transform.position + faceingDir;

        Debug.Log("Got to collider check");
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f);
        if (collider != null)
        {
            Debug.Log("got to interactable");
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}