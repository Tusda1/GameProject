using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AllAttackMoves", menuName = "Player/AllAttackMoves")]
public class AllAttackMoves : ScriptableObject
{
    public PlayerSkills[] allPlayerSkills;
    public EquipmentSkill[] allWeaponSkills;
}
