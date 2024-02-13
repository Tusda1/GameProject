using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatVisuals : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Slider enemyHealthSlider;
    [SerializeField] private TextMeshProUGUI enemyHealthText;
    [SerializeField] private TextMeshProUGUI enemyNameAndLevelText;
    
    [Header("Player")]
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private TextMeshProUGUI playerHealthText;
    [SerializeField] private TextMeshProUGUI playerManaText;
    
    private int _enemyMaxHealth;
    
    private int _playerMaxHealth;
    private int _playerMaxMana;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHealthSlider.maxValue = PlayerDataHolder.playerData["MaxHealth"];
        enemyHealthSlider.maxValue = EnemyDataHolder.currentEnemyHealth;
        UpdateBars();
        
        
        
        CombatEvents.updateHealthUI += UpdateBars;
    }

    private void UpdateBars()
    {
        playerHealthSlider.value = PlayerDataHolder.playerData["Health"];
        enemyHealthSlider.value = EnemyDataHolder.currentEnemyHealth;
        playerHealthText.text = $"{PlayerDataHolder.playerData["Health"]}/{PlayerDataHolder.playerData["MaxHealth"]}";
        playerManaText.text = $"{PlayerDataHolder.playerData["Mana"]}/{PlayerDataHolder.playerData["MaxMana"]}";
        enemyHealthText.text = $"{EnemyDataHolder.currentEnemyHealth}/{EnemyDataHolder.enemyData.maxHealth}";
        enemyNameAndLevelText.text = $"{EnemyDataHolder.enemyData.characterName} - Level {EnemyDataHolder.enemyData.level}";
    }
}
