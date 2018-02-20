using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterName : MonoBehaviour {
	
	// Update is called once per frame

	public Transform name;

	void Update () {
		
		name.LookAt (Camera.main.transform);
	//		Camera.main.transform);

	}
}
