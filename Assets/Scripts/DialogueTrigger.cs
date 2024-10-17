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
            Debug.Log("DialogueManager not found in the scene.");
        }
        else
        {
            Debug.Log("DialogueManager found: " + dialogueManager.name);
        }

        if (dialogueData == null)
        {
            Debug.Log("DialogueData is not assigned.");
        }
        else
        {
            Debug.Log("DialogueData assigned: " + dialogueData.name);
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
            Debug.Log("DialogueManager or DialogueData is null.");
        }
    }
}
