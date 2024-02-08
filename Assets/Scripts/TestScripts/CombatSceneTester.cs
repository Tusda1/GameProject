using UnityEngine;

public class CombatSceneTester : MonoBehaviour
{
    public EnemyScrub enemy;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyDataHolder.SetEnemyData(enemy);
            StartCombat();
        }
    }

    private void StartCombat()
    {
        GameManager.Instance.InitiateCombat();
    }
}
