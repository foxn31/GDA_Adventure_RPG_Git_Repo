using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour {

    public static InventorySystem instance;

    public static Inventory playerInventory = new Inventory(8 * 5);

    private static Item _itemOnCursor;
    public static Item ItemOnCursor
    {
        get { return _itemOnCursor; }
        set
        {
            _itemOnCursor = value;
            if (value != null)
            {
                instance.cursorImage.enabled = true;
                instance.cursorImage.sprite = value.icon;
            }
            else
            {
                instance.cursorImage.enabled = false;
            }
        }
    }

    public InventoryUI playerInventoryUI;
    public Image cursorImage;
    public GameObject deleteConfirmDialog;
    public Button confirmDeleteButton;
    public Button cancelDeleteButton;

    // Use this for initialization
    void Start () {
        instance = this;

        confirmDeleteButton.onClick.AddListener(ConfirmDelete);
        cancelDeleteButton.onClick.AddListener(CancelDelete);

        playerInventoryUI.Inventory = playerInventory;
        playerInventoryUI.NumColumns = 8;

        playerInventoryUI.ItemLeftClick += DefaultInventoryLeftClickAction;
        playerInventoryUI.ItemRightClick += DefaultInventoryRightClickAction;
	}

    public static void DefaultInventoryLeftClickAction(Item item, Inventory inventory, int slot)
    {
        // Swap the item with the one on the cursor
        ItemOnCursor = inventory.Swap(ItemOnCursor, slot);
    }

    public static void DefaultInventoryRightClickAction(Item item, Inventory inventory, int slot)
    {
        // Use the item
        if (item != null)
        {
            item.Use();
        }
    }

    // Update is called once per frame
    void Update () {
        cursorImage.transform.position = Input.mousePosition + new Vector3(15, -30, 0);

        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.I))
        {
            GameUI.instance.ToggleUIElement(playerInventoryUI.gameObject);

            if (!playerInventoryUI.gameObject.activeSelf)
            {
                // if the user closes their inventory while moving an item, remove the item from
                // the cursor and add it to their inventory
                if (ItemOnCursor != null)
                {
                    playerInventory.Add(ItemOnCursor);
                    ItemOnCursor = null;
                }
            }
        }

        // Handle deleting items by dropping them out of the inventory
        if (Input.GetMouseButtonDown(0) && ItemOnCursor != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Activate the confirmation dialog
                // The dialog will either call ConfirmDelete or CancelDelete
                deleteConfirmDialog.SetActive(true);
            }
        }
    }

    void ConfirmDelete()
    {
        deleteConfirmDialog.SetActive(false);
        ItemOnCursor = null;
    }

    void CancelDelete()
    {
        deleteConfirmDialog.SetActive(false);
        playerInventory.Add(ItemOnCursor);
        ItemOnCursor = null;
    }
}
