using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CheckLevelUnlocked : MonoBehaviour {

	public string file = "Assets/Save.txt";
	string desiredVal = "0";
	public float LockedColor = 12;
	// Use this for initialization
	void Start () {
		if(desiredVal == readTextFile(file))
		{
			Color clr = new Color();
			clr.r = LockedColor;
			clr.b = LockedColor;
			clr.g = LockedColor;
			clr.a = LockedColor;
			GetComponent<Image>().color = clr;
			GetComponent<Button>().interactable = false;
		}
	}

	string readTextFile(string file_path)
	{
		StreamReader inp_stm = new StreamReader(file_path);
		
		while(!inp_stm.EndOfStream)
		{
			string inp_ln = inp_stm.ReadLine( );
			return inp_ln;
		}
		
		inp_stm.Close( );  
		return "ayy lmao";
	}

	// Update is called once per frame
	void Update () {
	
	}


}
