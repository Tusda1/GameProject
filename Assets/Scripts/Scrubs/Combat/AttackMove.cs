using System;
using UnityEngine;

[Serializable]
public class AttackModifier
{
    public StatEnum statName;
    public Vector2 modifier;
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffects statusEffect;
    public int duration;
    public float chanceToApply;
}

[CreateAssetMenu(fileName = "AttackMove", menuName = "Combat/AttackMove")]
public class AttackMove : ScriptableObject
{
    public string attackMoveName;
    public string narration;
    
    [Header("Attack stats")]
    public AttackModifier attackModifier;
    public int manaCost;
    public int critChance;
    public int critMultiplier;
    
    [Header("Accuracy")]
    public int accuracy;
    public bool accuracyScalesWithSpeed;
    public bool alwaysHits;
    
    [Header("Status Effect")]
    public StatusEffect[] statusEffects;
}
