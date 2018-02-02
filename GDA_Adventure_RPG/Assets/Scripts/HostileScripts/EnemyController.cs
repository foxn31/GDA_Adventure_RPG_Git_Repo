using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public float lookRadius = 5f;

	public float speed = 10f;
	public float maxAggro = 20f;
	public Vector3 startPosition = new Vector3 (-39, 1, -27);
	public bool hasAggro = false;

    Transform target;
    NavMeshAgent agent;

	void Start () {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
		agent.transform.position = startPosition;
		agent.speed = speed;
	}

	void Update () {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
		float distanceFromStart = Vector3.Distance (startPosition, transform.position);
		if ((distanceToPlayer <= lookRadius) && distanceFromStart <= maxAggro) {
			agent.SetDestination (target.position);
			hasAggro = true;
		} else if (distanceFromStart >= maxAggro) {
			agent.SetDestination (startPosition);
			hasAggro = false;
		}
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, maxAggro);
    }
}
