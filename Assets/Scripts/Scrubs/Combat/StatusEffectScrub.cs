using UnityEngine;

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
    public string statusEffectName; 
    public int damage;
    public int maxHealthPercentDamage;
    public int chanceToStun;
}