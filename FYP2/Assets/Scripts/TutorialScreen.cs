using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour {

	public GameObject TutorialExitButton;
	public GameObject LevelScreenExitButton;
	
	void Start () 
	{
	}

	void Update () {
	}

	public void EnableTutorial() {
		LevelScreenExitButton.SetActive (false);
		TutorialExitButton.SetActive (true);
		GetComponent<Image> ().enabled = true;
	}

	public void DisableTutorial() {
		LevelScreenExitButton.SetActive (true);
		TutorialExitButton.SetActive (false);
		GetComponent<Image> ().enabled = false;
	}

	
	public void loadMenu()
	{
		GetComponent<AudioSource> ().Play ();
		Application.LoadLevel (0);
	}

}
