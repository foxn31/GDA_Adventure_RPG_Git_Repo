using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject talkPrompt;

	void Start () {
		NPCScript.ShowTalkPrompt += showTalkPrompt;
		NPCScript.HideTalkPrompt += hideTalkPrompt;

	}
	
	void Update () {
		
	}

	void showTalkPrompt() {
		if (!talkPrompt.activeSelf) {
			talkPrompt.SetActive (true);
		}
	}

	void hideTalkPrompt() {
		if (talkPrompt.activeSelf) {
			talkPrompt.SetActive (false);
		}
	}
}
