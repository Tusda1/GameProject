using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "New NPC")]
public class NpcScrub : ScriptableObject
{
    public Sprite sprite;
    public Image portrait;
    
    public string npcName;
}
