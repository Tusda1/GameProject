using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy CurrentEnemy { get; private set; }
    
    private EnemyScrub _enemyType;
    private NpcSpeaker _npcSpeaker;

    private void Start()
    {
        _npcSpeaker = GetComponent<NpcSpeaker>();
        _enemyType = _npcSpeaker.npc as EnemyScrub;
        _npcSpeaker.enabled = false;
        CombatEvents.combatEnd += CombatEnd;
        
        StartCombat();
    }

    private void StartCombat()
    {
        CurrentEnemy = this;
        EnemyDataHolder.SetEnemyData(_enemyType);
        GameManager.Instance.InitateCombat();
    }

    private void CombatEnd()
    {
        if (_npcSpeaker.enabled == false)
        {
            SelfDestruct();
        }
    }
    
    private void SelfDestruct()
    {
        Destroy(gameObject);
        CombatEvents.combatEnd -= CombatEnd;
    }
    
    public void TriggerDialogue()
    {
        _npcSpeaker.enabled = true;
        _npcSpeaker.userInput = true;
    }
}
