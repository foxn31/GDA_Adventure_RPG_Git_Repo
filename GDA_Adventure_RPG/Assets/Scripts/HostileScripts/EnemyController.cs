using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
	public float AggroRange;
	public float maxAggro;  //Change this in inspector??
	public float speed;
	public Vector3 startPosition; //= new Vector3 (-39, 1, -27);
	public bool hasAggro = false;

    Transform target;
    NavMeshAgent agent;

	void Start () {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
		agent.transform.position = startPosition;
	}

	void Update () {
		float aggroDistance = Vector3.Distance (startPosition, target.position);
		float distanceFromStart = Vector3.Distance (startPosition, transform.position);

		if (aggroDistance < AggroRange) {
			hasAggro = true;
		}

		if (hasAggro) {
			if (distanceFromStart < maxAggro) {
				AttackPlayer ();
			} else if (distanceFromStart >= maxAggro) {
				hasAggro = false;
				ReturnToStart ();
			}
		}
	}
		
	void AttackPlayer() {
		agent.speed = speed;
		agent.SetDestination (target.position);
		Debug.Log (agent.speed);
	}

	void ReturnToStart() {
		agent.speed = speed * (3 / 2f);
		agent.SetDestination (startPosition);
		Debug.Log (agent.speed);
	}

		
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(startPosition, AggroRange);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (startPosition, maxAggro);
    }
}
