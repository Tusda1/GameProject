using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private bool _playerTurn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerAttackMoves.PutAttackMovesInDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
