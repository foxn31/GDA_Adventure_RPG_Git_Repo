using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float speed = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 curPosition = this.transform.position;

		Vector3 translation = Vector3.zero;

		translation.x = Input.GetAxis ("Horizontal");
		translation.z = Input.GetAxis ("Vertical");

		this.transform.position = curPosition + translation * Time.deltaTime * speed;
	}

	private void OncCollisionEnter(Collision collision)
	{
		Destroy (this.gameObject);
	}
}
