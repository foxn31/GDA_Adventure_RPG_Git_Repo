using UnityEngine;

public class ItemPickup: Interactable {

	public static event System.Action ShowPickupPrompt;
	public static event System.Action HidePickupPrompt;

	public bool isHDItem = false;

	bool promptVisible = true;

	public Item item;

	void Start () {
		if (isHDItem) {
			transform.Rotate ( 0, 0, 30);
		}
		player = GameObject.FindGameObjectWithTag("Player").transform;

	}

	public override void Interact() {
		base.Interact ();
		PickUp ();
	}

	void PickUp() {

		Debug.Log ("Picking up " + item.name);

		hidePrompt ();

		bool wasPickedUp = Inventory.instance.Add (item);

		if (wasPickedUp) {
			Destroy (gameObject);
		}

	}

	public override void Update () {
		base.Update ();

		if(isHDItem) {
			animateHDItem ();
		}
	}

	void LateUpdate() {
		if (playerIsInRange()) {
			showPrompt ();
		} else {
			hidePrompt ();
		}
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

	void animateHDItem () {
		transform.Rotate (Vector3.up);
	}
}
