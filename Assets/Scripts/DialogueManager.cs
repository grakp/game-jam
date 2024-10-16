using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public Image speakerImage;
    [SerializeField] public GameObject dialogueBox;

    private DialogueData currentDialogue;
    private int index;
    private Action onDialogueFinished;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) // left click
        {
            if (textComponent.text == currentDialogue.lines[index]) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                string line = currentDialogue.lines[index];
                bool isItalic = IsLineItalic(ref line);
                textComponent.text = line; // shows full line
                if (isItalic)
                {
                    textComponent.fontStyle = FontStyles.Italic;
                }
                else
                {
                    textComponent.fontStyle = FontStyles.Normal;
                }
            }
        }
    }

    public void StartDialogue(DialogueData dialogueData, Action onDialogueFinished)
    {
        if (dialogueData == null)
        {
            Debug.LogError("DialogueData is null.");
            return;
        }

        if (dialogueData.lines == null || dialogueData.lines.Length == 0)
        {
            Debug.LogError("DialogueData lines are empty.");
            return;
        }

        if (textComponent == null)
        {
            Debug.LogError("Text Component is not assigned.");
            return;
        }

        if (speakerImage == null)
        {
            Debug.LogError("Speaker Image is not assigned.");
            return;
        }

        if (dialogueBox == null)
        {
            Debug.LogError("Dialogue Box is not assigned.");
            return;
        }

        currentDialogue = dialogueData;
        index = 0;
        this.onDialogueFinished = onDialogueFinished;
        speakerImage.sprite = currentDialogue.speakerImage;
        dialogueBox.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string line = currentDialogue.lines[index];
        bool isItalic = IsLineItalic(ref line);

        textComponent.text = string.Empty;

        foreach (char c in line.ToCharArray())
        {
            textComponent.text += isItalic ? $"<i>{c}</i>" : c.ToString();
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < currentDialogue.lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            speakerImage.sprite = currentDialogue.speakerImage; // update image
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("Dialogue Finished");
            onDialogueFinished?.Invoke(); // notify finished
        }
    }

    private bool IsLineItalic(ref string line)
    {
        if (line.StartsWith("*") && line.EndsWith("*"))
        {
            line = line.Substring(1, line.Length - 2); // remove the markers
            return true;
        }
        return false;
    }

    public void HideDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
}
