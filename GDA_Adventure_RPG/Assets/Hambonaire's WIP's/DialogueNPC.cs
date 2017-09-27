using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : Interactable {

	public static event System.Action ShowTalkPrompt;
	public static event System.Action HideTalkPrompt;

	public Dialogue dialogue;

	int sentenceIncrement = 0;
	float timeAtLastDiologue;

	void Start () {
		
	}
		
	//*************************************
	public override void Interact() {
		
		base.Interact ();

		if(sentenceIncrement == 0) {
			TriggerDialogue ();
			timeAtLastDiologue = Time.time;
			HideTalkPrompt ();
		}
		if (Time.time >= (timeAtLastDiologue + .5)) {
			TriggerNextDialogue ();
			timeAtLastDiologue = Time.time;
		}

	}
	//*************************************	

	void Update () {
		if (playerIsInRange() == true && sentenceIncrement == 0) {
			ShowTalkPrompt ();
		} else {
			HideTalkPrompt ();
		}
	}

	public void TriggerDialogue() {
		if (sentenceIncrement == 0) {
			FindObjectOfType <DialogueManager> ().StartDialogue (dialogue);
			sentenceIncrement++;
		}
	}

	public void TriggerNextDialogue() {
		FindObjectOfType <DialogueManager> ().DisplayNextSentence ();
		if (sentenceIncrement == dialogue.getSize ()) {
			sentenceIncrement = 0;
		} else {
			sentenceIncrement++;
		}
	}
		
}
