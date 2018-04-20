using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour {

	public Slider volumeSlider;
	public AudioSource masterAudio;
	public PlayerController pc;

	public GameObject advancedTab;

	void Start() 
	{
		// Option initially hidden
		gameObject.SetActive (false);

		// Volume is 50% as default
		volumeSlider.value = .5f;
	}

	void Update() 
	{
		SetVolume ();		
	}

	public void SetVolume() {
		masterAudio.volume = volumeSlider.value;
	}

	public void AdvancedMenu()
	{
		gameObject.SetActive (false);
		gameObject.SetActive (advancedTab);
	}
}
