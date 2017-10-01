﻿using UnityEngine;

public class Interactable : MonoBehaviour {

	public Transform player;
	public Transform interactTransform;

	public float interactRadius = 3f;

	bool hasInteracted;
	float distance;

	public virtual void Interact() {
		Debug.Log ("Interacting");
	}

	//public virtual void showPrompt() {
	//	Debug.Log ("Showing prompt");
	//}

	//public virtual void hidePrompt() {
	//	Debug.Log ("Hiding prompt");
	//}
	
	public virtual void Update () {
		distance = Vector3.Distance (player.position, transform.position);

		if (playerIsInRange() && Input.GetKeyDown(KeyCode.F)) {
			Interact ();
		}

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere (transform.position, interactRadius);
		if (interactTransform == null) {
			interactTransform = transform;
		}
	}

	public float getDistance () {
		return distance;
	}

	public bool playerIsInRange() {
		if (distance <= interactRadius) {
			return true;
		}
		else {
			return false;
		}
	}
}