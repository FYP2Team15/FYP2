using UnityEngine;
using System.Collections;

public class Fade_Screen : MonoBehaviour {

	public Texture2D fadeTexture;
	public float FadeSpeed = 0.0f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;
	public GameObject GameStart;
	bool firstTime = true;

	void Start () {

	}
	void OnGUI(){

		alpha += fadeDir * FadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	public float BeginFade(int direction){
				fadeDir = direction;
				return (FadeSpeed);
		}

	void OnLevelWasLoaded(){
				BeginFade (-1);
		}

	void Update(){
		if(alpha <= 0 && GameStart != null && firstTime)
		{
			GameStart.GetComponent<GameStart>().PanToVictory();
			firstTime = false;
		}
	}
}
