using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum MoveState
    {
        Idle,
        Moving,
        Airborne,
        Landing,
        Combat,
        Attacking
    }
    MoveState state;

    public float walkSpeed = 3;
	public float runSpeed = 8;

    [Range(0, 1)]
    public float airControlPercent = 0.1f;

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

    bool attacking = false;
    bool inCombat = false;
    float inCombatStartTime = 0f;
    public float inCombatDropoff = 4f;
    float attackInitialTime = 0f;
    public float attackChainDropoff = 2f;
    int tempAttackSpc = -1;

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
        animator.SetBool("attackComplete", true);
        animator.SetBool("inCombat", false);
        animator.SetInteger("animationSize", 0);
        animator.SetInteger("attackSpc", -1);
    }

    void Update()
    {
        float thisFrame = Time.time;

        // Check at the beginning of the frame if the player is still in combat
        if (CombatDropped() && inCombat)
        {
            inCombat = false;
            animator.SetBool("inCombat", false);
            animator.SetInteger("attackSpc", -1);
            Debug.Log("EXITING COMBAT");
        }
        // Check if the player is at the end of attack chain or has gone over the permitted chain time, reset variables if true
        if ((tempAttackSpc + 1 == animator.GetInteger("animationSize") || ChainDropped()))
        {
            //Debug.Log("Tree 1");
            //Debug.Log(tempAttackSpc + 1 == animator.GetInteger("animationSize"));
            //Debug.Log(ChainDropped());

            tempAttackSpc = -1;
            animator.SetInteger("attackSpc", -1);
        }

        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
		// Set the goal speed:
        //   0 if no movement buttons are pressed
        //   runSpeed if the player is running
        //   walkSpeed if the player is not running
		float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		// Check if LeftShift is pressed 
		running = Input.GetKey (KeyCode.LeftShift);

		// Change the target speed based on if walking or running
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
                animator.SetInteger("airborneSpc", -1);
                Debug.Log("Landing to Idle");
            }
        }
        else if (state == MoveState.Idle)
        {
            //Debug.Log("IDLE");

            // Check if the player should be attacking
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("CLICKING");
                state = MoveState.Attacking;
                //return;
            }
            // Check if the player is jumping
            else if (Input.GetKeyDown(KeyCode.Space))
            {
				
                // Jump
                //velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 3);
                animator.SetBool("onAirborne", true);
            }
            //Check if the player is moving
            else if (currentSpeed > 0.01)
            {
                // Player started moving
                state = MoveState.Moving;
            }
        }
        else if (state == MoveState.Moving)
        {
            //Debug.Log("MOVING");

            if (!controller.isGrounded)
            {
                // Player is falling
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 0);
                animator.SetBool("onAirborne", true);
            }
            // Check if the player is jumping
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Jump
                velocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 2);
                animator.SetBool("onAirborne", true);
            }
            // Check if the player has stopped moving
            else if (currentSpeed < 0.01)
            {            
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
            Debug.Log("AIRORNE");

            // For straight falling
            if (!animator.GetBool ("completeLand") && animator.GetInteger("airborneSpc") == 1) 
			{
				currentSpeed = 0;
			}
            // Player has landed
            if (controller.isGrounded)
            {              
                state = MoveState.Landing;
                animator.SetBool("onAirborne", false);
            }
            // Apply gravity
            else
            {
                velocity.y += gravity * Time.deltaTime;
                animator.SetFloat("fallSpeed", velocity.y);
            }
        }
        //Outside first if-else to allow single frame combat transitions
        if (state == MoveState.Attacking)
        {
            Debug.Log(Time.time == thisFrame);

            Debug.Log("ATTACKING");

            if (animator.GetInteger("currentWeapon") == 0)
            {
                Debug.Log("NO WEAPON");
                state = MoveState.Idle;
                return;
            }

            inCombat = true;
            SetCombatInitial();

            // Set Animator stuff here 
            animator.SetBool("inCombat", true);

            //Debug.Log("ATTACK SPC: " + tempAttackSpc);
            
        /*    // Check if the player is at the end of attack chain or has gone over the permitted chain time, reset variables if true
            if ((tempAttackSpc + 1 == animator.GetInteger("animationSize") || ChainDropped()))
            {
                Debug.Log("Tree 1");
                Debug.Log(tempAttackSpc + 1 == animator.GetInteger("animationSize"));
                Debug.Log(ChainDropped());

                tempAttackSpc = -1;
                animator.SetInteger("attackSpc", -1);
            }   */
            // Perform attack
            if (true)
            {
                SetChainInitial();
                //Debug.Log(GetChainInitial());
                Debug.Log("Tree 2");

                tempAttackSpc++;
                animator.SetInteger("attackSpc", tempAttackSpc);
                //SetChainInitial();
                state = MoveState.Combat;

                //animator.SetInteger("attackSpc", -1);
            }
            
        }
        else if (state == MoveState.Combat)
        {
            //Debug.Log("IN COMBAT");

            // If the player is in the middle of an attack
            if (!animator.GetBool("completeAttack"))
            {
                targetSpeed = 0f;
            }
            else
            {
                Debug.Log("SHOULD RETURN TO IDLE FROM COMBAT");
                animator.SetInteger("attackSpc", -1);
                state = MoveState.Idle;
            }
        }

        // Update the move speed using smoothing
    //    movementSpeed = Mathf.SmoothDamp(movementSpeed, goalSpeed, ref movementSmoothingVelocity, smoothing);
        // Perform the actual movement
    //    controller.Move((velocity + transform.forward * movementSpeed) * Time.deltaTime);

        // Calculate the smoothed speed and move the attached character controller
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref movementSmoothVelocity, smoothing);
		controller.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);

        // Calculate target animations and set animator variables to affect animations if not currently attacking
		baseAnimationSpeed = ((running) ? 1 : .7f) * inputDirection.magnitude;
        //if (!inCombat)
        //{
            animator.SetFloat("moveSpeed", baseAnimationSpeed, movementSmoothTime, Time.deltaTime);
        //}
    }

    public void DisableMove()
    {
        movementEnabled = false;
    }

    public void EnableMove()
    {
        movementEnabled = true;
    }

    
    // Return true if the player has dropped out of combat
    public bool CombatDropped()
    {
        return (Time.time - inCombatStartTime) > inCombatDropoff;
    }

    public void SetCombatInitial()
    {
        inCombatStartTime = Time.time;
    }

    // Return true if attacks can't be chained
    public bool ChainDropped()
    {
        return (Time.time - attackInitialTime) > attackChainDropoff;
    }

    public void SetChainInitial()
    {
        Debug.Log("SETTING CHAIN INITIAL");
        attackInitialTime = Time.time;
    }

    public float GetChainInitial()
    {
        return attackInitialTime;
    }

    /*	public void DisableTurn()
        {
            inputDirection = new Vector3 (0, 0, 0);
        } */
}

