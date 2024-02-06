using UnityEngine;


[CreateAssetMenu(fileName = "Enemy", menuName = "Combat/Enemy")]
public class EnemyScrub : CharacterBase
{
    public RuntimeAnimatorController animatorController;
    public int maxHealth;
    public bool isBoss;
    public float chanceToRun;
    public float chanceToTalk;
    public Vector2 xpGiven;
    public Vector2 goldGiven;
    public int level;
    
    public AttackMove[] attackMoves;

    public AttackMove EnemyAttack()
    {
        AttackMove chosenAttackMove = attackMoves[UnityEngine.Random.Range(0, attackMoves.Length)];
        return chosenAttackMove;
    }
}
