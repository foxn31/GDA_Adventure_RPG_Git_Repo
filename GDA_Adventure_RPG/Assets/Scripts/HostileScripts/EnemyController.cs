using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	float timeSinceAggro;
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
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator> ();
		startPosition = transform.position;
	}

	void Update () {
		float distanceToTarget = Vector3.Distance (startPosition, target.position);
		float myDistanceFromStart = Vector3.Distance (startPosition, transform.position);
		float attackDistance = Vector3.Distance (transform.position, target.position);


		if (myDistanceFromStart <= .1f) {
			timeSinceAggro = 0;
			hasAggro = false;
		}

		if (distanceToTarget < minAggroRange) {
			hasAggro = true;
			timeSinceAggro = Time.time;
		}

		if (hasAggro) {
			if(myDistanceFromStart < maxAggroRange && !isAttacking && attackDistance <= attackRange) 
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

		moveAnimSpeed = Mathf.Lerp(0, 1, Mathf.Clamp01(timeSinceAggro/3));
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
