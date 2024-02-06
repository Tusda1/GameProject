using UnityEngine;

[CreateAssetMenu(fileName = "AttackMove", menuName = "Combat/AttackMove")]
public class AttackMove : ScriptableObject
{
    public string attackMoveName;
    public Vector2 damageRange;
    public string narration;
    public StatusEffects statusEffect;
    public float chanceToApplyStatusEffect;
}
