using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    
    [Header("Property ID's")]
    [HideInInspector] public int horizontalParamID;
    [HideInInspector] public int verticalParamID;
    [HideInInspector] public int isMovingParamID; 
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        // Cache parameter IDs
        horizontalParamID = Animator.StringToHash("Horizontal");
        verticalParamID = Animator.StringToHash("Vertical");
        isMovingParamID = Animator.StringToHash("IsMoving");
    }

    public void UpdateAnimation(Vector2 moveVector)
    {
        if (moveVector != Vector2.zero)
        {
            _animator.SetFloat(horizontalParamID, moveVector.x);
            _animator.SetFloat(verticalParamID, moveVector.y);
            _animator.SetBool(isMovingParamID, true);
        }
        else
        {
            _animator.SetBool(isMovingParamID, false);
        }
    }
    
    public Vector3 GetFacingDirection()
    {
        return new Vector3(_animator.GetFloat(horizontalParamID), _animator.GetFloat(verticalParamID));
    }
}
