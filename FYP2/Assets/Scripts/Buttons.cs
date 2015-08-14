using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {

	public bool ButtSelect = false;

	public enum Buttontype
	{
		BUTTON_DEFAULT,
		BUTTON_START,
		BUTTON_MULTIPLAYER,
		BUTTON_SETTINGS,
		BUTTON_CREDITS,
		BUTTON_EXIT,
		BUTTON_PAUSE,
		BUTTON_CHANGE

	}public Buttontype Button_Select = Buttontype.BUTTON_DEFAULT;

	GameObject Butt = GameObject.FindWithTag("BUTTONS");


	// Use this for initialization
	void Start () {
	

	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	public void OnMouseDown(){

		if (Butt) {
			Debug.Log ("Input");
						switch (Button_Select) {
			
						case Buttontype.BUTTON_START:
								ButtSelect = true;
								break;
						case Buttontype.BUTTON_MULTIPLAYER:
								ButtSelect = true;
								break;


						}
				}


	}
	#endif

	#if UNITY_ANDROID
	void OnTouch(){
		
		if (Butt) {
			Debug.Log ("Input");
			switch (Button_Select) {
				
			case Buttontype.BUTTON_START:
				ButtSelect = true;
				break;
			case Buttontype.BUTTON_MULTIPLAYER:
				ButtSelect = true;
				break;
				
				
			}
		}
		
		
	}
	#endif

	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		OnMouseDown ();
		#endif
	}
}
