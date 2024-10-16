using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField] private DialogueTrigger[] dialogueTriggers;
    private int currentDialogueIndex = 0;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
            return;
        }
        
        if (dialogueTriggers.Length > 0)
        {
            dialogueTriggers[0].OnDialogueFinished += TriggerNextDialogue;
            dialogueTriggers[0].TriggerDialogue();
        }
    }

    private void TriggerNextDialogue()
    {
        dialogueTriggers[currentDialogueIndex].OnDialogueFinished -= TriggerNextDialogue;
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueTriggers.Length && currentDialogueIndex < 2) // ensure only the first two dialogues play for now
        {
            dialogueTriggers[currentDialogueIndex].OnDialogueFinished += TriggerNextDialogue;
            dialogueTriggers[currentDialogueIndex].TriggerDialogue();
        }

        dialogueManager.HideDialogueBox();
    }
}
