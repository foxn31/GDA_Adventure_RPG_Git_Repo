using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    string[] items = new string[0];
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            showInventory();
        }
    }

    public void addItem(string newItem)
    {
        string[] newItems = new string[items.Length + 1];

        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }

        newItems[newItems.Length - 1] = newItem;
        items = newItems;
    }

    void showInventory()
    {
        if (items.Length == 0)
        {
            Debug.Log("You have no items.");
            return;
        }

        string itemString = "";

        for (int i = 0; i < items.Length - 1; i++)
        {
            itemString += items[i] + " ";
        }

        itemString += items[items.Length - 1];

        Debug.Log("You have: " + itemString);
    }
}
