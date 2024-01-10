using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneTester : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCombat();
        }
    }

    private void StartCombat()
    {
        GameManager.Instance.InitateCombat();
    }
}
