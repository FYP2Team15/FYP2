﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	
	public GameObject Blood;
	public int[] weaknesses;
	public int[] strengths;
	public float Armor;
	public float CurArmor = 100.0f;
	public int charCount = 1;
	[HideInInspector]public bool displayStat = false;
	private bool displayHp = false;
	public float healthBarL;
	public bool hasAudio = false;
	public int nameLength = 5;
	
	GameObject audio = new GameObject();
	public AudioClip translateSound;
	
	//public Slider HpSlider;
	
	public float hp;
	public GameObject HPbar;
	float hpscale;
	bool Multiplayer = false;
	PlayerAttack PA = null;
	// Use this for initialization
	void Start () {
		audio = GameObject.Find ("Sfx");
		if (audio == null)
			hasAudio = false;
		healthBarL = Screen.width / 2;
		//hp = this.CurArmor;
		//HpSlider.value = this.CurArmor;
		if(GameObject.Find("Multiplayer"))
			Multiplayer = true;
	}
	
	void OnDestroy (){
		//Instantiate (Blood, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		//	hpscale = hp / this.Armor;
		//	HPbar.transform.localScale = new Vector3 (hpscale, HPbar.transform.localScale.y, HPbar.transform.localScale.z);
		//HPbar.transform.localScale = new Vector3 (hpscale, HPbar.transform.localScale.y);
		
		//HpSlider.value = this.CurArmor;
		
		if (this.CurArmor == 0 || this.CurArmor < 0) {
			GameObject.Find("BattleCamera").GetComponent<Camera>().enabled = false;
			GameObject.Find("Camera").GetComponent<Camera>().enabled = true;

			Instantiate (Blood, transform.position, Quaternion.identity);
			Destroy(this.gameObject);
			
			if(hasAudio)
				audio.GetComponent<AudioScript>().playOnceCustom(translateSound);
			//PA.animationOn = true;
		}
	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseEnter () {
		//displayStat = true;
		//Debug.Log (this.name);
	}
	void OnMouseExit () {
		//displayStat = false;
		//Debug.Log (this.name);
	}
	void OnMouseClick(){
		displayHp = true;
	}
	void OnMouseUp(){
		displayHp = false;
	}
	#endif	

	float ScaleToHeight(float value)
	{
		float newValue = value / 768;
		return (float)((value / 768) * Screen.height);
	}
	
	float ScaleToWidth(float value)
	{
		float newValue = value / 1024;
		return (float)((value / 1024) * Screen.width);
	}

	void OnGUI(){

		if(displayStat)
		{
			Rect rect = new Rect(ScaleToWidth(700),0,ScaleToWidth(300),70);
			GUI.contentColor = Color.white;
			
			Rect rect2 = rect;
			rect2.y = rect.y + 15;
			Rect rect3 = rect2;
			rect3.y = rect2.y + 15;
			Rect rect4 = rect3;
			rect4.y = rect3.y + 15;

			GUI.TextField(rect, this.name.Substring (0, nameLength)); // display its name, or other string
			GUI.Label (rect3, "Health:  " + (CurArmor*charCount).ToString() + "/" + (Armor*charCount).ToString());	
			GUI.Label (rect4, "Char count: " + charCount.ToString());
			//GUI.Box (new Rect (10, 500, this.healthBarL, 20), this.CurArmor + "/" + this.Armor);
		}
		if(displayHp)
		{
			GUI.Box (new Rect (10, 500, this.healthBarL, 20), this.CurArmor + "/" + this.Armor);
		}

	}

	public void PLHealthChange(float a)
	{
		GameObject blood = Instantiate (Blood, transform.position+new Vector3(0,0.7f,0), Quaternion.identity) as GameObject;
		blood.transform.SetParent(transform);
		this.CurArmor -= a;
		//HpSlider.value = this.CurArmor;

		Debug.Log ("Player got minus");
		if(Multiplayer)
		{
			NetworkView nv = GetComponent<NetworkView>();
			nv.RPC("MHealthChange", RPCMode.Others,a);
		}
		//}
	}
	[RPC]
	public void MPLHealthChange(float a)
	{
		GameObject blood = Instantiate (Blood, transform.position, Quaternion.identity) as GameObject;
		blood.transform.SetParent(transform);
		this.CurArmor -= a;
		//HpSlider.value = this.CurArmor;
		Debug.Log("Multiplayer Player got minus");
		//}
	}
}
