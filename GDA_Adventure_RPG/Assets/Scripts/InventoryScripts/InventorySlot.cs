using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public Image icon;
	public Button removeButton;

	private Item item;
    private Inventory inventory;
    private int slot;

	void Awake() {
		icon.enabled = false;
        removeButton.interactable = false;
	}

    // Should be called whenever the slot is created or when the inventory it is part of changes
    public void Bind(Inventory inventory, int slot)
    {
        this.inventory = inventory;
        this.slot = slot;
    }

    public void SetItem(Item newItem)
    {
        item = newItem;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            removeButton.interactable = true;
        }
        else
        {
            icon.enabled = false;
            removeButton.interactable = false;
        }
    }

	public void OnRemoveButton() {
        inventory.Remove(slot);

		Debug.Log ("Removing" + item);
	}

	public void UseItem() {
		if (item != null) {
			item.Use ();
		}
	}
}