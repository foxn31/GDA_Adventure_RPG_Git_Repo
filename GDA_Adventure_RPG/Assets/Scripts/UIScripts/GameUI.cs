using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject talkPrompt;
	public GameObject pickupPrompt;
	public GameObject inventory;

    public static GameUI instance;

	void Start () {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

		DialogueNPC.ShowTalkPrompt += showTalkPrompt;
		DialogueNPC.HideTalkPrompt += hideTalkPrompt;

		ItemPickup.ShowPickupPrompt += showPickupPrompt;
		ItemPickup.HidePickupPrompt += hidePickupPrompt;
	}

	public void ShowCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void HideCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
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