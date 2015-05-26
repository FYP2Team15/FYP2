using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	public float Damage = 10;
	public int type = 0;
	public int[] weaknesses;
	public int[] strengths;
	public float Armor = 10;
	public int charCount = 1;
	private bool displayStat = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Input.mousePosition.y);
	}
	void OnMouseEnter () {
		displayStat = true;
		//Debug.Log (this.name);
	}
	void OnMouseExit () {
		displayStat = false;
		//Debug.Log (this.name);
	}
	void OnGUI () {
		if(displayStat)
		{
			GUI.contentColor = Color.blue;
			GUI.Label (new Rect (Input.mousePosition.x+20, -Input.mousePosition.y+320, 100, 100), "Total Damage " + (Damage*charCount).ToString());
			GUI.Label (new Rect (Input.mousePosition.x+20, -Input.mousePosition.y+330, 100, 100), "Type " + (type).ToString());
			GUI.Label (new Rect (Input.mousePosition.x+20, -Input.mousePosition.y+340, 100, 100), "Total Defence " + (Armor*charCount).ToString());	
			GUI.Label (new Rect (Input.mousePosition.x+20, -Input.mousePosition.y+350, 100, 100), "Char count" + charCount.ToString());
		}
		Rect rect = new Rect(0,0,300,100);
		Vector3 offset =  new Vector3(0,-Camera.main.transform.position.y/1.5f,0); // height above the target position

		var point = Camera.main.WorldToScreenPoint(this.transform.position+offset);
		rect.x = point.x;
		rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
		GUI.contentColor = Color.blue;
		Rect rect2 = rect;
		rect2.y = rect.y + 10;
		GUI.Label(rect2, this.name); // display its name, or other string
		GUI.Label(rect, "C " + charCount.ToString()); // display its name, or other string
	}
	public void Attack(GameObject target) {
		Stats t = target.GetComponent<Stats>();
		float DmgDealt = Damage * charCount;
		foreach (int weakness in weaknesses) {
			if (weakness == type)
				DmgDealt = DmgDealt * 2;
		}
		foreach (int strength in strengths) {
			if (strength == type)
				DmgDealt = DmgDealt / 2;
		}
		if (t.Armor < DmgDealt)
			Destroy (target);
		else
			Destroy (this.gameObject);
	}
}
