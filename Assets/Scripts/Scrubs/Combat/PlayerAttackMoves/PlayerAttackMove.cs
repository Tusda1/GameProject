using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackMove", menuName = "Player/PlayerAttackMove")]
public class PlayerAttackMove : ScriptableObject
{
    public string attackMoveName;
    public Vector2 damageRange;
    public string narration;
    public StatusEffects statusEffect;
}
