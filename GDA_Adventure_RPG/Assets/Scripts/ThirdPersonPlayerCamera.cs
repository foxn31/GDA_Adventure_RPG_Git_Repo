using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerCamera : MonoBehaviour {

	public Transform target; 

	public float mouseSensitivity = 8;

	float rotationSmoothTime = .1f;
	float zoomSmoothTime = .2f;
	float zoomSpeed = 5;
	float camZoom;
	float camVerticalOffset = 2;
	float pitch;
	float yaw;

	float zoomVelocity;
	float currentZoom;

	bool camRotEnabled;

	Vector2 zoomMinMax = new Vector2 (2, 5);
	Vector2 pitchMinMax = new Vector2 (-50, 80);

	Vector3 rotationSmoothVelocity;
	Vector3 currentRotation;

	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		camZoom = zoomMinMax.y;
		camRotEnabled = true;
	}

	void Update () {
		camZoom -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
		camZoom = Mathf.Clamp (camZoom, zoomMinMax.x, zoomMinMax.y);
	}

	void LateUpdate () {
		//if (camEnable) {
			if (camRotEnabled) {
				yaw += Input.GetAxis ("Mouse X") * mouseSensitivity;
				pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
				pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);

				currentRotation = Vector3.SmoothDamp (currentRotation, new Vector3 (pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
			}
				camZoom -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
				camZoom = Mathf.Clamp (camZoom, zoomMinMax.x, zoomMinMax.y);
				currentZoom = Mathf.SmoothDamp (currentZoom, camZoom, ref zoomVelocity, zoomSmoothTime);

				transform.eulerAngles = currentRotation;

				//smoother zoom but not full up-close
				transform.position = target.position - transform.forward * camZoom + transform.up * camVerticalOffset * Mathf.Lerp (0, 1, (currentZoom / zoomMinMax.y));

				//less smooth but full up-close
				//transform.position = target.position - transform.forward * camZoom + transform.up * camVerticalOffset * Mathf.Lerp (0, 1, (currentZoom - camVerticalOffset));
		//}
	}
		
	public void EnableCamRot () {
		camRotEnabled = true;
	}

	public void DisableCamRot () {
		camRotEnabled = false;
	}
}