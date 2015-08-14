using UnityEngine;
using System.Collections;

public class UIBarInput : MonoBehaviour {

	public string SinglePlayer;
	public string MultiPlayer;
	public string Credits;
	public string Settings;
	public string option;
	public string Level1;
	public string Level2;
	public string Level3;
	public string Level4;
	public string Level5;
	public string LevelTutorial;
	string Sc;



	void Start(){
		//this.Button_Select = Buttontype.BUTTON_CHANGE;
	}

	IEnumerator ChangeLevel(string Sc){
		float fadeTime = GameObject.Find ("Screen_Fade").GetComponent<Fade_Screen>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Debug.Log("Seconds IN");
		Application.LoadLevel(Sc);

	}

	void Update()
	{

	}

	public void SinglePlayerLevelChange(){
		Sc = SinglePlayer;
		StartCoroutine (ChangeLevel(Sc));
	}

	public void MultiPlayerLevelChange(){
		Sc = MultiPlayer;
		StartCoroutine (ChangeLevel (Sc));
	}

	public void OptionLevelChange(){
		Sc = option;
		StartCoroutine (ChangeLevel (Sc));
	}
	public void LevelSelect1(){
		Sc = Level1;
		StartCoroutine (ChangeLevel (Sc));
	}
	
	public void LevelSelect2(){
		Sc = Level2;
		StartCoroutine (ChangeLevel (Sc));
	}
	
	public void LevelSelect3(){
		Sc = Level3;
		StartCoroutine (ChangeLevel (Sc));
	}
	
	public void LevelSelect4(){
		Sc = Level4;
		StartCoroutine (ChangeLevel (Sc));
	}
	
	public void LevelSelect5(){
		Sc = Level5;
		StartCoroutine (ChangeLevel (Sc));
	}

	public void MainPage(){
		Sc = "MainPage";
		StartCoroutine (ChangeLevel (Sc));
	}
}
