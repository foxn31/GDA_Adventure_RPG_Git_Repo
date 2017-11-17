using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

	public static EquipmentManager instance;
	void Awake () {
		instance = this;
	}
		
	public delegate void OnEquipmentChanged (Equipment newItem, Equipment equippedItem);
	public OnEquipmentChanged onEquipmentChanged;

	Equipment[] currentEquipment;

	Inventory inventory;

	void Start () {
		inventory = Inventory.instance;

		int numSlots = System.Enum.GetNames (typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
	}

	void Update() {

		//code for checking input for unequipping items through a keyboard input
		//Should be temporary until unequipping through a UI is available...
		if (Input.GetKeyDown (KeyCode.U)) {
			UnequipAll ();
		}

	}
		
	public void Equip (Equipment newItem) {
		int slotIndex = (int)newItem.equipSlot;

		Equipment equippedItem = null;

		if (currentEquipment [slotIndex] != null) {
			equippedItem = currentEquipment [slotIndex];
			inventory.Add (equippedItem);
		}

		if (OnEquipmentChanged != null) {
			onEquipmentChanged.Invoke (newItem, equippedItem);
		}

		currentEquipment [slotIndex] = newItem;
	}

	public void Unequip (int slotIndex) {
		if (currentEquipment [slotIndex] != null) {
			Equipment equippedItem = currentEquipment [slotIndex];
			inventory.Add (equippedItem);

			currentEquipment [slotIndex] = null;

			if (OnEquipmentChanged != null) {
				onEquipmentChanged.Invoke (null, equippedItem);
			}
		}
	}

	public void UnequipAll () {
		for (int i = 0; i < currentEquipment.Length; i++) {
			Unequip (i);
		}
	}
}
