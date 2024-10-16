 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public Image speakerImage;
    public Speaker[] speakers;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // left click
        {
            if (textComponent.text == lines[index]) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                textComponent.text = lines[index]; // shows full line
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        SetSpeakerImage();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // type each char 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            SetSpeakerImage(); // update image
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetSpeakerImage()
    {
        if (index < speakers.Length)
        {
            speakerImage.sprite = speakers[index].image;
        }
        else
        {
            speakerImage.sprite = null; 
        }
    }
}
