using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D _rb = null;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
}
