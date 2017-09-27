using UnityEngine;

public class DialogueNPC : Interactable {

	public static event System.Action ShowTalkPrompt;
	public static event System.Action HideTalkPrompt;

	bool promptVisible = true;

	public Dialogue dialogue;

	int sentenceIncrement = 0;
	float timeAtLastDiologue;

	void Start () {
		
	}
		
	public override void Interact() {
		
		base.Interact ();

		if(sentenceIncrement == 0) {
			TriggerDialogue ();
			timeAtLastDiologue = Time.time;
			hidePrompt();
		}
		if (Time.time >= (timeAtLastDiologue + .1)) {
			TriggerNextDialogue ();
			timeAtLastDiologue = Time.time;
		}

	}

	public override void Update () {
		base.Update ();
	}

	void LateUpdate() {
		if (playerIsInRange() && sentenceIncrement == 0) {
			showPrompt ();
		} else {
			hidePrompt ();
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
		
	void showPrompt() {
		if (!promptVisible && ShowTalkPrompt != null) {
			ShowTalkPrompt ();
			promptVisible = true;
		}
	}

	void hidePrompt() {
		if (promptVisible && HideTalkPrompt != null) {
			HideTalkPrompt ();
			promptVisible = false;
		}
	}

}
