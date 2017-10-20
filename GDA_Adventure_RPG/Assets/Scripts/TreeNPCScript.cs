using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNPCScript : MonoBehaviour {

	float speed = 3f;
	Vector3 direction = Vector3.left;
	// Use this for initialization
	void Start () {
		transform.position= new Vector3 (6, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.position.x > -4) && (transform.position.z>-5)) {
			transform.Translate (Vector3.left * speed * Time.deltaTime);
		}
		else if ((transform.position.z > -4) && transform.position.z 
		transform.Translate (Vector3.back * speed * Time.deltaTime);
		transform.Translate (Vector3.right * speed * Time.deltaTime);
		transform.Translate (Vector3.forward * speed * Time.deltaTime);


		
	}
}
