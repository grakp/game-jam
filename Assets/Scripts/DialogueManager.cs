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
    private bool dialogueFinished = false;

    private void Update()
    {
        if (dialogueFinished && Input.GetMouseButtonDown(0))
        {
            HideDialogueBox();
        }
        else if (Input.GetMouseButtonDown(0)) // left click
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
        currentDialogue = dialogueData;
        index = 0;
        this.onDialogueFinished = onDialogueFinished;
        speakerImage.sprite = currentDialogue.speakerImage;
        dialogueBox.SetActive(true);
        dialogueFinished = false; // reset flag
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
            dialogueFinished = true;
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

    public void ShowDialogueBox()
    {
        dialogueBox.SetActive(true);
    }
}
