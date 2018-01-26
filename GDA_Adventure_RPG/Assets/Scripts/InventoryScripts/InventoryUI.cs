using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform itemsParent;
	public GameObject inventoryUI;

	public static event System.Action ShowCursor;
	public static event System.Action HideCursor;

	Inventory inventory;

	InventorySlot[] slots;

	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;

		slots = itemsParent.GetComponentsInChildren<InventorySlot> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.I) && inventoryUI.activeSelf && HideCursor != null) {
			inventoryUI.SetActive (false);
			HideCursor ();
			Debug.Log ("Inventory hidden");
			FindObjectOfType<ThirdPersonPlayerCamera> ().EnableCamRot ();
		} else if (Input.GetKeyDown (KeyCode.I) && !inventoryUI.activeSelf && ShowCursor != null) {
			inventoryUI.SetActive (true);
			ShowCursor ();
			Debug.Log ("Inventory visible");
			FindObjectOfType<ThirdPersonPlayerCamera> ().DisableCamRot ();
		}
	}

	void UpdateUI() {

		Debug.Log ("Updating UI");

		for (int i = 0; i < slots.Length; i++) {
			
			if (i < inventory.items.Count) {
				
				slots [i].AddItem (inventory.items [i]);

			} else {
				
				slots [i].ClearSlot ();

			}

		}

	}

}
