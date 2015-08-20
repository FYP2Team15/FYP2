using UnityEngine;
using System.Collections;

public class PauseButton_InGame : MonoBehaviour {

	public GameObject PauseButton;
	public GameObject PauseMenu;

	public bool OnPaused;
	//public bool PausedIn;
	public GameObject SF;
	public string SceneQ;
	string Sc;
	bool StartQuit;

	// Use this for initialization
	void Start () {
		//PauseMenu.SetActive (false);
		OnPaused = false;
		//PausedIn = false;
		}

	IEnumerator ChangeLevel(string Sc){
		//float fadeTime = GameObject.Find ("Screen_Fade").GetComponent<Fade_Screen>().BeginFade(1);
		float fadeTime = SF.GetComponent<Fade_Screen> ().BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
//		yield return new WaitForSeconds(1);
		//Debug.Log("Seconds IN");
		Application.LoadLevel(Sc);

	}
	
	// Update is called once per frame
	void Update () {
		//PauseMenu.SetActive (OnPaused);
		//if (OnPaused == true) {
		//	PausedIn = true;
		//}		
	}

	public void OnPause(){
		OnPaused = !OnPaused;
		PauseMenu.SetActive (OnPaused);
	}

	public void OffPause(){
		OnPaused = false;
		PauseMenu.SetActive (false);
	}

	public void QuitPage(){
		Sc = "MainPage";
		OffPause();
		StartCoroutine (ChangeLevel (Sc));
	}

}
