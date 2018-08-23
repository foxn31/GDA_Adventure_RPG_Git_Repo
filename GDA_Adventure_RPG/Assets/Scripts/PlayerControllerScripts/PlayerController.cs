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
	float jumpHeight = 2;

	float baseAnimationSpeed;

    Vector3 velocity = Vector3.zero;
	float currentSpeed = 0;
	float targetSpeed = 0;
    float movementSmoothVelocity = 0;
    float movementSmoothTime = 0.1f;

	float turnSmoothVelocity = 0;
	float turnSmoothTime = .05f;

	bool movementEnabled = true;
	bool running;

    bool attacking = false;
    bool inCombat = false;
    float inCombatStartTime = 0f;
    float inCombatDropoff = 4f;
    float attackInitialTime = 0f;
    float attackChainDropoff = 4f;
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

        state = MoveState.Airborne;

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
        // Check at the beginning of the frame if the player is still in combat
        if (CombatTimeout() && inCombat)
        {
            inCombat = false;
            animator.SetBool("inCombat", false);
            animator.SetInteger("attackSpc", -1);
        }
        // Check if the player is at the end of attack chain or has gone over the permitted chain time, reset variables if true
        if ((tempAttackSpc + 1 == animator.GetInteger("animationSize") || ChainTimeout()))
        {
            //if (ChainTimeout()) { Debug.Log("Chain Timeout"); }
            //if (tempAttackSpc + 1 == animator.GetInteger("animationSize")) { Debug.Log("End of Chain"); }
            tempAttackSpc = -1;
            animator.SetInteger("attackSpc", -1);
        }

        // Get input direction
        inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        /*
         * Set the goal speed:
         *      0 if no movement buttons are pressed
         *      runSpeed if the player is running
         *      walkSpeed if the player is not running
         */  
        float goalSpeed = (inputDirection.magnitude == 0 || !movementEnabled) ? 0 : Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

		// Check if LeftShift is pressed and change target speed accordingly
		running = Input.GetKey (KeyCode.LeftShift);
		targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;

		// Set the smoothing time: this value is reduced if the player is in air
        float smoothing = movementSmoothTime / (state == MoveState.Airborne ? airControlPercent : 1);

        // Perform state transitions and state-specific calculations      
        if (state == MoveState.Idle)
        {
            // Check if the player should be attacking
            if (Input.GetMouseButtonDown(0))
            {
                state = MoveState.Attacking;
            }
            // Check if the player is jumping
            else if (Input.GetKeyDown(KeyCode.Space))
            {				
                // Jump
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 3);
                animator.SetBool("onAirborne", true);
            }
            // Player is falling
            if (!controller.isGrounded)
            {
                //animator.SetInteger("airborneSpc", 1);
                animator.SetBool("onAirborne", true);
                state = MoveState.Airborne;
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
            // Player is falling
            if (!controller.isGrounded)
            {
                state = MoveState.Airborne;
				animator.SetInteger ("airborneSpc", 0);
                animator.SetBool("onAirborne", true);
            }
            // Check if the player should be attacking
            else if (Input.GetMouseButtonDown(0))
            {
                state = MoveState.Attacking;
            }
            // Check if the player is jumping
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(-1 * gravity * jumpHeight);
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
            // Player has landed
            if (controller.isGrounded)
            {
                state = MoveState.Landing;
                animator.SetBool("onAirborne", false);
            }
            // Apply gravity if player has not landed
            /*
            else
            {
                velocity.y += gravity * Time.deltaTime;
                animator.SetFloat("fallSpeed", velocity.y);
            }
            */
        }
        else if (state == MoveState.Landing)
        {
            if (animator.GetInteger("airborneSpc") == 0)
            {
                // Player can't move while landing
                targetSpeed = 0f;
            }
            // Fall-to-Roll animation
            if (animator.GetInteger("airborneSpc") == 2)
            {
                targetSpeed = 5f;
            }
            
            // Landing animation is completed
            if (animator.GetBool("completeLand"))
            {
                state = MoveState.Idle;
                animator.SetInteger("airborneSpc", 0);
                animator.SetBool("completeLand", false);
            }
        }
        //Outside first if-else to allow single frame combat transitions
        if (state == MoveState.Attacking)
        {
            if (animator.GetInteger("currentWeapon") == 0)
            {
                Debug.Log("NO WEAPON");
                state = MoveState.Idle;
            }

            // Perform attack
            if (state != MoveState.Idle)
            {
                SetChainInitial();
                SetCombatInitial();

                tempAttackSpc++;
                animator.SetInteger("attackSpc", tempAttackSpc);
                animator.SetBool("inCombat", true);
                inCombat = true;
                state = MoveState.Combat;
            }
            
        }
        else if (state == MoveState.Combat)
        {
            // If the player is in the middle of an attack
            if (!animator.GetBool("completeAttack"))
            {
                targetSpeed = 0f;
            }
            else
            {
                animator.SetInteger("attackSpc", -1);
                state = MoveState.Idle;
            }
        }

        // Apply gravity outside 
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            animator.SetFloat("fallSpeed", velocity.y);
        }

        // Calculate the smoothed speed and move the attached character controller
        currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref movementSmoothVelocity, smoothing);
		controller.Move((velocity + transform.forward * currentSpeed) * Time.deltaTime);

        // Calculate target animations and set animator variables to affect animations if not currently attacking
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

    
    // Return true if the player has dropped out of combat
    public bool CombatTimeout()
    {
        return (Time.time - inCombatStartTime) > inCombatDropoff;
    }

    public void SetCombatInitial()
    {
        inCombatStartTime = Time.time;
    }

    // Return true if attacks can't be chained
    public bool ChainTimeout()
    {
        return (Time.time - attackInitialTime) > attackChainDropoff;
    }

    public void SetChainInitial()
    {
        attackInitialTime = Time.time;
    }

    /*	public void DisableTurn()
        {
            inputDirection = new Vector3 (0, 0, 0);
        } */
}

