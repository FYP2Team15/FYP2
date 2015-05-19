using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	public float Damage = 10;
	public int type = 0;
	public int[] weaknesses;
	public int[] strengths;
	public float Armor = 10;
	public int charCount = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		if (t.Armor > DmgDealt)
			Destroy (target);
		else
			Destroy (this);
	}
}
