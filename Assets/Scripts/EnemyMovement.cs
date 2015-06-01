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
		if (!GameStart.Player1 () && GameStart.movesleft > 0) {
						if(enemyUnits.Length < 1)
							GameStart.movesleft--;
						foreach (GameObject enemy in enemyUnits) {
				if (enemy != null && !enemy.GetComponent<TranslateMonster> ().TMonster && GameStart.movesleft == currMove && !enemy.GetComponent<MonsterGrid> ().turnOver) {
										if (delay >= 50) {
												delay = 0;
												currMove--;
												int range = enemy.GetComponent<MonsterGrid> ().range;
												Collider[] hitColliders = Physics.OverlapSphere (enemy.GetComponent<MonsterGrid> ().transform.position, range*2);
												int i = 0;
												bool moved = false;
												while (i < hitColliders.Length) {
														if (hitColliders [i].transform.tag == "Player") {
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
																			float NextPos = Vector3.Distance (hitColliders [i].gameObject.transform.position, tile.transform.position);
																			float CurrPos = Vector3.Distance (hitColliders [i].gameObject.transform.position, closestObject.transform.position);
																			if (NextPos <= CurrPos) {
																					closestObject = tile;
																			}
																		}
																		
																}
																if (closestObject != enemy)
																{
																	GameStart.disableGrid = true;
																	enemy.GetComponent<TranslateMonster> ().Translate (true, closestObject.transform.position);
																}
																enemy.GetComponent<MonsterGrid> ().GridDelete ();
																enemy.GetComponent<MonsterGrid> ().GridActive = false;
																Debug.Log (closestObject.name);
																moved = true;
																break;
														}
														i++;
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
													enemy.GetComponent<MonsterGrid> ().GridDelete ();
													enemy.GetComponent<MonsterGrid> ().GridActive = false;
												}
										}
										delay++;
								}
						}
				} 
		else
			currMove = enemyUnits.Length;
	}
}
