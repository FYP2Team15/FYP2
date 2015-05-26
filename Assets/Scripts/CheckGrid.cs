using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckGrid : MonoBehaviour {
	bool PlayerT = false;
	public int nameLength = 3;
	public bool setToCombine = false;//set to combine flag
	public bool setToCombineAndDestroy = false;//set to combine and destroyed flag
	// Use this for initialization
	void Start () {

	}
	void OnMouseDown ()
	{
		if (GameStart.Player1()) {//If player turn
			PlayerT = false;
			GameObject go = GameObject.FindGameObjectWithTag ("PlayerTile");//Get a tile that belongs to player
			MonsterGrid MonsterScript = this.GetComponent<MonsterGrid> ();//Get the monster grid of this gameobj
			if (!MonsterScript.GridCheck (go.name)) {
				GameObject[] Monster = GameObject.FindGameObjectsWithTag (this.tag);
				foreach (GameObject go2 in Monster) {
					MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
					if (MonsterScript2.GridCheck (go.name)) {
						Collider[] tiles = Physics.OverlapSphere (this.transform.position, 0.1f);
						int j = 0;
						bool onTile = false;
						while(j < tiles.Length)
						{
							if(tiles[j].transform.tag == "PlayerTile")
							{
								onTile = true;
								break;
							}
							j++;
						}
						if (go2.tag == this.tag && onTile && this.name.Substring(0, nameLength) == go2.name.Substring(0, nameLength)) {
							if (MonsterScript2.GridActive) {//Closes grid after checking
								MonsterScript2.GridDelete ();
								MonsterScript2.GridActive = false;
							}
							go2.GetComponent<TranslateMonster>().Translate(true,this.transform.position);//translate go2 to this
							go2.GetComponent<CheckGrid>().setToCombineAndDestroy = true;//mark them as combined target
							setToCombine = true;
							break;
							//Vector3 go2newPos = this.transform.position;
							//go2.transform.position = go2newPos;
						}
					}
				}
				if(!PlayerT)
				{
					GameObject[] Monster2 = GameObject.FindGameObjectsWithTag ("Player");
					foreach (GameObject go2 in Monster2) {
						MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
						if (MonsterScript2.GridCheck (go.name)) {
							if (MonsterScript2.GridActive) {
								MonsterScript2.GridDelete ();
								MonsterScript2.GridActive = false;
							}
							if(go2.tag == "Player" && this.tag == "EnemyMonster")
							{
								GameStart.disableGrid = true;
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
	}
	protected List<GameObject> insideMe = new List<GameObject>();

	private void OnTriggerEnter(Collider e)
	{

	}
	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 0);//get gameobjects right beside this object
		int i = 0;
		while (i < hitColliders.Length) {
			if (hitColliders [i].transform.tag == "Player" && this.tag == "Player" && setToCombine)
			{
				if(hitColliders [i].GetComponent<CheckGrid>().setToCombineAndDestroy)
				{
					Stats cg = hitColliders [i].GetComponent<Stats> ();
					this.GetComponent<Stats> ().charCount += cg.charCount;//Add one count to this gameobject
					Destroy (hitColliders [i].gameObject);//destroy the other gameobject
					PlayerT = true;
					if(GameStart.movesleft > 0)
						GameStart.movesleft--;//use up 1 move
				}
			}
			if (hitColliders [i].transform.tag == "Player" && this.tag == "EnemyMonster") {//if this is enemy and collided to player
								hitColliders [i].GetComponent<Stats> ().Attack (this.gameObject);//attack player
						}
						if (hitColliders [i].transform.tag == "EnemyMonster" && this.tag == "Player") {//if this is player and collided to enemy
								this.GetComponent<Stats> ().Attack (hitColliders [i].gameObject);//attack enemy
						}
			i++;
		} 
	}
}
