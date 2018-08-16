using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum MoveState
    {
        Idle,
        Moving,
        Airborne,
        Landing
    }

	public float walkSpeed = 3;
	public float runSpeed = 8;

    [Range(0, 1)]
    public float airControlPercent = 0.1f;

    MoveState state;

    float gravity = -15;
	float jumpHeight = 1;

	float baseAnimationSpeed;

    Vector3 velocity = Vector3.zero;
    //float movementSpeed = 0;
	float currentSpeed = 0;
	float targetSpeed = 0;
    float movementSmoothVelocity = 0;
    float movementSmoothTime = 0.1f;

	//float turnSpeed = 0;
	float turnSmoothVelocity = 0;
	float turnSmoothTime = .05f;

	bool movementEnabled = true;
	bool running;

	Transform cameraTransform;
	CharacterController controller;
	Animator animator;

	Vector3 inputDirection;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        state = MoveState.Idle;

        animator.SetBool("onAirborne", false);
        animator.SetFloat("moveSpeed", 0);
        animator.SetFloat("fallSpeed", 0);
        animator.SetInteger("airborneSpc", 0);
    }

    void Update()
    {
        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
		// Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
		float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		//
		running = Input.GetKey (KeyCode.LeftShift);

		//
		targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;

		// Set the smoothing time: this value is reduced if the player is in air
        float smoothing = movementSmoothTime / (state == MoveState.Airborne ? airControlPercent : 1);
        
        // Perform state transitions and state-specific calculations
        if (state == MoveState.Landing)
        {
            Debug.Log("Landing");

            targetSpeed = 0; // Player can't move while landing
			if (animator.GetInteger ("airborneSpc") == 2) 
			{
				Debug.Log("Tree 1");
				targetSpeed = 5f;
			}
            if (animator.GetBool("completeLand"))
            {
                state = MoveState.Idle;
                Debug.Log("Landing to Idle");
            }
        }
        else if (state == MoveState.Idle)
        {
			//Debug.Log("Idling");

            if (Input.GetKeyDown(KeyCode.Space))
            {
				
                // Jump
                //velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 3);
                animator.SetBool("onAirborne", true);
            }
            else if (currentSpeed > 0)
            {
                // Player started moving
                state = MoveState.Moving;
            }
        }
        else if (state == MoveState.Moving)
        {
            if (!controller.isGrounded)
            {
                // Player is falling
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 0);
                animator.SetBool("onAirborne", true);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 2);
                animator.SetBool("onAirborne", true);
            }
            else if (currentSpeed < 0)
            {
                // Player stopped moving
                state = MoveState.Idle;
            }

            // Update facing direction
			if (inputDirection != Vector3.zero)
            {
                //float targetAngle = Mathf.Atan2(direction.x, direction.z) + cameraTransform.eulerAngles.y * Mathf.Deg2Rad;
				//transform.eulerAngles = Vector3.up * (targetAngle * Mathf.Rad2Deg);

				float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			}
        }
        else if (state == MoveState.Airborne)
        {
			if (!animator.GetBool ("completeLand") && animator.GetInteger("airborneSpc") == 3) 
			{
				currentSpeed = 0;
			}
            if (controller.isGrounded)
            {
                // Player has landed
                state = MoveState.Landing;
                animator.SetBool("onAirborne", false);
            }
            else
            {
                // Apply gravity
                velocity.y += gravity * Time.deltaTime;
                animator.SetFloat("fallSpeed", velocity.y);
            }
        }

        // Update the move speed using smoothing
    //    movementSpeed = Mathf.SmoothDamp(movementSpeed, goalSpeed, ref movementSmoothingVelocity, smoothing);
        // Perform the actual movement
    //    controller.Move((velocity + transform.forward * movementSpeed) * Time.deltaTime);

		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref movementSmoothVelocity, smoothing);

		controller.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);

		baseAnimationSpeed = ((running) ? 1 : .7f) * inputDirection.magnitude;
		animator.SetFloat("moveSpeed", baseAnimationSpeed, movementSmoothTime, Time.deltaTime);

    }

    public void DisableMove()
    {
        movementEnabled = false;
    }

    public void EnableMove()
    {
        movementEnabled = true;
    }

/*	public void DisableTurn()
	{
		inputDirection = new Vector3 (0, 0, 0);
	} */
}

