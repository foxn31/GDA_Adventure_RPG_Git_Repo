using UnityEngine;

public class ItemPickup: Interactable {

	public static event System.Action ShowPickupPrompt;
	public static event System.Action HidePickupPrompt;

	public bool isHDItem = false;

	bool promptVisible = true;

	public Item item;

    //Added audio objects to play the pickup sound effect
    public AudioClip[] pickupSounds;
    private AudioSource audioSource;

	void Start () {
		if (isHDItem) {
			transform.Rotate ( 0, 0, 30);
		}
		player = GameObject.FindGameObjectWithTag("Player").transform;

        //Assign the AudioSource component of player's third child (at index 2 - PlayerSoundFXPlayer) to audioSource
        audioSource = player.Find("PlayerSoundFXPlayer").GetComponent<AudioSource>();
	}

	public override void Update () {
		base.Update ();

		if(isHDItem) {
			animateHDItem ();
		}
	}

	void LateUpdate() {
		if (playerIsInRange()) {
			showPrompt ();
		} else {
			hidePrompt ();
		}
	}

	public override void Interact() {
		base.Interact ();
		PickUp ();
	}

	void PickUp() {

		Debug.Log ("Picking up " + item.name);

		hidePrompt ();

        if (InventorySystem.playerInventory.Add(item))
        {
            PlayRandomPickupSound();
            Destroy(gameObject);
        }

	}

	void showPrompt() {
		if (!promptVisible && ShowPickupPrompt != null) {
			ShowPickupPrompt ();
			promptVisible = true;
		}
	}

	void hidePrompt() {
		if (promptVisible && HidePickupPrompt != null) {
			HidePickupPrompt ();
			promptVisible = false;
		}
	}

	void animateHDItem () {
		transform.Rotate (Vector3.up);
	}

    /**
     * Plays one of two random pickup sound effects
     */
    void PlayRandomPickupSound()
    {
        if (pickupSounds.Length > 0)
        {
            int soundChoice = Random.Range(0, pickupSounds.Length);
            audioSource.PlayOneShot(pickupSounds[soundChoice]);
        }
    }
}