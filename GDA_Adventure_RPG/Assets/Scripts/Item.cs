using UnityEngine;

public class Item : Interactable {

	public static event System.Action ShowPickupPrompt;
	public static event System.Action HidePickupPrompt;

	bool promptVisible = true;

	void Start () {
		
	}

	public override void Interact() {
		base.Interact ();
		PickUp ();
	}

	public override void Update () {
		base.Update ();
	}

	void LateUpdate() {
		if (playerIsInRange()) {
			showPrompt ();
		} else {
			hidePrompt ();
		}
	}

	void PickUp() {
		
		Debug.Log ("Picking up item");

		hidePrompt ();
		Destroy (gameObject);		

	}

	void showPrompt() {
		if (!promptVisible && ShowPickupPrompt != null) {
			ShowPickupPrompt ();
			promptVisible = true;
		}
	}

	void hidePrompt() {
		if (promptVisible && HidePickupPrompt != null) {
			HidePickupPrompt ();
			promptVisible = false;
		}
	}
}
