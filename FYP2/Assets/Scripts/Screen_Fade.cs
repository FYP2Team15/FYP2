using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Screen_Fade : MonoBehaviour {

	public float fadeSpeed = 1.5f;
	float FAlpha = 1.0f;
	//private bool sceneStart = true;
	public bool Faded = false;

	public Canvas FadePrefab;
	Canvas Fade;

	public enum FadeType
	{
		FADE_IN,
		FADE_OUT
	}public FadeType Ftype;
	
	protected static Screen_Fade mInstance;
	public static Screen_Fade Instance
	{
		get
		{
			if (mInstance == null)
			{
				GameObject tempObj = new GameObject();
				mInstance = tempObj.AddComponent<Screen_Fade>();
				Destroy(tempObj);
			}
			return mInstance;
		}
	}

	void PlaceFade(){
		if (FadePrefab != null) {
			Fade = Instantiate(FadePrefab, FadePrefab.transform.position, Quaternion.identity) as Canvas;
		}
	}
	// Use this for initialization
	void Start () {
	
		Faded = false;
		PlaceFade ();
	}

	public void StartFade(bool FadeMode){
		if (FadeMode) {
		
			if(Fade.GetComponentInChildren<Image>().color.a > 0.0f)
			{
				FAlpha -= Time.deltaTime * fadeSpeed;
			}else
			{
				Faded = true;
				Destroy(Fade.gameObject);
			}
		}
	}

	public void ResetFade(bool Mode)
	{
		//Reset Flag
		Faded = false;
		
		//Instantiate the destroyed Fade Object
		if (Fade == null)
			PlaceFade();
		
		//Fade-In
		if (Mode)
		{
			this.Ftype = FadeType.FADE_IN;
			Fade.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
		else
		{
			this.Ftype = FadeType.FADE_OUT;
			Fade.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
	}
	// Update is called once per frame
	void Update () {
	

		if(Fade != null)
		{
			switch(Ftype)
			{
				case FadeType.FADE_IN:
					StartFade(true);
					break;
				case FadeType.FADE_OUT:
					StartFade(false);
					break;
			}
			Fade.GetComponentInChildren<Image>().color = new Color(1.0f,1.0f,1.0f,FAlpha);
		}

	}
}
