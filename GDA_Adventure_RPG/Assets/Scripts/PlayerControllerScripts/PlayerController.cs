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

    Vector3 velocity = Vector3.zero;
    float movementSpeed = 0;
    float speedSmoothingVelocity = 0;
    float movementSmoothingTime = 0.1f;

    bool movementEnabled = true;

	Transform cameraTransform;
	CharacterController controller;
	Animator animator;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        state = MoveState.Idle;

        animator.SetBool("isAirborne", false);
        animator.SetFloat("moveSpeed", 0);
        animator.SetFloat("fallSpeed", 0);
        animator.SetInteger("airborneSpc", 0);
    }

    void Update()
    {
        // Get input direction
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        // Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
        float goalSpeed = (direction.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        // Set the smoothing time: this value is reduced if the player is in air
        float smoothing = movementSmoothingTime / (state == MoveState.Airborne ? airControlPercent : 1);
        

        // Perform state transitions and state-specific calculations
        if (state == MoveState.Landing)
        {
            Debug.Log("Landing");
            goalSpeed = 0; // Player can't move while landing
            if (animator.GetBool("completeLand"))
            {
                state = MoveState.Idle;
            }
        }
        else if (state == MoveState.Idle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
                animator.SetBool("isAirborne", true);
            }
            else if (movementSpeed > 0)
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
                animator.SetBool("isAirborne", true);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
                animator.SetBool("isAirborne", true);
            }
            else if (movementSpeed < 0)
            {
                // Player stopped moving
                state = MoveState.Idle;
            }

            // Update facing direction
            if (direction != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) + cameraTransform.eulerAngles.y * Mathf.Deg2Rad;
                transform.eulerAngles = Vector3.up * (targetAngle * Mathf.Rad2Deg);
            }
        }
        else if (state == MoveState.Airborne)
        {
            if (controller.isGrounded)
            {
                // Player has landed
                state = MoveState.Landing;
                animator.SetBool("isAirborne", false);
            }
            else
            {
                // Apply gravity
                velocity.y += gravity * Time.deltaTime;
                animator.SetFloat("fallSpeed", velocity.y);
            }
        }

        // Update the move speed using smoothing
        movementSpeed = Mathf.SmoothDamp(movementSpeed, goalSpeed, ref speedSmoothingVelocity, smoothing);
        animator.SetFloat("moveSpeed", movementSpeed);
        //Debug.Log(goalSpeed + ", " + movementSpeed);
        // Perform the actual movement
        controller.Move((velocity + transform.forward * movementSpeed) * Time.deltaTime);
    }

    public void DisableMove()
    {
        movementEnabled = false;
    }

    public void EnableMove()
    {
        movementEnabled = true;
    }
}

