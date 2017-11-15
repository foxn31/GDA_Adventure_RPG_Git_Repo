using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject talkPrompt;
	public GameObject pickupPrompt;
	public GameObject inventory;

	void Start () {
		DialogueNPC.ShowTalkPrompt += showTalkPrompt;
		DialogueNPC.HideTalkPrompt += hideTalkPrompt;
		ItemPickup.ShowPickupPrompt += showPickupPrompt;
		ItemPickup.HidePickupPrompt += hidePickupPrompt;
		PlayerController.ShowInventory += showInventory;
		PlayerController.HideInventory += hideInventory;
	}

	void showInventory() {
		if (!inventory.activeSelf) {
			inventory.SetActive (true);
		}
	}

	void hideInventory() {
		if (inventory.activeSelf) {
			inventory.SetActive (false);
		}
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