using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetScript : MonoBehaviour {

	public Transform playerTarget;

	float maxDistanceFromPlayer = 3f;
	float speed = 5;

	Vector3 height = new Vector3(0,2,0);


	void Update() {
		Vector3 displacementFromPlayer = playerTarget.position - transform.position + height;
		Vector3 directionToPlayer = displacementFromPlayer.normalized;
		Vector3 velocity = directionToPlayer * speed;

		float distanceToPlayer = displacementFromPlayer.magnitude;

		if (distanceToPlayer > maxDistanceFromPlayer) {
			transform.Translate (velocity * Time.deltaTime);
		}
	}
}