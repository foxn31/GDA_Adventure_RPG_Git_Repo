using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject talkPrompt;
	public GameObject pickupPrompt;


	void Start () {
		//NPCScript.ShowTalkPrompt += showTalkPrompt;
		//NPCScript.HideTalkPrompt += hideTalkPrompt;
		DialogueNPC.ShowTalkPrompt += showTalkPrompt;
		DialogueNPC.HideTalkPrompt += hideTalkPrompt;
		Item.ShowPickupPrompt += showPickupPrompt;
		Item.HidePickupPrompt += hidePickupPrompt;

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

	void showPickupPrompt() {
		if (!pickupPrompt.activeSelf) {
			pickupPrompt.SetActive (true);
		}
	}

	void hidePickupPrompt() {
		if (pickupPrompt.activeSelf) {
			pickupPrompt.SetActive (false);
		}
	}
}
