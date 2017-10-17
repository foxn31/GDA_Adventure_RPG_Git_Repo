using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float walkSpeed = 3;
	public float runSpeed = 8;

	float animatorSpeedPercent;
	float speed;
	float gravity = -15;
	float angle;
	float currentSpeed;
	float smoothedTurnVelocity;
	float smoothedVelocity;
	float smoothedTurnTime = 0.1f;
	float smoothedSpeedTime = 0.1f;
	float velocityY;
	float jumpHeight = 1;

	[Range(0,1)]
	public float airControlPercent;

	bool running;
	bool movementDisabled;

	Vector3 inputDirection;

	Transform cameraTransform;
	CharacterController controller;
	Animator animator;

	void Start () {
		movementDisabled = false;
		cameraTransform = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator> ();
	}

	bool onGround () {
		return Physics.Raycast (transform.position, Vector3.down, 1.05f);
	}

	void Update () {

		if (!movementDisabled) {
			Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
			inputDirection = input.normalized;
		} else if (movementDisabled) {
			inputDirection = Vector3.zero;
		}

		Move (inputDirection, running);	


		if (Input.GetKeyDown (KeyCode.Space)) {
			velocityY = 0;
			Jump();
		}
			
		animatorSpeedPercent = ((running) ? currentSpeed/runSpeed : currentSpeed/walkSpeed *.5f);
		animator.SetFloat ("speedPercent", animatorSpeedPercent, smoothedSpeedTime, Time.deltaTime);
	}

	void Jump() {
		if (controller.isGrounded) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}

	void Move(Vector3 inputDir, bool running) {
		if (inputDir != Vector3.zero) {
			float targetAngle = Mathf.Atan2 (inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref smoothedTurnVelocity, GetModififedSmoothTime (smoothedTurnTime));
		}

		running = Input.GetKey (KeyCode.LeftShift);
		speed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

		currentSpeed = Mathf.SmoothDamp (currentSpeed, speed, ref smoothedVelocity, GetModififedSmoothTime (smoothedSpeedTime));

		if (!controller.isGrounded) {
			velocityY += Time.deltaTime * gravity;
		}

		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move (velocity * Time.deltaTime);

		if (controller.isGrounded) {
			velocityY += 0;
		}

	}
		
	public void DisableMove() {
		if (movementDisabled == false) {
			movementDisabled = true;
		}
	}

	public void EnableMove() {
		if (movementDisabled == true) {
			movementDisabled = false;
		}
	}

	float GetModififedSmoothTime(float smoothTime) {
		if (controller.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

}

