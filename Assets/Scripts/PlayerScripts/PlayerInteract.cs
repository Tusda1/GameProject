using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    [Header("Components")]
    private Rigidbody2D _rigidbody2D;
    private Input_Controller _input;
    private Animator _animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interacebleLayer;
    
    private bool _interactable;
    [HideInInspector] public static bool frozen = false;

    private Vector2 _inputAxis;
    private Vector3 targetPos;

    public Vector2 facing;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = GetComponent<Input_Controller>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_input.interactPressed) Interact();
    }
    
    void Interact()
    {
        var faceingDir = new Vector3(_animator.GetFloat("horizontal"),_animator.GetFloat("vertikcal"));
        facing = faceingDir;
        var interactPos = transform.position + faceingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interacebleLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
}