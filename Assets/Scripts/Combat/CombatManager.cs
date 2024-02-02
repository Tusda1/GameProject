using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    [Header("Player")]
    private bool _playerInput;
    private bool _waitingForUserInput;
    
    [FormerlySerializedAs("allAttackMoves")] [SerializeField] private AllPlayerAttackMoves allPlayerAttackMoves;
    
    [Header("Text Box")]
    [SerializeField] private TextMeshProUGUI combatTextBox;
    
    [Header("Sprites")]
    [SerializeField] private Animator enemyAnimator;

    [Header("Semi temporary frequently changed variables")]
    private ActionType _actionType;
    private string _actionName;
    
    // Start is called before the first frame update
    void Start()
    {
        //Make sure text box is empty on start
        combatTextBox.text = "";
        
        enemyAnimator.runtimeAnimatorController = EnemyDataHolder.enemyData.animatorController;

        //Subscribe to the playerTurnOver event
        CombatEvents.playerTurnEnd += PlayerTurnEnd;
        GameEvents.interactPressed += UserInput;
    }

    private void OnDestroy()
    {
        CombatEvents.playerTurnEnd -= PlayerTurnEnd;
        GameEvents.interactPressed -= UserInput;
    }

    private void UserInput()
    {
        if (_waitingForUserInput)
            _playerInput = true;
    }
    
    private void PlayerTurnEnd(ActionType actionType, string actionName)
    {
        _actionType = actionType;
        _actionName = actionName;
        
        StartCoroutine(RunTurns());
    }

    #region Turns Running
    private IEnumerator RunTurns()
    {
        yield return StartCoroutine(PlayerTurn());


        StartCoroutine(EnemyDataHolder.currentEnemyHealth > 0 ? EnemyTurn() : OnWin());
    }
    
    private IEnumerator PlayerTurn()
    {
        if (_actionType == ActionType.Attack)
        {
            AttackMove attackMove = ScriptableObject.CreateInstance<AttackMove>();
            
            foreach (var attack in allPlayerAttackMoves.allAttackMoves)
            {
                if (attack.attackMoveName == _actionName)
                {
                    attackMove = attack;
                }
            }
            
            //Applies attack to enemy
            int damage = Random.Range((int)attackMove.damageRange.x, (int)attackMove.damageRange.y + 1);
            EnemyDataHolder.TakeDamage(damage);

            if (attackMove.statusEffect != StatusEffects.None)
            {
                Debug.Log("status effect");
            }
            
            //Writes the attack narration to the combat text box
            string attackNarration = attackMove.narration + " " + EnemyDataHolder.enemyData.characterName + " and dealt " + damage + " damage!";
            yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));
            combatTextBox.text = "";
        }
        
        else if (_actionType == ActionType.Item)
        {
            
        }
        
        else if (_actionType == ActionType.Run)
        {
            int random = Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToRun)
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("You ran away!"));
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("Couldn't run away..."));
            }
        }
        
        else if (_actionType == ActionType.Talk)
        {
            int random = Random.Range(0, 100);
            if (random < EnemyDataHolder.enemyData.chanceToTalk)
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("You're charisma convinced them!"));
                Enemy.CurrentEnemy.TriggerDialogue();
                GameManager.Instance.EndCombat();
            }
            else
            {
                yield return StartCoroutine(WriteTextAndWaitForUserInput("They're not convinced"));
            }
        }
    }
    
    private IEnumerator EnemyTurn()
    {   
        //Gets necessary information about enemy attack move
        AttackMove enemyMoveData = EnemyDataHolder.enemyData.EnemyAttack();
        int damage = Random.Range((int)enemyMoveData.damageRange.x, (int)enemyMoveData.damageRange.y + 1);
        
        //Applies attack to player
        PlayerDataHolder.TakeDamage(damage);
        
        //Writes the attack narration to the combat text box
        string attackNarration = EnemyDataHolder.enemyData.characterName + " " + enemyMoveData.narration + " and dealt " + damage + " damage!";
        yield return StartCoroutine(WriteTextAndWaitForUserInput(attackNarration));
        combatTextBox.text = "";

        //If player is alive go back to main menu, if player is dead be defeated
        if (PlayerDataHolder.playerData["Health"] <= 0)
        {
            yield return StartCoroutine(WriteTextAndWaitForUserInput("You died..."));
            GameManager.Instance.EndCombat();
        }
        else
        {
            ButtonScriptCombatMenu.BackToMain();
        }
    }
    #endregion
    
    private IEnumerator OnWin()
    {
        PlayerDataHolder.GetGold(EnemyDataHolder.goldGiven);
        PlayerDataHolder.GetExp(EnemyDataHolder.expGiven);
        
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You Won!"));
        
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You earned " + EnemyDataHolder.goldGiven +  " gold!"));
        
        yield return StartCoroutine(WriteTextAndWaitForUserInput("You earned " + EnemyDataHolder.expGiven + " xp!"));
        
        //Gold and exp given here
        GameManager.Instance.EndCombat();
    }
    
    private IEnumerator WriteTextAndWaitForUserInput(string text)
    {
        yield return StartCoroutine(GeneralWritingScript.WriteText(combatTextBox, text));
        yield return StartCoroutine(WaitForUserInput());
    }
    
    private IEnumerator WaitForUserInput()
    {
        _waitingForUserInput = true;
        while (!_playerInput)
        {
            yield return null;
        }
        _waitingForUserInput = false;
        _playerInput = false;
    }
}
