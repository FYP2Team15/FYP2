using UnityEngine;
using System.Collections;

public class TeleportTile : MonoBehaviour {

	public GameObject teleportTarget;
	public bool resetMove = true;
	[HideInInspector]public GameObject currentMonster = null;
	GameObject lastTeleported = new GameObject ();
	bool MultiMode = false;
	// Use this for initialization
	void Start () {
		if (GameObject.Find ("Multiplayer") != null)
			MultiMode = true;
		//if (MultiMode)
		//	lastTeleported = GameMultiplayer.movesleft;
		//else 
		//	lastTeleported = GameStart.movesleft;

	}
	
	// Update is called once per frame
	void Update () {
		//if (lastTeleported != null) 
		//{
		//	if(!cMonsterStillOnTile())
		//		lastTeleported = null;
		//}
		if(teleportTarget != null)
		{
			TeleportMonster();
		}
	}
	void Teleport(GameObject Monster) {	
		Vector3 newPos = teleportTarget.transform.position;
		newPos.y = Monster.transform.position.y;
		Monster.transform.position = newPos;
		//TeleportTile targetTile = teleportTarget.GetComponent<TeleportTile> ();
		lastTeleported = Monster;
		if(resetMove)
		{
			if(MultiMode)
			{
				GameMultiplayer.movesleft++;
				Monster.GetComponent<MMonsterGrid>().turnOver = false;
			}
			else
			{
				GameStart.movesleft++;
				Monster.GetComponent<MonsterGrid>().turnOver = false;
			}

		}
	}

	void TeleportMonster(){
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 0.3f);//get gameobjects right beside this object
		foreach(Collider obj in hitColliders) {
			if(obj.gameObject.tag == "Player" || obj.gameObject.tag == "EnemyMonster")
			{
				if(!obj.gameObject.GetComponent<TranslateMonster>().TMonster && obj.gameObject != teleportTarget.GetComponent<TeleportTile> (). lastTeleported) {
					Teleport(obj.gameObject);
				}
			}
		}
	}
	bool cMonsterStillOnTile(){
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 0.3f);//get gameobjects right beside this object
		foreach(Collider obj in hitColliders) {
			if(obj.gameObject.tag == "Player" || obj.gameObject.tag == "EnemyMonster")
			{
				if(obj.gameObject == teleportTarget.GetComponent<TeleportTile> (). lastTeleported)
					return true;
			}
		}
		return false;
	}
}
