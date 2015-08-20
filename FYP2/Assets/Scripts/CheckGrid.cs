using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckGrid : MonoBehaviour {
	PlayerAttack PA;
	PlayerHealth PH;
	EnemyAttack EA;
	EnemyHealth EH;
	bool PlayerT = false;
	public bool PlayPause;
	public int nameLength = 3;		
	public int NumMonster = 0;			// number of monsters joined into one
	public int PlayStats = 0;
	[HideInInspector]public bool setToCombine = false;//set to combine flag
	[HideInInspector]public bool setToCombineAndDestroy = false;//set to combine and destroyed flag
	// Use this for initialization
	void Start () {
		PlayPause = true;
		if(this.tag == "Player")
		GetComponent<PlayerHealth> ().nameLength = nameLength;
		else if(this.tag == "EnemyMonster")
		GetComponent<EnemyHealth> ().nameLength = nameLength;
	}
	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseDown ()
	{
		Interact ();
				
	}
	#endif	

	#if UNITY_ANDROID
	void OnTouch ()
	{
		Interact ();
		
	}
	#endif	
	void Interact()
	{
		
		if (GameStart.Player1 ()) {//If player turn
			PlayerT = false;
			GameObject go = GameObject.FindGameObjectWithTag ("PlayerTile");//Get a tile that belongs to player
			if (go != null) {
				MonsterGrid MonsterScript = this.GetComponent<MonsterGrid> ();//Get the monster grid of this gameobj
				if (!MonsterScript.GridCheck (go.name)) {
					GameObject[] Monster = GameObject.FindGameObjectsWithTag (this.tag);
					foreach (GameObject go2 in Monster) {
						MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
						if (MonsterScript2.GridCheck (go.name)) {
							Collider[] tiles;
							if (this.GetComponent<CarScript>() != null)
								tiles = Physics.OverlapSphere (this.transform.position+this.GetComponent<MonsterGrid>().Offset,1);
							else
								tiles = Physics.OverlapSphere (this.transform.position+this.GetComponent<MonsterGrid>().Offset,0);
							int j = 0;
							bool onTile = false;
							bool MergeTile = false;
							while (j < tiles.Length) {
								if (tiles [j].transform.tag == "PlayerTile") {
									onTile = true;
									break;
								}
								if (tiles [j].transform.tag == "MergeTile") {
									MergeTile = true;
									break;
								}
								j++;
							}
							if (go2.tag == this.tag && onTile && this.GetComponent<CarScript>() != null)
							{
								if (MonsterScript2.GridActive) {//Closes grid after checking
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
								go2.GetComponent<TranslateMonster> ().Translate (true, this.transform.position+this.GetComponent<MonsterGrid>().Offset);
								this.GetComponent<CarScript>().EnterWhenReached(go2);
							}
							else if (go2.tag == this.tag && MergeTile && this.name.Substring (0, nameLength) == go2.name.Substring (0, nameLength)) {
								NumMonster += 1;
								if (MonsterScript2.GridActive) {//Closes grid after checking
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
								go2.GetComponent<TranslateMonster> ().Translate (true, this.transform.position);//translate go2 to this
								go2.GetComponent<CheckGrid> ().setToCombineAndDestroy = true;//mark them as combined target
								setToCombine = true;
								PA.PlayDMG = NumMonster * PA.DMG;

								break;
								//Vector3 go2newPos = this.transform.pocombsition;
								//go2.transform.position = go2newPos;
							}
						}
					}
					if (!PlayerT) {
						GameObject[] Monster2 = GameObject.FindGameObjectsWithTag ("Player");
						foreach (GameObject go2 in Monster2) {
							MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
							if (MonsterScript2.GridCheck (go.name)) {
								Collider[] tiles = Physics.OverlapSphere (this.transform.position, 0.1f);
								int j = 0;
								bool onTile = false;
								while (j < tiles.Length) {

									if (tiles [j].transform.tag == "PlayerTile") {
										onTile = true;
										break;
									}
									j++;
								}
								if (go2.tag == "Player" && onTile && this.tag == "EnemyMonster") {
									if (MonsterScript2.GridActive) {
										MonsterScript2.GridDelete ();
										MonsterScript2.GridActive = false;
									}
									
									GameStart.disableGrid = true;
									go2.GetComponent<TranslateMonster> ().Translate (true, this.transform.position);
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
			else
			{
				go = GameObject.FindGameObjectWithTag ("ArcherTile");//Get a tile that belongs to player
				if (go != null)
				{
					GameObject[] Monster2 = GameObject.FindGameObjectsWithTag ("Player");
					foreach (GameObject go2 in Monster2) {
						MonsterGrid MonsterScript2 = go2.GetComponent<MonsterGrid> ();
						if (MonsterScript2.GridCheck (go.name)) {
							Collider[] tiles = Physics.OverlapSphere (this.transform.position, 0.1f);
							int j = 0;
							bool onArcherTile = false;
							while (j < tiles.Length) {
								if (tiles [j].transform.tag == "ArcherTile") {
									onArcherTile = true;
									break;
								}
								j++;
							}
							if (go2.tag == "Player" && onArcherTile && this.tag == "EnemyMonster") {
								if (MonsterScript2.GridActive) {
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
							
								GameStart.disableGrid = true;
								go2.GetComponent<MonsterGrid> ().throwOver = true;
								go2.GetComponent<PlayerAttack> ().ShootArrow (this.transform.position,this.gameObject);
								//go2.GetComponent<Stats>().Attack(this.gameObject);
							}
						}
					}
				}
			}
		}
	}
	protected List<GameObject> insideMe = new List<GameObject>();

	void CheckPlayerStats(){		//Check player stats when merging between two

	}

	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1.0f);//get gameobjects right beside this object
		int i = 0;
		if (PlayPause == true) {
						while (i < hitColliders.Length) {
								if (hitColliders [i].transform.tag == "Player" && this.tag == "Player" && setToCombine) {
										if (hitColliders [i].GetComponent<CheckGrid> ().setToCombineAndDestroy) {
												if(hitColliders [i].GetComponent<PlayerHealth> () != null)
												{
													PlayerHealth cg = hitColliders [i].GetComponent<PlayerHealth> ();
													this.GetComponent<PlayerHealth> ().charCount += cg.charCount;//Add one count to this gameobject
												}
												this.GetComponent<MonsterGrid> ().GridActive = false;
												GameStart.disableGrid = false;
												GameStart.disableCameraControl = false;
												setToCombine = false;
												Destroy (hitColliders [i].gameObject);//destroy the other gameobject
												PlayerT = true;
												if (GameStart.movesleft > 0)
														GameStart.movesleft--;//use up 1 move
										}
								}

								if (hitColliders [i].transform.tag == "EnemyMonster" && this.tag == "Player" 
				    && !hitColliders [i].GetComponent<EnemyAttack> ().attacking && !GetComponent<PlayerAttack> ().attacking && PH.CurArmor == EH.CurArmor /*&& PA.PlayDMG == EA.EneDMG*/) {//if this is player and collided to enemy
										hitColliders [i].GetComponent<EnemyAttack> ().StartEnAttack (this.gameObject);//attack enemy

										GetComponent<PlayerAttack> ().StartAttack (hitColliders [i].gameObject);//attack player
										Debug.Log ("Got minus");	
										//this.GetComponent<TranslateMonster> ().nextTurn();
								}else if (hitColliders [i].transform.tag == "EnemyMonster" && this.tag == "Player" 
				         		 && !hitColliders [i].GetComponent<EnemyAttack> ().attacking && !GetComponent<PlayerAttack> ().attacking && PA.PlayDMG > EA.EneDMG) {		// if player and collided to enemy but player hp more than enemy
									EH.CurArmor = 0;
									
								}
								i++;
						}
				} else if (PlayPause == false) {
				}
	}
}
