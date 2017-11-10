using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public static Inventory instance;

	void Awake() {
		if (instance != null) {
			Debug.LogWarning ("More than one instance of Inventory found");
		}

		instance = this;
	}

	public delegate void OnItemChanged ();
	public OnItemChanged onItemChangedCallback;

	public int invSpace = 20;

	public List<Item> items = new List<Item>();

	public bool Add (Item item) {

		if (!item.isDefaultItem) {
<<<<<<< HEAD

=======
			
>>>>>>> parent of bf1cde1... Added Inventory
			if (items.Count >= invSpace) {
				Debug.Log ("Not enough space in inventory");
				return false;
			}

			items.Add (item);

			if (onItemChangedCallback != null) {
				onItemChangedCallback.Invoke ();
			}
		}

		return true;
	}

	public void Remove (Item item) {
		items.Remove(item);
	}

<<<<<<< HEAD
}
=======
}
>>>>>>> parent of bf1cde1... Added Inventory
