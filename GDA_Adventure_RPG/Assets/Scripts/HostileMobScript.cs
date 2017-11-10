using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileMobScript : MonoBehaviour {

	Vector3 startingPosition = new Vector3(12,1,10);
	Vector3 distanceFromPlayer;

	float speed = 7;

	public GameObject mob;
	public GameObject player;
	//public static event System.Action Aggroed;


	// Use this for initialization
	void Start () {
		mob.transform.position = startingPosition;
		//HostileMobScript.Aggroed += aggroed ();
	}


	// Update is called once per frame
	void Update ()
	{
		Vector3 distanceFromPlayer = Vector3.Distance (player.transform.position, startingPosition);
		Vector3 directionToPlayer = distanceFromPlayer.normalized;
		Vector3 velocity = directionToPlayer * speed;
		float distanceToPlayer = distanceFromPlayer.magnitude;
	}
}

			