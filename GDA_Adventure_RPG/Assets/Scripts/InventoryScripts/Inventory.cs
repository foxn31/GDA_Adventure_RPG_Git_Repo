using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A wrapper over an array of items that provides common functions for
// manipulating items in an inventory as well as allowing other scripts
// (such as a UI script) to watch for changes
public class Inventory {

    // The contents of the inventory
    private Item[] slots;

    public int Size
    {
        get { return slots.Length; }
    }

    public delegate void OnInventoryItemChanged(int slot, Item oldItem, Item newItem);
    private OnInventoryItemChanged callback;

    public Inventory(int size)
    {
        this.slots = new Item[size];
    }

    // Watcher handling methods
    public void Watch(OnInventoryItemChanged cb)
    {
        callback = cb;
    }

    private void TriggerItemChangedCallback(int slot, Item oldItem, Item newItem)
    {
        callback(slot, oldItem, newItem);
    }

    // Getter methods
    public Item Get(int slot)
    {
        return slots[slot];
    }

    public Item this[int i]
    {
        get { return slots[i]; }
    }

    // Insertion methods
    public bool Add(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                // Found an empty slot; add the item
                Item oldItem = slots[i];
                slots[i] = item;
                TriggerItemChangedCallback(i, oldItem, item);
                return true;
            }
        }
        // Inventory is full; item was not added
        return false;
    }

    public Item Swap(Item item, int slot)
    {
        Item oldItem = slots[slot];
        slots[slot] = item;
        TriggerItemChangedCallback(slot, oldItem, item);
        return oldItem;
    }

    // Removal methods
    public bool Remove(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == item)
            {
                Item oldItem = slots[i];
                slots[i] = null;
                TriggerItemChangedCallback(i, oldItem, null);
                return true;
            }
        }
        return false;
    }

    public bool Remove(int slot)
    {
        if (slots[slot] != null)
        {
            Item oldItem = slots[slot];
            slots[slot] = null;
            TriggerItemChangedCallback(slot, oldItem, null);
            return true;
        }
        return false;
    }
}