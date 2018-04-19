using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Transform playerCam, character, centerPoint;

	private float mouseX, mouseY;
	public float mouseSensitivity = 10f;

	private float moveForward, moveSide;
	float moveSpeed = 2f;

	private float zoom;
	public float zoomSpeed = 2f;
	float zoomMin = -2f;
	float zoomMax = -10f;

	// Use this for initialization
	void Start () {

		zoom = -3f;

	}
	
	// Update is called once per frame
	void Update () {

		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

		if (zoom > zoomMin) {
			zoom = zoomMin;
		}
		if (zoom < zoomMax) {
			zoom = zoomMax;
		}

		playerCam.transform.position = new Vector3 (0, 0, zoom);



	}
}
