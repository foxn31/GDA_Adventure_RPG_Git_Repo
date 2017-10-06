using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Check_For_Fall : MonoBehaviour {

    Transform target;
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (((target.position.x > 15.5 || target.position.x < -15.5) ||
            (target.position.z > 15.5 || target.position.z < -15.5)) && target.position.y < -5)
        {
            SceneManager.LoadScene("Additional Test Scene");
        }
    }
}
