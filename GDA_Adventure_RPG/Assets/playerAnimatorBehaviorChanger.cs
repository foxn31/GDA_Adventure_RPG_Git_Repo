using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimatorBehaviorChanger : StateMachineBehaviour {

	public bool completeLandEnter;
	public bool completeLandExit;

    public bool incrementAttackSpcExit;
    public bool completeAttackEnter;
    public bool completeAttackExit;

	//public bool onCompleteLand;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (completeLandEnter)
        {
			animator.SetBool("completeLand", false);
		}

        if (completeAttackEnter)
        {
            animator.SetBool("completeAttack", false);
        }
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (completeLandExit)
        {
			animator.SetBool("completeLand", true);
            Debug.Log("Complete Land on Exit");
		}

        if (completeAttackExit)
        {
            animator.SetBool("completeAttack", true);
            Debug.Log("Complete Attack on Exit");
        }

        if (incrementAttackSpcExit)
        {
            int num = animator.GetInteger("attackSpc");
            animator.SetInteger("attackSpc", num++);
            Debug.Log("Incrementing Attack");
        }
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

}
