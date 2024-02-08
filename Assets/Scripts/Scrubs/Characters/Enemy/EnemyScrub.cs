using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public ItemScrub item;
    public float dropChance;
}

[System.Serializable]
public class EquipmentDrop
{
    public EquipmentScrub equipment;
    public float dropChance;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Combat/Enemy")]
public class EnemyScrub : CharacterBase
{
    [Header("Variables")]
    public RuntimeAnimatorController animatorController;
    public bool isBoss;
    public float chanceToRun;
    public float chanceToTalk;
    
    [Header("Stats")]
    public int level;
    public int maxHealth;
    public StatModifier[] enemyStats;
    
    [Header("Rewards")]
    public Vector2 xpGiven;
    public Vector2 goldGiven;
    
    [Header("Attack Moves")]
    public AttackMove[] attackMoves;
    
    [Header("Drops")]
    public ItemDrop[] itemDrops;
    public EquipmentDrop[] equipmentDrops;
    
    
    public AttackMove EnemyAttack()
    {
        AttackMove chosenAttackMove = attackMoves[Random.Range(0, attackMoves.Length)];
        return chosenAttackMove;
    }
}
