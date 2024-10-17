using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DialogueController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private DialogueTrigger[] dialogueTriggers;
    private int currentDialogueIndex = 0;
    private Queue<int> dialogueQueue = new Queue<int>();
    private bool isPlayingDialogues = false;
    private Action postDialogueAction;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.Log("DialogueManager not found in the scene.");
            return;
        }

        // only play dialogues if the current scene is the kitchen scene
        if (SceneManager.GetActiveScene().name == "Kitchen")
        {
            PlayDialogues(0, 2);
        }

        // check if a battle was won and trigger the appropriate dialogue
        if (GameStateManager.instance.ShouldShowVictoryDialogue())
        {
            Debug.Log("Battle won. Triggering victory dialogue: " + GameStateManager.instance.GetVictoryDialogueIndex());
            StartCoroutine(DelayedTriggerVictoryDialogue());
        }
    }

    private IEnumerator DelayedTriggerVictoryDialogue()
    {
        yield return new WaitForSeconds(0.5f); // delay to ensure everything is initialized

        int victoryDialogueIndex = GameStateManager.instance.GetVictoryDialogueIndex();

        if (victoryDialogueIndex != -1)  
        {
            TriggerDialogueForVictory(victoryDialogueIndex);
            GameStateManager.instance.ResetVictoryDialogueIndex();
        }

        // reset flags
        GameStateManager.instance.ResetShowVictoryDialogue();
        GameStateManager.instance.SetBattleWon(false);
    }

    public void PlayDialogues(int startIndex, int count)
    {
        for (int i = startIndex; i < startIndex + count; i++)
        {
            dialogueQueue.Enqueue(i);
            Debug.Log("Enqueued dialogue index: " + i);
        }

        currentDialogueIndex = startIndex + count;

        if (!isPlayingDialogues)
        {
            isPlayingDialogues = true;
            TriggerNextDialogue();
        }
    }

    public void TriggerNextDialogue()
    {       
        Debug.Log("Dialogue Queue Count: " + dialogueQueue.Count);
        if (dialogueQueue.Count > 0)
        {
            int dialogueIndex = dialogueQueue.Dequeue();
            Debug.Log("Triggering dialogue index: " + dialogueIndex);

            dialogueTriggers[dialogueIndex].OnDialogueFinished -= OnDialogueFinished;
            dialogueTriggers[dialogueIndex].OnDialogueFinished += OnDialogueFinished;

            dialogueManager.ShowDialogueBox();
            dialogueTriggers[dialogueIndex].TriggerDialogue();
        }
        else
        {
            isPlayingDialogues = false;
            dialogueManager.HideDialogueBox();
            Debug.Log("No more dialogues to trigger.");
            postDialogueAction?.Invoke();
        }
    
    }

    public void OnActionCompleted(int numDialogues)
    {
        Debug.Log("Action completed. Continuing dialogues.");
        PlayDialogues(currentDialogueIndex, numDialogues);
    }

    private void OnDialogueFinished()
    {
        Debug.Log("Dialogue finished.");
        // unsubscribe from the specific dialogue trigger that finished the dialogue
        if (dialogueTriggers[currentDialogueIndex] != null)
        {
            dialogueTriggers[currentDialogueIndex].OnDialogueFinished -= OnDialogueFinished;
        }

        isPlayingDialogues = false;

        if (dialogueQueue.Count > 0)
        {
            // wait for user input before triggering next dialogue
            StartCoroutine(WaitForNextDialogue());
        }
        else
        {
            // no more dialogues, invoke the post-dialogue action
            dialogueManager.HideDialogueBox();
            postDialogueAction?.Invoke();
        }
    }

    private IEnumerator WaitForNextDialogue()
    {
        Debug.Log("Waiting for user input to trigger next dialogue.");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        // trigger the next dialogue in the queue
        Debug.Log("Triggering next dialogue.");
        TriggerNextDialogue();
    }

    public void TriggerDialogueForEnemy(int enemyIndex, Action postDialogueCallback)
    {
        if (isPlayingDialogues) return;
        isPlayingDialogues = true;

        Debug.Log("Triggering dialogue for enemy: " + enemyIndex);
        postDialogueAction = postDialogueCallback;

        // assuming the dialogue index matches the enemy index
        if (enemyIndex >= 0 && enemyIndex < dialogueTriggers.Length)
        {
            dialogueTriggers[enemyIndex].OnDialogueFinished -= OnDialogueFinished;
            dialogueTriggers[enemyIndex].OnDialogueFinished += OnDialogueFinished;

            dialogueTriggers[enemyIndex].TriggerDialogue();
        }
        else
        {
            Debug.LogError("Invalid enemy index or dialogue index out of range.");
        }
    }

    public void TriggerDialogueForVictory(int victoryDialogueIndex)
    {
        dialogueManager.ShowDialogueBox();

        if (victoryDialogueIndex >= 0 && victoryDialogueIndex < dialogueTriggers.Length)
        {
            dialogueTriggers[victoryDialogueIndex].OnDialogueFinished += OnDialogueFinished;
            dialogueTriggers[victoryDialogueIndex].TriggerDialogue();
        }
        else
        {
            Debug.LogError("Invalid victory dialogue index or dialogue index out of range.");
        }
    }
}
