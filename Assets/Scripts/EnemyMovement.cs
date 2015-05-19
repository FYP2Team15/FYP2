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
		if (!GameStart.Player1 () && GameStart.movesleft > 0) {
						
						GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag ("EnemyMonster");
						foreach (GameObject enemy in enemyUnits) {
							if (enemy != null && !enemy.GetComponent<TranslateMonster> ().TMonster && GameStart.movesleft == currMove) {
										if (delay >= 50) {
												delay = 0;
												currMove--;
												int range = enemy.GetComponent<MonsterGrid> ().range;
												Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, range);
												int i = 0;
												while (i < hitColliders.Length) {
														if (hitColliders [i].transform.tag == "Player") {
																if (!enemy.GetComponent<MonsterGrid> ().GridActive) {
																		enemy.GetComponent<MonsterGrid> ().GridInit ();
																		enemy.GetComponent<MonsterGrid> ().GridActive = true;
																}
																GameObject[] enemyTile = GameObject.FindGameObjectsWithTag ("EnemyTile");
																GameObject closestObject = null;
																foreach (GameObject tile in enemyTile) {
																		if (closestObject == null)
																				closestObject = tile;
																		float NextPos = Vector3.Distance (hitColliders [i].gameObject.transform.position, tile.transform.position);
																		float CurrPos = Vector3.Distance (hitColliders [i].gameObject.transform.position, closestObject.transform.position);
																		if (NextPos <= CurrPos) {
																				closestObject = tile;
																		}
																}
																enemy.GetComponent<TranslateMonster> ().Translate (true, closestObject.transform.position);
																enemy.GetComponent<MonsterGrid> ().GridDelete ();
																enemy.GetComponent<MonsterGrid> ().GridActive = false;
																Debug.Log (closestObject.name);
																break;
														}
														i++;
												}
										}
										delay++;
								}
						}
				} 
		else
			currMove = 2;
	}
}
