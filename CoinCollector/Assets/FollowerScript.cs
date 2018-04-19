using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour {

	public float speed = 5f;

	public GameObject bucket;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 curPosition = this.transform.position;
		Vector3 translation = Vector3.zero;

		translation = (bucket.transform.position - curPosition).normalized;


		this.transform.position = curPosition + translation * Time.deltaTime * speed;
	}
}
