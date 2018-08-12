using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

	public EquipmentSlot equipSlot;
	public SkinnedMeshRenderer mesh;
	public EquipmentMeshRegion[] coveredMeshRegions;
    public WeaponType weaponType;

	public int armorModifier;
	public int damageModifier;

	public override void Use() {
        base.Use();
        RemoveFromInventory();
		EquipmentManager.instance.Equip(this);
	}


}

public enum EquipmentSlot{ Head, Chest, Arms, Legs, Feet, RightHand, LeftHand};
public enum EquipmentMeshRegion{ UpperTorso, LowerTorso, UpperLegs, LowerLegs, UpperArms, LowerArms, Hands, Feet, Head };
public enum WeaponType { None, OneHandedSword, TwoHandedSword};