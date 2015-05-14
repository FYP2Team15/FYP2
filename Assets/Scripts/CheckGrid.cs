using UnityEngine;
using System.Collections;

public class CheckGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	void OnMouseDown ()
	{
		GameObject[] Monster = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject go in Monster ) {
			if(go != null && go.name != this.name)
			{
				MonsterGrid MonsterScript = go.GetComponent<MonsterGrid>();
				if(MonsterScript.GridActive)
				{
					MonsterScript.GridDelete ();
					MonsterScript.GridActive = false;
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
