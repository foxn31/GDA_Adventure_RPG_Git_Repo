using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerCamera : MonoBehaviour {

	public Transform target;

	public float mouseSensitivity = 8;
	public float rotationSmoothTime = .1f;
	public float zoomSpeed = 5;
	public float camZoom;

	float pitch;
	float yaw;

	public bool lockedCursor;

	Vector2 zoomMinMax = new Vector2 (2, 8);
	Vector2 pitchMinMax = new Vector2 (-50, 80);

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;


	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		lockedCursor = true;
		camZoom = zoomMinMax.y;
	}

	void Update () {
		camZoom -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
		camZoom = Mathf.Clamp (camZoom, zoomMinMax.x, zoomMinMax.y);
	
		if ( Input.GetKeyDown(KeyCode.Escape) ) {
			changeCursorVisibilty();
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

		currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

		camZoom -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
		camZoom = Mathf.Clamp (camZoom, zoomMinMax.x, zoomMinMax.y);

		transform.eulerAngles = currentRotation;
		transform.position = target.position - transform.forward * camZoom;
	}

	void changeCursorVisibilty () {
		if (!lockedCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			lockedCursor = true;
		}
		else if (lockedCursor) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			lockedCursor = false;
		}
	}
}
