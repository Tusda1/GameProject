using UnityEngine;
using UnityEngine.UI;

public class CombatVisuals : MonoBehaviour
{
    
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider enemyHealthSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealthSlider.maxValue = PlayerDataHolder.playerData["MaxHealth"];
        enemyHealthSlider.maxValue = EnemyDataHolder.currentEnemyHealth;
        playerHealthSlider.value = PlayerDataHolder.playerData["Health"];
        enemyHealthSlider.value = EnemyDataHolder.currentEnemyHealth;
        
        CombatEvents.updateHealthUI += UpdateHealthBars;
    }

    private void UpdateHealthBars()
    {
        playerHealthSlider.value = PlayerDataHolder.playerData["Health"];
        enemyHealthSlider.value = EnemyDataHolder.currentEnemyHealth;
    }
}
