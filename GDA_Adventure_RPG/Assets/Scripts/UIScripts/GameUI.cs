using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameUI : MonoBehaviour {

	public GameObject talkPrompt;
	public GameObject pickupPrompt;
	public GameObject inventory;

    public static GameUI instance;

    private HashSet<GameObject> openUIElements = new HashSet<GameObject>();
    private ThirdPersonPlayerCamera playerCamera;

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

        playerCamera = FindObjectOfType<ThirdPersonPlayerCamera>();
    }

    public void ToggleUIElement(GameObject obj)
    {
        if (obj.activeSelf)
        {
            HideUIElement(obj);
        }
        else
        {
            ShowUIElement(obj);
        }
    }

    public void ShowUIElement(GameObject obj)
    {
        if (openUIElements.Count == 0)
        {
            // This is the first element to be opened
            ShowCursor();
            playerCamera.DisableCamRot();
        }

        openUIElements.Add(obj);
        obj.SetActive(true);
    }

    public void HideUIElement(GameObject obj)
    {
        openUIElements.Remove(obj);
        obj.SetActive(false);

        if (openUIElements.Count == 0)
        {
            // All elements are now closed
            HideCursor();
            playerCamera.EnableCamRot();
        }
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