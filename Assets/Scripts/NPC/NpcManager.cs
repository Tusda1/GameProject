using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public NpcScrub npc;
    public TextMeshProUGUI text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
}