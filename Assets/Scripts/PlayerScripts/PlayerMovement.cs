using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Input_Controller _input;
    
    private Rigidbody2D _rb = null;
    

    private void Awake()
    {
        _input = GetComponent<Input_Controller>();
        _rb = GetComponent<Rigidbody2D>();
    }

   
}
