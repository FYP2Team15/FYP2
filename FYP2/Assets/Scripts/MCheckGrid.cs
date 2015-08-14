using UnityEngine;
using System.Collections;

public class MCheckGrid : MonoBehaviour {
	//bool PlayerT = false;
	public int nameLength = 3;
	[HideInInspector]public bool setToCombine = false;//set to combine flag
	[HideInInspector]public bool setToCombineAndDestroy = false;//set to combine and destroyed flag
	// Use this for initialization
	void Start () {
		
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
		if (GameMultiplayer.Player1() && Network.isServer) {//If player2 turn
			GameObject go = GameObject.FindGameObjectWithTag ("PlayerTile");//Get a tile that belongs to player1
			if(go != null)
			{
				MMonsterGrid MonsterScript = this.GetComponent<MMonsterGrid> ();//Get the monster grid of this gameobj
				if (!MonsterScript.GridCheck (go.name)) {
					GameObject[] Monster = GameObject.FindGameObjectsWithTag (this.tag);
					foreach (GameObject go2 in Monster) {
						MMonsterGrid MonsterScript2 = go2.GetComponent<MMonsterGrid> ();
						if (MonsterScript2.GridCheck (go.name)) {
							Collider[] tiles;
							if (this.GetComponent<CarScript>() != null)
								tiles = Physics.OverlapSphere (this.transform.position+this.GetComponent<MonsterGrid>().Offset,1);
							else
								tiles = Physics.OverlapSphere (this.transform.position+this.GetComponent<MonsterGrid>().Offset,0);
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
							if (go2.tag == this.tag && onTile && this.GetComponent<CarScript>() != null)
							{
								if (MonsterScript2.GridActive) {//Closes grid after checking
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
								go2.GetComponent<TranslateMonster> ().Translate (true, this.transform.position+this.GetComponent<MonsterGrid>().Offset);
								this.GetComponent<CarScript>().EnterWhenReached(go2);
							}
							else if (go2.tag == this.tag && onTile && this.name.Substring(0, nameLength-1) == go2.name.Substring(0, nameLength-1)) {
								if (MonsterScript2.GridActive) {//Closes grid after checking
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
								
								go2.GetComponent<MCheckGrid>().SetToCombine();
								this.setToCombineAndDestroy = true;
								go2.GetComponent<TranslateMonster>().Translate(true,this.transform.position);//translate go2 to this
								//nv.RPC("SetToCombine", RPCMode.Others);
								//nv.RPC("SetToCombineAndDestroy", RPCMode.Others);
								break;
								//Vector3 go2newPos = this.transform.pocombsition;
								//go2.transform.position = go2newPos;
							}
						}
					}
					
				}
			}
		}
		if (GameMultiplayer.Player2() && Network.isClient) {//If player2 turn
			GameObject go = GameObject.FindGameObjectWithTag ("EnemyTile");//Get a tile that belongs to player2
			if(go != null)
			{
				MMonsterGrid MonsterScript = this.GetComponent<MMonsterGrid> ();//Get the monster grid of this gameobj
				if (!MonsterScript.GridCheck (go.name)) {
					GameObject[] Monster = GameObject.FindGameObjectsWithTag (this.tag);
					foreach (GameObject go2 in Monster) {
						MMonsterGrid MonsterScript2 = go2.GetComponent<MMonsterGrid> ();
						if (MonsterScript2.GridCheck (go.name)) {
							Collider[] tiles = Physics.OverlapSphere (this.transform.position, 0);
							int j = 0;
							bool onTile = false;
							while(j < tiles.Length)
							{
								if(tiles[j].transform.tag == "EnemyTile")
								{
									onTile = true;
									break;
								}
								j++;
							}
							if (go2.tag == this.tag && onTile && this.name.Substring(0, nameLength-1) == go2.name.Substring(0, nameLength-1)) {
								if (MonsterScript2.GridActive) {//Closes grid after checking
									MonsterScript2.GridDelete ();
									MonsterScript2.GridActive = false;
								}
								go2.GetComponent<MCheckGrid>().SetToCombine();
								this.setToCombineAndDestroy = true;
								go2.GetComponent<TranslateMonster>().Translate(true,this.transform.position);//translate go2 to this
								
								break;
								//Vector3 go2newPos = this.transform.pocombsition;
								//go2.transform.position = go2newPos;
							}
						}
					}
					
				}
			}
		}
	}

	[RPC]
	private void SetToCombineAndDestroy()
	{
		setToCombineAndDestroy = true;
	}

	[RPC]
	private void SetToCombine()
	{
		setToCombine = true;
	}
	
	[RPC]
	private void Combine(int CharCount)
	{
		//GetComponent<Stats> ().charCount += CharCount;//Add one count to this gameobject
		GetComponent<MMonsterGrid> ().GridActive = false;
		GameMultiplayer.disableGrid = false;
		setToCombine = false;
		if(GameMultiplayer.movesleft > 0)
		GameMultiplayer.movesleft--;//use up 1 move
	}
	
	private void Destroy(NetworkView nv)
	{
		Destroy (this.gameObject);//destroy the other gameobject
		nv.RPC("DestroyNetwork", RPCMode.Others);
	}

	[RPC]
	private void DestroyNetwork()
	{
		Destroy (this.gameObject);//destroy the other gameobject
	}

	// Update is called once per frame
	void Update () {
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1.0f);//get gameobjects right beside this object
		int i = 0;
		while (i < hitColliders.Length) {
			if (hitColliders [i].transform.tag == "Player" && this.tag == "Player" && setToCombine ||
			    hitColliders [i].transform.tag == "EnemyMonster" && this.tag == "EnemyMonster" && setToCombine  )
			{
				if(hitColliders [i].GetComponent<MCheckGrid>().setToCombineAndDestroy)
				{
					//int CharCount = hitColliders [i].GetComponent<Stats>().charCount;
					GameObject multi = GameObject.Find ("Multiplayer");
					NetworkView nv = GetComponent<NetworkView>();
					hitColliders [i].GetComponent<MCheckGrid>().Destroy(nv);
					Combine(1);
					nv.RPC("Combine", RPCMode.Others,1);
				}
			}
			if (hitColliders [i].transform.tag == "Player" && this.tag == "EnemyMonster" && !hitColliders[i].GetComponent<PlayerAttack>().attacking) {//if this is player and collided to enemy
				hitColliders[i].GetComponent<PlayerAttack>().StartAttack(this.gameObject);//attack enemy
				GetComponent<EnemyAttack>().StartEnAttack(hitColliders[i].gameObject,false);//attack player

			}
			if (hitColliders [i].transform.tag == "EnemyMonster" && this.tag == "Player" && !hitColliders[i].GetComponent<EnemyAttack>().attacking) {//if this is player and collided to enemy
				hitColliders[i].GetComponent<EnemyAttack>().StartEnAttack(this.gameObject,false);//attack enemy
				GetComponent<PlayerAttack>().StartAttack(hitColliders[i].gameObject);//attack player
			}
			i++;
		} 
	}
}
