using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;
    private DialogueManager dialogueManager;

    public event Action OnDialogueFinished;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }
    }

    public void TriggerDialogue()
    {
        if (dialogueManager != null && dialogueData != null)
        {
            dialogueManager.StartDialogue(dialogueData, OnDialogueFinished);
        }
        else
        {
            Debug.LogError("DialogueManager or DialogueData is null.");
        }
    }
}
