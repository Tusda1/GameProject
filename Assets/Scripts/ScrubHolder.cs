using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrubHolder : MonoBehaviour
{
    public static ScrubHolder Instance { get; private set; }
    
    public AllEquipment allEquipment;
    public AllAttackMoves allAttackMoves;
    
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
    }
}
