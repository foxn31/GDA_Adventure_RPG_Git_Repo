using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour {

	public GameObject mainMenu;
	public AudioSource StartMenuAudio;


	public void Play() {
		SceneManager.LoadScene ("Prototype Scene");
	}

	public void Quit() {
		Application.Quit();
	}

	public void MainMenu() {
		mainMenu.SetActive (true);
	}

	public void OptionsMenu() {
		mainMenu.SetActive (false);
	}
}
