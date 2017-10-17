using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	public GameObject dialogueUI;

	private Queue<string> sentences;

	public static event System.Action EndAllDialogue;

	void Start () {
		sentences = new Queue<string> ();
		dialogueUI.SetActive (false);
	}

	public void StartDialogue(Dialogue dialogue)
	{
		FindObjectOfType<PlayerController> ().DisableMove ();

		dialogueUI.SetActive (true);

		nameText.text = dialogue.name;

		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
		}

		DisplayNextSentence ();
	}

	public void DisplayNextSentence() {
		if (sentences.Count == 0) {
			EndDialogue ();
			return;
		}
		string sentence = sentences.Dequeue ();
		StopAllCoroutines();
		StartCoroutine (TypeSentence (sentence));
	}

	IEnumerator TypeSentence (string sentence) {
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue() {
		dialogueUI.SetActive (false);
		FindObjectOfType<PlayerController> ().EnableMove ();
		if (EndAllDialogue != null) {
			EndAllDialogue ();
		}
	}


}
