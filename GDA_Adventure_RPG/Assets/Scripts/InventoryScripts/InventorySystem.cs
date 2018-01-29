using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static event System.Action ShowCursor;
    public static event System.Action HideCursor;

    public InventoryUI playerInventoryUI;
    public Image cursorImage;

    // Use this for initialization
    void Start () {
        instance = this;

        playerInventoryUI.Inventory = playerInventory;
        playerInventoryUI.NumColumns = 8;
	}
	
	// Update is called once per frame
	void Update () {
        cursorImage.transform.position = Input.mousePosition + new Vector3(30, -30, 0);

        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.I))
        {
            if (playerInventoryUI.gameObject.activeSelf)
            {
                playerInventoryUI.gameObject.SetActive(false);
                HideCursor();
                Debug.Log("Inventory Hidden");
                FindObjectOfType<ThirdPersonPlayerCamera>().EnableCamRot();

                // if the user closes their inventory while moving an item, remove the item from
                // the cursor and add it to their inventory
                if (ItemOnCursor != null)
                {
                    playerInventory.Add(ItemOnCursor);
                    ItemOnCursor = null;
                }
            }
            else
            {
                playerInventoryUI.gameObject.SetActive(true);
                ShowCursor();
                Debug.Log("Inventory Visible");
                FindObjectOfType<ThirdPersonPlayerCamera>().DisableCamRot();
            }
        }
    }
}
