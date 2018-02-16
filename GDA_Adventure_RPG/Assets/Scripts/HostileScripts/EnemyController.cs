using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	public enum States { IDLE, CHASING, ATTACKING, FLEEING};
	public States state;

	//float timeSinceAggro;
	public float minAggroRange;
	public float maxAggroRange;
	public float attackRange;
	public float speed;
	float moveAnimSpeed = 0;
	float animSmoothedSpeedTime = 0.1f;
	//float animSmoothVelocity;
	//float animSmoothedSpeedTime = 0.1f;

	public bool hasAggro = false;
	public bool isAttacking = false;

	private Vector3 startPosition;

    Transform target;
    NavMeshAgent agent;
	Animator animator;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator> ();

		target = PlayerManager.instance.player.transform;
		startPosition = transform.position;

		state = States.IDLE;
	}

	void Update () {
		float startDistanceToTarget = Vector3.Distance (startPosition, target.position);
		float myDistanceFromStart = Vector3.Distance (startPosition, transform.position);
		float myDistanceFromTarget = Vector3.Distance (transform.position, target.position);

		moveAnimSpeed = 1;//Mathf.Lerp(0, 1, Mathf.Clamp01(timeSinceAggro/3));

		if (myDistanceFromStart <= .1f) {
			//timeSinceAggro = 0;
			hasAggro = false;
			moveAnimSpeed = 0;
		}
		if (startDistanceToTarget < minAggroRange) {
			hasAggro = true;
			//timeSinceAggro = Time.time;
		}

		if (hasAggro) {
			if(myDistanceFromStart < maxAggroRange && !isAttacking && myDistanceFromTarget <= attackRange) 
			{
				hasAggro = true;
				AttackPlayer ();
			}
			else if (myDistanceFromStart < maxAggroRange && !isAttacking) {
				hasAggro = true;
				MoveTowardsPlayer ();
			} 
			else if (myDistanceFromStart >= maxAggroRange && !isAttacking) {
				hasAggro = false;
				ReturnToStart ();
			}
		}
			

		if(myDistanceFromTarget < 1) {
			moveAnimSpeed = 0;
		}

		animator.SetFloat ("moveSpeed", moveAnimSpeed, animSmoothedSpeedTime, Time.deltaTime);
	}
		
	void MoveTowardsPlayer() {
		agent.speed = speed;
		agent.SetDestination (target.position);
	}

	void ReturnToStart() {
		agent.speed = speed * (3 / 2f);
		agent.SetDestination (startPosition);
	}

	void AttackPlayer() { //Take in a specific attack?
		//attack here
		agent.isStopped = true;
		isAttacking = true;
		print ("attacking");
		isAttacking = false;
		agent.isStopped = false;
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(startPosition, minAggroRange);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (startPosition, maxAggroRange);
    }
}
