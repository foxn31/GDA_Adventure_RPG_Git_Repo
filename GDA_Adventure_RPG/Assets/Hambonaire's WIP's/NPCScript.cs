using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour {

	Transform player;

	public static event System.Action ShowTalkPrompt;
	public static event System.Action HideTalkPrompt;

	public Dialogue dialogue;
	public float interactionRadius = 3;

	int sentenceIncrement = 0;
	float timeAtLastDiologue;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
		
	void Update () {
		float distance = Vector3.Distance (player.position, transform.position);

		if (distance <= interactionRadius && sentenceIncrement == 0) {
			ShowTalkPrompt ();
		} else {
			HideTalkPrompt ();
		}
		if (distance <= interactionRadius && Input.GetKeyDown(KeyCode.F) && sentenceIncrement == 0) {
			TriggerDialogue ();
			timeAtLastDiologue = Time.time;
			HideTalkPrompt ();
		}
		if (distance <= interactionRadius && Input.GetKeyDown(KeyCode.F) && Time.time >= (timeAtLastDiologue + .5)) {
			TriggerNextDialogue ();
			timeAtLastDiologue = Time.time;
		}
	}

	public void TriggerDialogue() {
		if (sentenceIncrement == 0) {
			FindObjectOfType <DialogueManager> ().StartDialogue (dialogue);
			sentenceIncrement++;
		}
	}

	public void TriggerNextDialogue() {
		FindObjectOfType <DialogueManager> ().ContinueDialogue ();
		if (sentenceIncrement == dialogue.getSize ()) {
			sentenceIncrement = 0;
		} else {
			sentenceIncrement++;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, interactionRadius);
	}
}
