using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy CurrentEnemy { get; private set; }
    
    [SerializeField] private EnemyScrubBase enemyType;
    private NpcSpeaker _npcSpeaker;

    private void Awake()
    {
        _npcSpeaker = GetComponent<NpcSpeaker>();
        //Set the script to inactive
        _npcSpeaker.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            if (other.CompareTag("Player"))
            {
                EnemyDataHolder.SetEnemyData(enemyType);
                StartCombat();
            }
    }

    private void StartCombat()
    {
        CurrentEnemy = this;
        GameManager.Instance.InitateCombat();
    }
    
    public void TriggerDialogue()
    {
        _npcSpeaker.enabled = true;
        _npcSpeaker.userInput = true;
    }
}
