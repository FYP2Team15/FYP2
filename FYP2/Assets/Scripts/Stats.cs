using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stats : MonoBehaviour {
	public float Damage = 1;
	public int type = 0;
	public int[] weaknesses;
	public int[] strengths;
	public float Armor = 100;
	public int charCount = 1;
	private bool displayStat = false;

	public float hp;
	public GameObject HPbar;
	float hpscale;

	public GUIText DamageText;
	public GUIText NameText;
	public GUIText DefenceText;
	public GUIText TypeText;
	public GUIText CharCountText;

	[HideInInspector]public bool Onclick;

	// Use this for initialization
	void Start () {
		Onclick = false;
		hp = Armor;

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Input.mousePosition.y);

		hpscale = hp / Armor;
		HPbar.transform.localScale = new Vector3 (hpscale, HPbar.transform.localScale.y, HPbar.transform.localScale.z);

	}
	void OnMouseEnter () {
		displayStat = true;
		//Debug.Log (this.name);
	}
	void OnMouseExit () {
		displayStat = false;
		//Debug.Log (this.name);
	}

	void OnInputDown()
	{
		Onclick = true;
	}

	void OnInputUp()
	{
		Onclick = false;
	}

	void OnGUI () {
		if(displayStat)
		{
			GUI.contentColor = Color.blue;
			GUI.Label (new Rect (Input.mousePosition.x+20, Screen.height-Input.mousePosition.y, 100, 100), "Total Damage " + (Damage*charCount).ToString());
			GUI.Label (new Rect (Input.mousePosition.x+20, Screen.height-Input.mousePosition.y+20, 100, 100), "Type " + (type).ToString());
			GUI.Label (new Rect (Input.mousePosition.x+20, Screen.height-Input.mousePosition.y+40, 100, 100), "Total Defence " + (Armor*charCount).ToString());	
			GUI.Label (new Rect (Input.mousePosition.x+20, Screen.height-Input.mousePosition.y+70, 100, 100), "Char count" + charCount.ToString());
			NameText.text = "Name: " + this.name;
			DamageText.text = "Total Damage: " + (Damage*charCount).ToString();
			DefenceText.text = "Total Defence: " + (Armor*charCount).ToString();
			//TypeText.text = "Type: " (type);
			CharCountText.text = "Char count" + charCount.ToString();

		}

		if(Onclick)
		{


		}

		//displayStat = false;

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
	public void AttackM(GameObject target) {//Multiplayer version of attack
		Attack(target);
		NetworkView nv = GetComponent<NetworkView>();
		nv.RPC("Attack", RPCMode.Others,target);
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
		//GameStart.StopCameraPan ();
		if (t.Armor < DmgDealt)
		{
			this.gameObject.GetComponent<TranslateMonster> ().nextTurn();
			Destroy (target);
		}
		else
		{
			this.gameObject.GetComponent<TranslateMonster> ().nextTurn();
			Destroy (this.gameObject);
		}
	}
}
