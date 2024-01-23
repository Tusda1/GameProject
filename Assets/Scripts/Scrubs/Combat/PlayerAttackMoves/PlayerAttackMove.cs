using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackMove", menuName = "Player/PlayerAttackMove")]
public class PlayerAttackMove : ScriptableObject
{
    public string attackMoveName;
    public int damage;
    public string narration;
    public StatusEffects statusEffect;
}
