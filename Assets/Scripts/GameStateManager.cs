using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

    private Vector3 playerPosition;
    private string previousSceneName;
    private bool returningFromBattle = false;
    private bool battleWon = false;
    private int victoryDialogueIndex;
    private bool showVictoryDialogue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(GameObject player)
    {
        playerPosition = player.transform.position;
        previousSceneName = SceneManager.GetActiveScene().name;
    }

    public void RestorePlayerState(GameObject player)
    {
        player.transform.position = playerPosition;
    }

    public string GetPreviousSceneName()
    {
        return previousSceneName;
    }
    public void SetBattleWon(bool won)
    {
        battleWon = won;
    }

    public bool IsBattleWon()
    {
        return battleWon;
    }

    public void SetVictoryDialogueIndex(int index)
    {
        victoryDialogueIndex = index;
    }

    public int GetVictoryDialogueIndex()
    {
        return victoryDialogueIndex;
    }

    public bool ShouldShowVictoryDialogue()
    {
        return showVictoryDialogue;
    }

    public void SetShowVictoryDialogue(bool value)
    {
        showVictoryDialogue = value;
    }
    
    public void ResetShowVictoryDialogue()
    {
        showVictoryDialogue = false;
    }

    public void SetReturningFromBattle(bool value)
    {
        returningFromBattle = value;
    }

    public bool IsReturningFromBattle()
    {
        return returningFromBattle;
    }
}
