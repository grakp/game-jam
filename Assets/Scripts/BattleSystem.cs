using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, WAIT }

public class BattleSystem : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    public Transform enemyBattleStation;
    public Transform playerBattleStation;
    Unit enemyUnit;
    Unit playerUnit;
    public TextMeshProUGUI dialogueText;
    public BattleHUD enemyHUD;
    public BattleHUD playerHUD;
    public BattleState state;

    [SerializeField] private Sprite hurtSprite;
    private float textSpeed = 0.05f;
    private int victoryDialogueIndex;
    

    void Start()
    {
        state = BattleState.START;
        GameStateManager.instance.SetReturningFromBattle(true);
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName;

        enemyHUD.SetHUD(enemyUnit);
        playerHUD.SetHUD(playerUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        string line = "Choose an action";
        StartCoroutine(TypeText(line));
    }

    private IEnumerator TypeText(string line)
    {
        BattleState prevState = state;
        state = BattleState.WAIT;
        dialogueText.text = ""; // clear the text before typing

        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        state = prevState;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerHeal());
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(RunAway());
    }

    private IEnumerator RunAway()
    {
        state = BattleState.ENEMYTURN;

        string line = "Weak.";
        StartCoroutine(TypeText(line));

        yield return new WaitForSeconds(3f);
        
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator PlayerHeal()
    {
        playerUnit.Heal(20);

        playerHUD.SetHP(playerUnit.currentHP);
        string line = "You feel renewed strength!";
        StartCoroutine(TypeText(line));

        state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(3f);

        StartCoroutine(EnemyTurn());
    }

    private IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        string line = "The attack was successful!";
        StartCoroutine(TypeText(line));

        // change enemy sprite to hurt for a few seconds
        SpriteRenderer enemySpriteRenderer = enemyUnit.GetComponent<SpriteRenderer>();
        Sprite originalSprite = enemySpriteRenderer.sprite;
        enemySpriteRenderer.sprite = hurtSprite;
        state = BattleState.WAIT;

        yield return new WaitForSeconds(3f);

        // revert to original sprite
        enemySpriteRenderer.sprite = originalSprite;

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        // change state based on outcome
    }

    private IEnumerator EnemyTurn()
    {
        string line = enemyUnit.unitName + " attacks!";
        StartCoroutine(TypeText(line));

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);
        state = BattleState.WAIT;

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    private IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            string line = "You won the battle!";
            StartCoroutine(TypeText(line));
            state = BattleState.WAIT;
            yield return new WaitForSeconds(1f);

            // hard coded which enemy was defeated and set the victory dialogue index accordingly lol
            if (enemyUnit != null)
            {
                if (enemyUnit.unitName == "Eyemask")
                {
                    Debug.Log("Eyemask defeated");
                    GameStateManager.instance.SetVictoryDialogueIndex(2);
                }
                else if (enemyUnit.unitName == "Photograph")
                {
                    Debug.Log("Photograph defeated");
                    GameStateManager.instance.SetVictoryDialogueIndex(3);
                }
                 else if (enemyUnit.unitName == "Boss")
                {
                    Debug.Log("Boss defeated");
                    SceneManager.LoadScene("Win");
                    yield break; // exit the coroutine
                }
            }

            GameStateManager.instance.SetBattleWon(true);
            GameStateManager.instance.SetShowVictoryDialogue(true);

            StartCoroutine(ReturnToPreviousScene());
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
            SceneManager.LoadScene("Lose");
        }
    }

    private IEnumerator ReturnToPreviousScene()
    {
        yield return new WaitForSeconds(2f); // wait for 2 seconds to show the victory message
        string previousSceneName = GameStateManager.instance.GetPreviousSceneName();

        if (previousSceneName == "BedRoom")
        {
            SceneManager.LoadScene("Win");
        }

        else
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(previousSceneName);
        }
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameStateManager.instance.GetPreviousSceneName())
        {
            GameObject player = GameObject.FindWithTag("Player");
            GameStateManager.instance.RestorePlayerState(player);
        }

        GameStateManager.instance.SetReturningFromBattle(false);

        // unsubscribe from the event to avoid multiple triggers
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetVictoryDialogueIndex(int index)
    {
        victoryDialogueIndex = index;
    }
}
