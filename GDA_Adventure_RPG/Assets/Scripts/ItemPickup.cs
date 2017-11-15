using UnityEngine;

public class ItemPickup: Interactable {

	public static event System.Action ShowPickupPrompt;
	public static event System.Action HidePickupPrompt;

	public bool isHDItem = false;

	bool promptVisible = true;

	public Item item;

    //Two different sound effects for picking up the item, and an audiosource through which to play them
    public AudioClip pickUpSound1;
    public AudioClip pickUpSound2;
    private AudioSource audioSource;

	void Start () {
		if (isHDItem) {
			transform.Rotate ( 0, 0, 30);
		}

        //Find the player and assign the player's PlayerSoundFXPlayer (index 2 of player's children) to audioSource
		player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = player.GetChild(2).GetComponent<AudioSource>();
	}

	public override void Interact() {
		base.Interact ();
		PickUp ();
	}

	void PickUp() {

		Debug.Log ("Picking up " + item.name);

		hidePrompt ();

		bool wasPickedUp = Inventory.instance.Add (item);

		if (wasPickedUp) {
            PlayRandomPickupSound();
            Destroy (gameObject);
		}
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

    //Plays one of two pickup sound effects, chosen at random
    void PlayRandomPickupSound()
    {
        int soundPicked = (int)Random.Range(0, 2);
        switch (soundPicked)
        {
            case 0:
                audioSource.PlayOneShot(pickUpSound1);
                break;
            case 1:
                audioSource.PlayOneShot(pickUpSound2);
                break;
        }
    }
}