using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckGrid : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}
	void OnMouseDown ()
	{
		if (GameStart.Player1()) {
			GameObject go = GameObject.FindGameObjectWithTag ("PlayerTile");
			MonsterGrid MonsterScript = this.GetComponent<MonsterGrid> ();
			if (!MonsterScript.GridCheck (go.name)) {
				GameObject[] Monster = GameObject.FindGameObjectsWithTag (this.tag);
				foreach (GameObject go2 in Monster) {
					MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
					if (MonsterScript2.GridCheck (go.name)) {
						if (go2.tag == this.tag) {
							if (MonsterScript2.GridActive) {
								MonsterScript2.GridDelete ();
								MonsterScript2.GridActive = false;
							}
							Stats cg = go2.GetComponent<Stats> ();
							this.GetComponent<Stats> ().charCount += cg.charCount;
							Destroy (go2);
							if(GameStart.movesleft > 0)
								GameStart.movesleft--;
							break;
							//Vector3 go2newPos = this.transform.position;
							//go2.transform.position = go2newPos;
						}
					}
				}
				GameObject[] Monster2 = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject go2 in Monster2) {
					MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
					if (MonsterScript2.GridCheck (go.name)) {
							if (MonsterScript2.GridActive) {
								MonsterScript2.GridDelete ();
								MonsterScript2.GridActive = false;
							}
							if(go2.tag == "Player" && this.tag == "EnemyMonster" ||
							this.tag == "Player" && go2.tag == "EnemyMonster" )
							{
								go2.GetComponent<TranslateMonster>().Translate(true,this.transform.position);
								//go2.GetComponent<Stats>().Attack(this.gameObject);
							}
							break;
							//Vector3 go2newPos = this.transform.position;
							//go2.transform.position = go2newPos;
					}
				}
			}
		}
	}
	protected List<GameObject> insideMe = new List<GameObject>();

	private void OnTriggerEnter(Collider e)
	{
		if(e.tag == "Player" && this.tag == "EnemyMonster" ||
		   this.tag == "Player" && e.tag == "EnemyMonster" )
		{
			//e.GetComponent<TranslateMonster>().Translate(true,this.transform.position);
			e.GetComponent<Stats>().Attack(this.gameObject);
		}
		Debug.Log("Collided");
	}
	// Update is called once per frame
	void Update () {
	}
}
