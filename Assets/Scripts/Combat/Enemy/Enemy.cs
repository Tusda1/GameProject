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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("Player"))
            {
                StartCombat();
            }
    }

    private void StartCombat()
    {
        CurrentEnemy = this;
        EnemyDataHolder.SetEnemyData(_enemyType);
        GameManager.Instance.InitateCombat();
    }
    
    public void TriggerDialogue()
    {
        _npcSpeaker.enabled = true;
        _npcSpeaker.userInput = true;
    }
}
