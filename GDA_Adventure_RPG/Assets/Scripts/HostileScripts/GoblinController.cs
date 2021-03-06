﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinController : MonoBehaviour
{		
	public enum States
	{
		Idle,
		Attack,
		Flee,
		Chase,
		ReturnToStart
	}
	public States goblinState = States.Idle;

	Transform target;
	NavMeshAgent agent;
	Animator animator;
	Vector3 startPosition;

	private float animSmoothedSpeedTime = 0.1f;
	//private float moveAnimSpeed;
	private float speed = 5;
	private float health = 100;
	private float minAggroRange = 5f;
	private float maxAggroRange = 20f;
	private float attackRange = 1f;

	void Start () {		
		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponentInChildren<Animator> ();

		startPosition = transform.position;
		startPosition = transform.position;

		target = PlayerManager.instance.player.transform;
	}

	void Update()
	{
		float startDistanceToTarget = Vector3.Distance (startPosition, target.position);
		float distanceFromStart = Vector3.Distance (startPosition, transform.position);
		float distanceFromTarget = Vector3.Distance (transform.position, target.position);

		switch (goblinState) {
		case States.Idle:

			if(distanceFromTarget < minAggroRange) {
				goblinState = States.Attack;
			}
		
			break;

		case States.Attack:
				
			if (health < percentHealth(10)) {
				goblinState = States.ReturnToStart;
			}
			else if (distanceFromTarget > attackRange) {
				goblinState = States.Chase;
			}
			break;

		case States.Flee:
				
			if (health > percentHealth(60)) {
				goblinState = States.ReturnToStart;
			}
			break;

		case States.Chase:
				
			if (distanceFromTarget < attackRange) {
				goblinState = States.Attack;
			}
			if (distanceFromStart > maxAggroRange) {
				goblinState = States.ReturnToStart;
			}
			break;

		case States.ReturnToStart:

			if (distanceFromTarget < minAggroRange/2) {
				goblinState = States.Attack;
			}
			if (distanceFromStart < .1f ) {
				goblinState = States.Idle;
			}

			break;
		}
			
		DoAction(target, goblinState);
	}
		
	public void DoAction(Transform target, States enemyMode) {
		float fleeSpeed = 10f;
		float attackSpeed = 5f;

		switch (enemyMode)
		{
		case States.Attack:

			//Attack player
			agent.SetDestination(transform.position);

			transform.rotation = Quaternion.LookRotation(target.position - transform.position);

			animator.SetFloat ("moveSpeed", 0, animSmoothedSpeedTime, Time.deltaTime);	//Temp

			//animator.

			break;

		case States.Idle:

			//Idle
			animator.SetFloat ("moveSpeed", 0, animSmoothedSpeedTime, Time.deltaTime);

			break;

		case States.Flee:

			agent.speed = speed * (3 / 2f);
			agent.SetDestination (startPosition);

			animator.SetFloat ("moveSpeed", 1, animSmoothedSpeedTime, Time.deltaTime);

			//Move away from player
			//Look in the opposite direction
			//enemyObj.rotation = Quaternion.LookRotation(enemyObj.position - playerObj.position);

			//Move
			//enemyObj.Translate(enemyObj.forward * fleeSpeed * Time.deltaTime);

			break;

		case States.Chase:

			agent.speed = speed;
			agent.SetDestination (target.position);

			animator.SetFloat ("moveSpeed", 1, animSmoothedSpeedTime, Time.deltaTime);

			//Look at the player
			//enemyObj.rotation = Quaternion.LookRotation(playerObj.position - enemyObj.position);

			//Move
			//.Translate(enemyObj.forward * attackSpeed * Time.deltaTime);

			break;

		case States.ReturnToStart:

			agent.speed = speed * (3 / 2f);
			agent.SetDestination (startPosition);

			animator.SetFloat ("moveSpeed", 1, animSmoothedSpeedTime, Time.deltaTime);


			break;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(startPosition, minAggroRange);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (startPosition, maxAggroRange);
	}

	public float percentHealth(float percent) {
		return (percent/100) * health;
	}

}