using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour {

	public GameObject startMenu;
	public GameObject options;
	public GameObject controls;
	public AudioSource startMenuAudio;

	void Start()
	{
		startMenu.SetActive (true);
		options.SetActive (false);
		controls.SetActive (false);
	}
	// Main Menu Choices
	public void Play() {
		SceneManager.LoadScene ("Prototype Scene");
	}
	public void Quit() {
		Application.Quit();
	}
	public void Options() {
		startMenu.SetActive (false);
		options.SetActive (true);
	}

	// Options choices
	public void OptionsToStartMenu()
	{
		startMenu.SetActive (true);
		options.SetActive (false);
	}
	public void OptionsToControls()
	{
		options.SetActive (false);
		controls.SetActive (true);
	}

	// Controls choices
	public void ControlsToOptions()
	{
		controls.SetActive (false);
		options.SetActive (true);
	}
}
