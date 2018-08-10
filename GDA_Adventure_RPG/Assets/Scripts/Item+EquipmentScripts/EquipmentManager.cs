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

	public Equipment[] defaultItems;
	Equipment[] currentEquipment;
	SkinnedMeshRenderer[] currentMeshes;

	Inventory inventory;

	public SkinnedMeshRenderer targetMesh;

    public int weaponTypeInt = 0;

	void Start () {
		inventory = InventorySystem.playerInventory;

		int numSlots = System.Enum.GetNames (typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
		currentMeshes = new SkinnedMeshRenderer[numSlots];

		EquipDefaultItems();
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

		Equipment equippedItem = Unequip (slotIndex);

		if (onEquipmentChanged != null) {
			onEquipmentChanged.Invoke (newItem, equippedItem);
		}

        if ( (int)newItem.weaponType == 2) {
            Unequip (6);
        }

		SetEquipmentBlendShapes (newItem, 100);

		currentEquipment [slotIndex] = newItem;
		SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer> (newItem.mesh);
		newMesh.transform.parent = targetMesh.transform;

		newMesh.bones = targetMesh.bones;
		newMesh.rootBone = targetMesh.rootBone;
		currentMeshes [slotIndex] = newMesh;

        weaponTypeInt = (int)newItem.weaponType;
	}

	public Equipment Unequip (int slotIndex) {
		if (currentEquipment [slotIndex] != null) {

			if (currentEquipment [slotIndex] != null) {
				Destroy(currentMeshes[slotIndex].gameObject);
			}

			Equipment equippedItem = currentEquipment [slotIndex];
			SetEquipmentBlendShapes (equippedItem, 0);
			inventory.Add (equippedItem);

			currentEquipment [slotIndex] = null;

			if (onEquipmentChanged != null) {
				onEquipmentChanged.Invoke (null, equippedItem);
			}

            weaponTypeInt = 0;

			return equippedItem;
		}
		return null;
	}

	public void UnequipAll () {
		for (int i = 0; i < currentEquipment.Length; i++) {
			Unequip (i);
		}
		EquipDefaultItems();
	}

	void EquipDefaultItems() {
		foreach (Equipment item in defaultItems) {
			Equip (item);
		}
	}

	void SetEquipmentBlendShapes(Equipment item, int weight) {
		foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) {
			targetMesh.SetBlendShapeWeight ((int)blendShape, weight);
			Debug.Log ("setting blend shapes");
		}
	}

}
