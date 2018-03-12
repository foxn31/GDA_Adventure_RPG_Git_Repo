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
	bool isInventoryShowing;

	Vector3 inputDirection;
	Vector3 storedInput;

	Transform cameraTransform;
	CharacterController controller;
	Animator animator;

	void Start () {
		movementDisabled = false;
		cameraTransform = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator> ();
		//animator.SetInteger ("currentWeapon", 1);
	}

	bool onGround () {
		return Physics.Raycast (transform.position, Vector3.down, 1.05f);
	}	

	void Update () {
		if (!movementDisabled) {
			Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
			inputDirection = input.normalized;

			if (Input.GetKeyDown (KeyCode.Space) && controller.isGrounded) {
				velocityY = 0;
				Jump();
			}
			else if (controller.isGrounded) {
				animator.SetBool ("onAirborne", false);
			}
		} 
		else if (movementDisabled) {
			inputDirection = Vector3.zero;
		}

		if (!animator.GetBool("completeLand")) {
			//inputDirection = Vector3.zero;
			MoveWithoutCam (storedInput/2, false);	
		}
		else if (animator.GetBool("completeLand")) {
			Move (inputDirection, running);	
			animatorSpeedPercent = ((running) ? currentSpeed/runSpeed : currentSpeed/walkSpeed *.55f);
			animator.SetFloat ("moveSpeed", animatorSpeedPercent, smoothedSpeedTime, Time.deltaTime);
		}
	}
		
	void Jump () {
		if (controller.isGrounded && animator.GetBool("completeLand")) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;

			updateStoredMovement ();

			animator.SetBool ("onAirborne", true);
			animator.SetInteger ("airborneSpc", 1);
		}
	}

	void Move (Vector3 inputDir, bool running) {
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
		
	void MoveWithoutCam (Vector3 inputDir, bool running) {

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
		if (!movementDisabled) {
			movementDisabled = true;
		}
	}

	public void EnableMove() {
		if (movementDisabled) {
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

	float getSpeed()
	{
		if (running) {
			return runSpeed;
		} else if (movementDisabled) {
			return 0;
		} else {
			return walkSpeed;
		}
	}

	void updateStoredMovement() {
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		storedInput = input.normalized;
	}
}

