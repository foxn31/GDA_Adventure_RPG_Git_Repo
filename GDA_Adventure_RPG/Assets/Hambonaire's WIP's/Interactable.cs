using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public Transform player;

	public Transform interactTransform;

	public float interactRadius = 3f;

	float distance;

	bool hasInteracted;
	bool isInteracting;

	void Start () {
		hasInteracted = false;
		isInteracting = false;
	}

	public virtual void Interact() {

	}
	
	void Update () {
		distance = Vector3.Distance (player.position, transform.position);
		Debug.Log (distance);

		if (playerIsInRange() && Input.GetKeyDown(KeyCode.F)) {
			isInteracting = true;
			Interact ();
			isInteracting = false;
			hasInteracted = true;
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
