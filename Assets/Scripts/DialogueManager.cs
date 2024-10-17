using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public Image speakerImage;
    [SerializeField] public GameObject dialogueBox;

    private DialogueData currentDialogue;
    private int index;
    private Action onDialogueFinished;
    private Coroutine typingCoroutine;
    private bool isLineFullyDisplayed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIElementWithTag("DialogueBox"))
        {   
            if (isLineFullyDisplayed)
            {
                Debug.Log("line fully shown already, showing next line");
                index++;
                NextLine();
            }
            else
            {
                Debug.Log("show current full line");
                StopCoroutine(typingCoroutine);
                string line = currentDialogue.lines[index];
                bool isItalic = IsLineItalic(ref line);
                textComponent.text = line; // shows full line
                textComponent.fontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal;
                isLineFullyDisplayed = true;
            }
        }
    }

    public void StartDialogue(DialogueData dialogueData, Action onDialogueFinished)
    {
        ShowDialogueBox();
        currentDialogue = dialogueData;
        textComponent.text = string.Empty;
        index = 0;

        this.onDialogueFinished = onDialogueFinished;
        speakerImage.sprite = currentDialogue.speakerImage;

        NextLine();
    }

    private IEnumerator TypeLine()
    {
        isLineFullyDisplayed = false;
        string line = currentDialogue.lines[index];
        bool isItalic = IsLineItalic(ref line); // check if the line should be italic

        textComponent.text = string.Empty; // clear the text component before typing
        textComponent.fontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal; // apply italic style to the whole line

        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isLineFullyDisplayed = true;
        Debug.Log("incrementing index");
        index++;
    }


    private void NextLine()
    {
        if (index < currentDialogue.lines.Length)
        {   
            textComponent.text = string.Empty;
            speakerImage.sprite = currentDialogue.speakerImage; // update image
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("Dialogue lines finished");
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
        Debug.Log("Hiding box");
        dialogueBox.SetActive(false);
    }

    public void ShowDialogueBox()
    {
        Debug.Log("Showing box");
        dialogueBox.SetActive(true);
    }

    private bool IsPointerOverUIElementWithTag(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }
}
