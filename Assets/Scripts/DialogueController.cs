using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private DialogueTrigger[] dialogueTriggers;
    private int currentDialogueIndex = 0;
    private Queue<int> dialogueQueue = new Queue<int>();
    private bool isPlayingDialogues = false;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
            return;
        }

        // play first two dialogues
        PlayDialogues(0, 2);
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
            dialogueTriggers[dialogueIndex].OnDialogueFinished += OnDialogueFinished;
            dialogueManager.ShowDialogueBox();
            dialogueTriggers[dialogueIndex].TriggerDialogue();
        }
        else
        {
            isPlayingDialogues = false;
            dialogueManager.HideDialogueBox();
            Debug.Log("No more dialogues to trigger.");
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
        // unsubscribe from the event to avoid retriggering
        foreach (var trigger in dialogueTriggers)
        {
            trigger.OnDialogueFinished -= OnDialogueFinished;
        }

        // wait for user input before triggering next dialogue
        StartCoroutine(WaitForNextDialogue());
    }

    private IEnumerator WaitForNextDialogue()
    {
        Debug.Log("Waiting for user input to trigger next dialogue.");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        // trigger the next dialogue in the queue
        Debug.Log("Triggering next dialogue.");
        TriggerNextDialogue();
    }
}
