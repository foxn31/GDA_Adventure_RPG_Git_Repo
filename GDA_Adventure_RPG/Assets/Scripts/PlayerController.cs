using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	public float moveSpeed = 8;
	public float turnTime = 20;

	float angle;
	float currentSpeed;
	float smoothedVelocity;
	float smoothedTurnVelocity;
	float smoothedTurnTime = 0.1f;
	float smoothedSpeedTime = 0.1f;

	//Rigidbody rigidBody;
	Transform cameraTransform;

	// Use this for initialization
	void Start () {
		//rigidBody = GetComponent<Rigidbody> ();
		cameraTransform = Camera.main.transform;
	}

	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 inputDirection = input.normalized;

		if (inputDirection != Vector3.zero) {
			float targetAngle = Mathf.Atan2 (inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref smoothedTurnVelocity, smoothedTurnTime);
		}

		currentSpeed = Mathf.SmoothDamp (currentSpeed, inputDirection.magnitude * moveSpeed, ref smoothedVelocity, smoothedSpeedTime);

		transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);
	}

	void FixedUpdate() {
		//rigidBody.MovePosition ( transform.forward + velocity * moveSpeed * Time.deltaTime);
		//rigidBody.MoveRotation ( Quaternion.Euler (Vector3.up * angle));
		//rigidBody.transform.Translate (transform.forward * velocity * moveSpeed * Time.deltaTime);
	}

}

