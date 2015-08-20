using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	public float AttackTimer = 1.0f;
	float attackdmg = 0;
	public float EneDMG = 0;
	public float EDMG = 0;
	public float minDmg = 5;
	public float maxDmg = 10;
	public float coolDown = 3.0f;
	GameObject attackTarget = null;
	[HideInInspector]public bool attacking = false;
	bool AI = false;

	public bool playerInRange;
	public float timer;
	bool Multiplayer = false;

	public GameObject FightAnim;
	public GameObject IdleAnim;
	public bool hasAnimation = true;

	// Use this for initialization
	void Start () {
		//AttackTimer = 0.0f;
		//coolDown = 2.0f;
		if (FightAnim == null || IdleAnim == null)
			hasAnimation = false;
		if(GameObject.Find("Multiplayer"))
			Multiplayer = true;
	}

	// Update is called once per frame
	// Update is called once per frame
	public void Update () {

		if (attackTarget == null) {

			if (hasAnimation && attacking) {
				FightAnim.SetActive (false);
				IdleAnim.SetActive (true);
			}
			attacking = false;
		}

		if(attacking)
		{
			attackdmg = Random.Range (minDmg, maxDmg);
		
			if (AttackTimer > 0) 
				AttackTimer -= Time.deltaTime;
		
			//if (AttackTimer < 0)
				//AttackTimer = 0;
		
			//if (AttackTimer <= 0)
			//	AttackTimer = coolDown;

			if(AI)
				EnAttack();
			if(Multiplayer)
			{
				if(Network.isClient)
					Attack();
			}
		}
	}

	public void StartEnAttack(GameObject target, bool singleplayer = true)
	{
		target.transform.position = this.transform.position + new Vector3(0,0,-1.0f);
		if(singleplayer)
			target.GetComponent<TranslateMonster>().nextTurn();
		if(Multiplayer)
		{
			if(GameMultiplayer.Player1() && Network.isServer)
				target.GetComponent<TranslateMonster>().nextTurn();
			else if(GameMultiplayer.Player2() && Network.isClient)
				GetComponent<TranslateMonster>().nextTurn();
		}
		attacking = true;
		attackTarget = target;
		AI = singleplayer;
		GetComponent<TranslateMonster> ().Stop ();

		if (hasAnimation) {
			FightAnim.SetActive (true);
			IdleAnim.SetActive (false);
		}

		if(GameObject.Find("Camera").GetComponent<Camera>().enabled)
		{
			GameObject.Find("Camera").GetComponent<Camera>().enabled = false;
			GameObject.Find("BattleCamera").GetComponent<Camera>().enabled = true;
			GameObject.Find("BattleCamera").transform.position = this.transform.position+new Vector3(3,1,-0.3f);
		}
	}

	public void EnAttack()
	{
		PlayerHealth t = attackTarget.GetComponent<PlayerHealth>();
		Debug.Log ("EnemyIn");
		if (t.transform.tag == "Player") {
			Debug.Log ("PlayerTOuched");
			if (AttackTimer <= 0) {
				t.PLHealthChange (attackdmg);
				AttackTimer = coolDown;
			}
		}
		
	}

	public void Attack(){
		Debug.Log ("Got in Attack");
		PlayerHealth t = attackTarget.GetComponent<PlayerHealth>();

		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetKeyUp (KeyCode.A)) {
			if (t.transform.tag == "Player") {
				t.PLHealthChange (attackdmg);
			}
		}//here ends for input A
		#endif	
		
		#if UNITY_ANDROID
		//if (Click attack button)) {
		//	if (t.transform.tag == "Player") {
		//		t.PLHealthChange (attackdmg);
		//	}
		//}//here ends for input A
		#endif	
	}

}
