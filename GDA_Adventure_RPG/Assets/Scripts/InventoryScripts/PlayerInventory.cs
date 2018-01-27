using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public static Inventory inventory = new Inventory(8 * 5);

    public static event System.Action ShowCursor;
    public static event System.Action HideCursor;

    public InventoryUI ui;

    // Use this for initialization
    void Start () {
        ui.Inventory = inventory;
        ui.NumColumns = 8;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (ui.gameObject.activeSelf)
            {
                ui.gameObject.SetActive(false);
                HideCursor();
                Debug.Log("Inventory Hidden");
                FindObjectOfType<ThirdPersonPlayerCamera>().EnableCamRot();
            }
            else
            {
                ui.gameObject.SetActive(true);
                ShowCursor();
                Debug.Log("Inventory Visible");
                FindObjectOfType<ThirdPersonPlayerCamera>().DisableCamRot();
            }
        }
    }
}
