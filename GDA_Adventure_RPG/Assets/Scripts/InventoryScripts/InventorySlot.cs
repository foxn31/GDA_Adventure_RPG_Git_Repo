using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler {

	public Image icon;
	public Button removeButton;

	private Item item;
    private Inventory inventory;
    private int slot;

	void Awake() {
		icon.enabled = false;
        removeButton.interactable = false;
        removeButton.onClick.AddListener(RemoveItem);
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PickupItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
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

	public void RemoveItem() {
        inventory.Remove(slot);
	}

    public void PickupItem()
    {
        InventorySystem.ItemOnCursor = inventory.Swap(InventorySystem.ItemOnCursor, slot);
    }

	public void UseItem() {
		if (item != null) {
			item.Use ();
		}
	}

    
}