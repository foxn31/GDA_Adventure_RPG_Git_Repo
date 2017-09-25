using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour {

	Transform player;

	public Dialogue dialogue;
	public float interactionRadius = 2;

	int sentenceIncrement = 0;
	float timeAtLastDiologue;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
		
	void Update () {
		float distance = Vector3.Distance (player.position, transform.position);

		if (distance <= interactionRadius && Input.GetKeyDown(KeyCode.F) && sentenceIncrement == 0) {
			this.TriggerDialogue ();
			timeAtLastDiologue = Time.time;
		}
		if (Input.GetKeyDown(KeyCode.F) && Time.time >= (timeAtLastDiologue + 1)) {
			this.TriggerNextDialogue ();
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
		FindObjectOfType <DialogueManager> ().DisplayNextSentence ();
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
