using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    void Start () {
		EquipmentManager.instance.onEquipmentChanged += onEquipmentChanged;
	}
	
	void onEquipmentChanged(Equipment oldItem, Equipment newItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);

            Debug.Log("PlayerStats has changed (added)");
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);

            Debug.Log("PlayerStats has changed (removed)");
        }
        
    }
}
