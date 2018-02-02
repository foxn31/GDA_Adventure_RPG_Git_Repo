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

		InventoryUI.ShowCursor += showCursor;
		InventoryUI.HideCursor += hideCursor;
	}

	void  showCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void  hideCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void showInventory() {
		if (!inventory.activeSelf) {
			inventory.SetActive (true);
			//disable move
		}
	}

	void hideInventory() {
		if (inventory.activeSelf) {
			inventory.SetActive (false);
			//enable move
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