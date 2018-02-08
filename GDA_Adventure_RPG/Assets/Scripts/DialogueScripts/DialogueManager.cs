using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueUI;

    private string[] sentences;
    public int SentenceIndex { get; private set; }
    public bool Active { get; private set; }

    public static event System.Action EndAllDialogue;

    public static DialogueManager instance;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        dialogueUI.SetActive(false);
        Active = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        FindObjectOfType<PlayerController>().DisableMove();

        dialogueUI.SetActive(true);
        Active = true;

        nameText.text = dialogue.name;

        sentences = dialogue.sentences;

        SentenceIndex = 0;
        DisplaySentence();
    }

    public void ContinueDialogue()
    {
        SentenceIndex++;

        if (SentenceIndex >= sentences.Length)
        {
            EndDialogue();
            return;
        }

        DisplaySentence();
    }

    private void DisplaySentence()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences[SentenceIndex]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        Active = false;

        FindObjectOfType<PlayerController>().EnableMove();
        if (EndAllDialogue != null)
        {
            EndAllDialogue();
        }
    }


}
