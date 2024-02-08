using System;
using UnityEngine;

[Serializable]
public class AttackModifier
{
    public StatEnum statName;
    public Vector2 modifier;
}

[CreateAssetMenu(fileName = "AttackMove", menuName = "Combat/AttackMove")]
public class AttackMove : ScriptableObject
{
    public string attackMoveName;
    public AttackModifier attackModifier;
    public string narration;
    public StatusEffects statusEffect;
    public float chanceToApplyStatusEffect;
}
