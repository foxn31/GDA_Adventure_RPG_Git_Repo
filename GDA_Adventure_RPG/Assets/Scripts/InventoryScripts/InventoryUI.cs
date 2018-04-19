using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public GameObject slotPrefab;

    public Transform background;
    public Transform slotContainer;

    public delegate void InventoryActionCallback(Item item, Inventory inventory, int slot);
    public event InventoryActionCallback ItemLeftClick;
    public event InventoryActionCallback ItemRightClick;

    void OnItemLeftClick(Item item, Inventory inventory, int slot)
    {
        if (ItemLeftClick != null)
        {
            ItemLeftClick(item, inventory, slot);
        }
    }
    void OnItemRightClick(Item item, Inventory inventory, int slot)
    {
        if (ItemRightClick != null)
        {
            ItemRightClick(item, inventory, slot);
        }
    }

    private Inventory _inventory;
    private int _numColumns = -1;

    public int NumColumns
    {
        get { return _numColumns; }
        set { _numColumns = value; UpdateUISize(); }
    }
    public Inventory Inventory
    {
        get { return _inventory; }
        set
        {
            InventoryUnsubscribe();
            _inventory = value;
            InventorySubscribe();
            UpdateInventory();
        }
    }

    // Subscribe/Unsubscribe to InventoryItemChanged events
    private void InventorySubscribe()
    {
        if (_inventory != null) _inventory.InventoryItemChanged += this.OnInventoryItemChanged;
    }
    private void InventoryUnsubscribe()
    {
        if (_inventory != null) _inventory.InventoryItemChanged -= this.OnInventoryItemChanged;
    }

    public void OnInventoryItemChanged(int slot, Item oldItem, Item newItem)
    {
        slotContainer.GetChild(slot).GetComponent<InventorySlot>().SetItem(newItem);
    }

    // Rebuilds the UI to address any changes to the bound inventory object or the
    // size (number of columns) of the UI
    private void UpdateUISize()
    {
        RectTransform rect = GetComponent<RectTransform>();
        GridLayoutGroup layout = slotContainer.GetComponent<GridLayoutGroup>();

        int numRows = (int)Mathf.Ceil((float)_inventory.Size / _numColumns);

        float width = (layout.cellSize.x + layout.spacing.x) * _numColumns;
        float height = (layout.cellSize.y + layout.spacing.y) * numRows;

        rect.sizeDelta = new Vector2(width, height);
    }

    // Handles fixing the structure of the UI if the inventory object changes
    private void UpdateInventory()
    {
        int oldSize = slotContainer.childCount;

        // Grow or shrink the number of inventory slots in the UI to match the number in the
        // inventory object
        if (oldSize < _inventory.Size)
        {
            for (int i = 0; i < _inventory.Size - oldSize; i++)
            {
                GameObject slot = Instantiate(slotPrefab);
                slot.transform.SetParent(slotContainer, false);

                // Propagate events from the slot to this entire inventory UI so that scripts
                // can control left/right click behavior
                InventorySlot s = slot.GetComponent<InventorySlot>();
                s.ItemLeftClick += OnItemLeftClick;
                s.ItemRightClick += OnItemRightClick;
            }
        }
        else if (oldSize > _inventory.Size)
        {
            for (int i = 0; i < oldSize - _inventory.Size; i++)
            {
                Destroy(slotContainer.GetChild(0));
            }
        }

        // Set the slot buttons to display the correct items
        for (int i = 0; i < _inventory.Size; i++)
        {
            InventorySlot slot = slotContainer.GetChild(i).GetComponent<InventorySlot>();
            slot.Bind(_inventory, i);
            slot.SetItem(_inventory[i]);
        }

        // The UI should probably be a different size now
        UpdateUISize();
    }

    void Start()
    {
        if (_inventory == null) _inventory = new Inventory(20);
        if (_numColumns <= 0) _numColumns = 5;

        InventorySubscribe();

        Debug.Log(slotContainer);

        UpdateInventory();
    }

    void OnDestroy()
    {
        InventoryUnsubscribe();
    }

}
