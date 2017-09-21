using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float moveSpeed = 8;
	public float gravity = -12;

	float angle;
	float currentSpeed;
	float smoothedTurnVelocity;
	float smoothedVelocity;
	float smoothedTurnTime = 0.1f;
	float smoothedSpeedTime = 0.1f;
	float velocityY;
	float jumpHeight = 1;

	bool movementDisabled;

	Transform cameraTransform;
	CharacterController controller;

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
	}

	bool onGround () {
		return Physics.Raycast (transform.position, Vector3.down, 1.05f);
	}

	// Update is called once per frame
	void Update () {

		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 inputDirection = input.normalized;

		if (!movementDisabled) {
			Move (inputDirection);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			velocityY = 0;
			Jump();
		}
			
	}

	void Jump() {
		if (controller.isGrounded) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}

	void Move(Vector3 inputDir) {

		if (inputDir != Vector3.zero) {
			float targetAngle = Mathf.Atan2 (inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref smoothedTurnVelocity, smoothedTurnTime);
		}

		currentSpeed = Mathf.SmoothDamp (currentSpeed, inputDir.magnitude * moveSpeed, ref smoothedVelocity, smoothedSpeedTime);

		if (!controller.isGrounded) {
			velocityY += Time.deltaTime * gravity;
		}

		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);

		if (controller.isGrounded) {
			velocityY += 0;
		}
	}



}

