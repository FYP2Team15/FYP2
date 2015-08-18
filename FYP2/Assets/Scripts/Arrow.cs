using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	GameObject Target;
	bool hasAtarget = false;
	public float Damage = 20;
	// Use this for initialization
	void Start () {
		GetComponent<TranslateMonster> ().Translate (true, Target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<TranslateMonster> ().TMonster && hasAtarget) {
			DamageTarget(Target);
		}
	}

	public void SetTarget(GameObject target) {
		Target = target;
		hasAtarget = true;
	}

	public void DamageTarget(GameObject target) {
		if (target.GetComponent<EnemyHealth> () != null)
			target.GetComponent<EnemyHealth> ().EnHealthChange (Damage * target.GetComponent<EnemyHealth> ().charCount);
		else if (target.GetComponent<PlayerHealth> () != null)
			target.GetComponent<PlayerHealth> ().PLHealthChange (Damage * target.GetComponent<PlayerHealth> ().charCount);
		hasAtarget = false;
		Destroy (this.gameObject);
		GameStart.movesleft++;
	}
}
