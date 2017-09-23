using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerCamera : MonoBehaviour {

	public Transform target;
	public float distFromTarget = 5;
	public float mouseSensitivity = 8;
	public float rotationSmoothTime = .1f;
	public Vector2 pitchMinMax = new Vector2 (-50, 80);

	public bool lockedCursor;

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	float pitch;
	float yaw;

	// Use this for initialization
	void Start () {
		if (lockedCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

		Vector3 targetRotation = new Vector3 (pitch, yaw);
		transform.eulerAngles = currentRotation;

		transform.position = target.position - transform.forward * distFromTarget;
	}
}
