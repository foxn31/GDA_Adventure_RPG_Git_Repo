using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    private static GameObject inventoryUIPrefab;

    // Creates a new InventoryUI for the given inventory
    public static InventoryUI CreateInventoryUI(Inventory inventory, int numColumns)
    {
        if (inventoryUIPrefab == null)
        {
            inventoryUIPrefab = (GameObject)Resources.Load("Inventory/InventoryUI.prefab");
        }

        GameObject obj = Instantiate(inventoryUIPrefab);

        InventoryUI invUI = obj.GetComponent<InventoryUI>();
        invUI.Inventory = inventory;
        invUI.NumColumns = numColumns;

        return invUI;
    }



    public GameObject slotPrefab;

    public Transform background;
    public Transform slotContainer;

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
            _inventory = value;
            if (_inventory != null) _inventory.Watch(this.OnInventoryItemChanged);
            UpdateInventory();
        }
    }

    public void OnInventoryItemChanged(int slot, Item oldItem, Item newItem)
    {
        Debug.Log("OnInventoryItemChanged " + slot);
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

        _inventory.Watch(this.OnInventoryItemChanged);

        Debug.Log(slotContainer);

        UpdateInventory();
    }

    /*
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
		if (Input.GetKeyDown (KeyCode.T) && inventoryUI.activeSelf && HideCursor != null) {
			inventoryUI.SetActive (false);
			HideCursor ();
			Debug.Log ("inventory hidden");
			FindObjectOfType<ThirdPersonPlayerCamera> ().EnableCamRot ();
		} else if (Input.GetKeyDown (KeyCode.T) && !inventoryUI.activeSelf && ShowCursor != null) {
			inventoryUI.SetActive (true);
			ShowCursor ();
			Debug.Log ("inventory visible");
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

    */

}
