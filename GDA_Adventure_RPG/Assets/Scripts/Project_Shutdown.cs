using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project_Shutdown : MonoBehaviour {

    float timeElapsed;

	void Start () {
        timeElapsed = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= timeElapsed + 2)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
	}
}
