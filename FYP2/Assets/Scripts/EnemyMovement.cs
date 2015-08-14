using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	private int delay = 50;
	private int currMove = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag ("EnemyMonster");
		if (!GameStart.Player1 () && GameStart.movesleft > 0 && GameObject.Find("BattleCamera").GetComponent<Camera>().enabled == false) {
						if(enemyUnits.Length < 1)
							GameStart.movesleft--;
						foreach (GameObject enemy in enemyUnits) {
				if (enemy != null && !enemy.GetComponent<TranslateMonster> ().TMonster && GameStart.movesleft == currMove && !enemy.GetComponent<MonsterGrid> ().turnOver) {
										if (delay >= 50) {
												delay = 0;
												int range = enemy.GetComponent<MonsterGrid> ().range;
												Collider[] hitColliders = Physics.OverlapSphere (enemy.GetComponent<MonsterGrid> ().transform.position, range*2);
												int i = 0;
												bool moved = false;
												while (i < hitColliders.Length) {
														if (hitColliders [i].transform.tag == "Obstacle")
															break;
														if (hitColliders [i].transform.name == "Child") {
															moved = Move(enemy, hitColliders [i].gameObject);
																break;
														}
														i++;
												}
												if(!moved)
												{
													i = 0;
													while (i < hitColliders.Length) {
														if (hitColliders [i].transform.tag == "Player") {
															moved = Move(enemy, hitColliders [i].gameObject);
														break;
														}
														i++;
													}
												}
												if(!moved)
												{
													if (!enemy.GetComponent<MonsterGrid> ().GridActive) {
														enemy.GetComponent<MonsterGrid> ().GridInit ();
														enemy.GetComponent<MonsterGrid> ().GridActive = true;
													}
													GameObject[] enemyTile = GameObject.FindGameObjectsWithTag ("EnemyTile");
													int rand = Random.Range(0, enemyTile.Length);
													GameStart.disableGrid = true;
													enemy.GetComponent<TranslateMonster> ().Translate (true, enemyTile[rand].transform.position);
													currMove--;
													enemy.GetComponent<MonsterGrid> ().GridDelete ();
													enemy.GetComponent<MonsterGrid> ().GridActive = false;
												}
										}
										delay++;
								}
						}
				} 
		else if (GameObject.Find("BattleCamera").GetComponent<Camera>().enabled == false) 
			currMove = enemyUnits.Length;
	}
	bool Move(GameObject enemy, GameObject player)
	{
		if (!enemy.GetComponent<MonsterGrid> ().GridActive) {
			enemy.GetComponent<MonsterGrid> ().GridInit ();
			enemy.GetComponent<MonsterGrid> ().GridActive = true;
		}
		GameObject[] enemyTile = GameObject.FindGameObjectsWithTag ("EnemyTile");
		GameObject closestObject = enemy;
		foreach (GameObject tile in enemyTile) {
			Collider[] tiles = Physics.OverlapSphere (tile.transform.position, 0);
			int j = 0;
			bool obstacle = false;
			while(j < tiles.Length)
			{
				if(tiles[j].transform.tag == "Obstacle")
				{
					obstacle = true;
					break;
				}
				j++;
			}
			
			if(!obstacle)
			{
				float NextPos = Vector3.Distance (player.transform.position, tile.transform.position);
				float CurrPos = Vector3.Distance (player.transform.position, closestObject.transform.position);
				if (NextPos <= CurrPos) {
					closestObject = tile;
				}
			}
			
		}
		if (closestObject != enemy)
		{
			GameStart.disableGrid = true;
			enemy.GetComponent<TranslateMonster> ().Translate (true, closestObject.transform.position);
			currMove--;
			Debug.Log (closestObject.name);
			enemy.GetComponent<MonsterGrid> ().GridDelete ();
			enemy.GetComponent<MonsterGrid> ().GridActive = false;
			return true;
		}
		enemy.GetComponent<MonsterGrid> ().GridDelete ();
		enemy.GetComponent<MonsterGrid> ().GridActive = false;
		return false;
	}
}
