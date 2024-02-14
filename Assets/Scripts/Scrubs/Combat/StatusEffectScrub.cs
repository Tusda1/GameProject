using UnityEngine;
using UnityEngine.Serialization;

public enum StatusEffectsEnum
{
    Poison,
    Freeze,
    Paralyze,
    Sleep,
    Confusion,
    Bleed,
    Stun
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffectScrub statusEffectScrub;
    public int duration;
    public float chanceToApply;
}

[CreateAssetMenu(fileName = "StatusEffectScrub", menuName = "Combat/StatusEffect")]
public class StatusEffectScrub : ScriptableObject
{
    public StatusEffectsEnum statusEffectName; 
    public int damage;
    public int maxHealthPercentDamage;
    public int chanceToStun;
}