using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	
	public GUITexture texture;
	public float PlayerAttDmg;
//	public GameObject target;
	public float PlayDMG = 0;
	public float DMG = 10;
	public float minDmg = 5;
	public float maxDmg = 10;
	GameObject attackTarget = null;
	
	public bool hasAnimation = true;
	[HideInInspector]public bool attacking = false;

	float TimeAttacked = 0;		//what time player attacked; check if time is in range
	float ExDMG, VgDMG, GDMG;		//excellent damage, very good damage, good damage
	bool TimeExDMG, TimeVgDMG, TimeGDMG = false;		//Time for the damage
	bool TimeInRange, StartTCount;			//check if time is in range; start time count
	float TimeCount, TimeBeforeCount;			//the time; Time Before Reach; Time before count
	bool PlayerAttacked;			//Check when player attacked
	bool ActivateButton;
	bool TimeReached;
	float TimeTexCount;		// count down to texture show
	public bool animationOn;
	public GameObject FightAnim;
	public GameObject IdleAnim;
	
	public string animationName = "Soldier_Attack";
	public GameObject aBattleCamera;

	public GameObject Arrow;
	//Animator anim;
	Animation anim;
	bool Multiplayer = false;
	// Use this for initialization
	void Start () {
		if (texture == null)
			texture = GameObject.Find ("Battle_Button").GetComponent<GUITexture>();
		TimeCount = 0;
		PlayerAttacked = false;
		StartTCount = false;
		ActivateButton = false;
		TimeReached = false;
		TimeTexCount = 0.5f;
		animationOn = true;
		anim = GetComponent<Animation>();
		if (FightAnim == null || IdleAnim == null)
			hasAnimation = false;
		if(GameObject.Find("Multiplayer"))
			Multiplayer = true;
	}

	[RPC]
	public void ShootArrow (Vector3 targetPos,GameObject Target) {
		GameObject ShotArrow = Instantiate (Arrow, transform.position, Quaternion.identity) as GameObject;
		ShotArrow.GetComponent<Arrow> ().SetTarget (Target);
	}

	void Update () {


//		anim = aBattleCamera.GetComponent<Animator> ();
		

		PlayerAttDmg = Random.Range (5,10);	//random damage
		ExDMG = Random.Range (15, 20);
		VgDMG = Random.Range (5,15);
		GDMG = Random.Range (1, 5);

		if(attacking)
		{
			if(attackTarget != null)
			{
				if(!Multiplayer)
					Attack();
				else if(Network.isServer)
					Attack();
				if (hasAnimation) {
					FightAnim.SetActive (false);
					IdleAnim.SetActive (true);
				}
			}
			else
				attacking = false;
		}
		if (StartTCount == true) {
			if (TimeCount == 0) {
				TimeCount += Time.deltaTime;
			}
		}

		if (TimeTexCount > 0.0f) {
			TimeTexCount -= Time.deltaTime;
		} else if (TimeTexCount < 0.0f) {
			TimeReached = true;
			
		}

	}

	public void CheckTime(){
		
		if (TimeAttacked > 0.0f && TimeAttacked <= 0.01f) {
			TimeExDMG = true;
			
			///	texture.GetComponent<GUITexture> ().color = Color.white;
			Debug.Log("a");
		} else if (TimeAttacked >= 0.02f && TimeAttacked < 0.05f) {
			TimeVgDMG = true;
			
			//	texture.GetComponent<GUITexture> ().color = Color.white;
			Debug.Log("b");
		} else if (TimeAttacked > 0.05f) {
			TimeGDMG = true;
			
			//	texture.GetComponent<GUITexture> ().color = Color.white;
			Debug.Log("c");
		}
		Debug.Log ("CheckTime IN");
	}

	public void ChangeTexture(){
		
		if (ActivateButton == true) {
			texture.GetComponent<GUITexture> ().color = Color.red;
			//TimeBeforeCount = 0.0f;
		} else if(ActivateButton == false) {
			texture.GetComponent<GUITexture> ().color = Color.white;
		}
		
	}

	public void StartAttack(GameObject target){
		Vector3 newPos = this.transform.position + new Vector3(0,0,1.0f);
		newPos.y = target.transform.position.y;
		target.transform.position = newPos;
		attacking = true;
		attackTarget = target;

		if (hasAnimation) {
			FightAnim.SetActive (true);
			IdleAnim.SetActive (false);

		}

		if(GameObject.Find("Camera").GetComponent<Camera>().enabled)
		{
			Debug.Log ("asd");
			GameObject.Find("Camera").GetComponent<Camera>().enabled = false;

				GameObject.Find("BattleCamera").GetComponent<Camera>().enabled = true;
		
			Debug.Log ("asd2");
				GameObject.Find("BattleCamera").transform.position = this.transform.position+new Vector3(3,1,-0.3f);
			/*if(animationOn == false){
				anim.SetBool("StartBattleAnimation", true);
				animationOn = true;
			}else if(animationOn == true){
				anim.SetBool("StartBattleAnimation", false);
			}*/

		}


	}

	public void Attack(){
		Debug.Log ("Got in Attack");
		EnemyHealth t = attackTarget.GetComponent<EnemyHealth> ();
		//if (hasAnimation == true){
		//	anim.Play (animationName);
		//}
		if (TimeReached == true) {
			texture.GetComponent<GUITexture> ().color = Color.red;
			//StartTCount = true;

			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (Input.GetKeyUp (KeyCode.A)) {
				ChangeHP(t);
			}//here ends for input A
			#endif	

			#if UNITY_ANDROID
			//if (Click attack button)) {
			//	ChangeHP(t);
			//}//here ends for input A
			#endif	
			//hasAnimation = false;
		} else {}
	}
	void ChangeHP(EnemyHealth t){
		/*hasAnimation = true;
		PlayerAttacked = true;
		ActivateButton = false;
		texture.GetComponent<GUITexture> ().color = Color.white;
		TimeReached = false;
		
		//Debug.Log ("PlayerA");
		if (PlayerAttacked == true) {
			TimeAttacked = TimeCount;
			//Debug.Log ("PlayerAttacked");
			//Debug.Log (TimeAttacked);
			CheckTime ();
			
			if (t.transform.tag == "EnemyMonster") {
				
				if (TimeExDMG == true) {
					t.EnHealthChange (ExDMG);
					TimeExDMG = false;
					Debug.Log ("Excellent");
				}
				else if (TimeVgDMG == true) {
					t.EnHealthChange (VgDMG);
					Debug.Log ("Very Good");
					TimeVgDMG = false;
				}
				else if (TimeGDMG == true) {
					t.EnHealthChange (GDMG);
					Debug.Log ("Good");
					TimeGDMG = false;
				}
				else  {
					t.EnHealthChange (2);
					Debug.Log ("fucked up");
					TimeGDMG = false;
				}
				StartTCount = false;
				TimeCount = 0;
			}	
		}
		PlayerAttacked = false;
		TimeTexCount = 0.5f;
	}*/

		//hasAnimation = true;
		PlayerAttacked = true;
		ActivateButton = false;
		texture.GetComponent<GUITexture> ().color = Color.white;
		TimeReached = false;
		
		Debug.Log ("PlayerA");
		if (PlayerAttacked == true) {
			TimeAttacked = TimeCount;
			Debug.Log ("PlayerAttacked");
			Debug.Log (TimeAttacked);
			CheckTime ();
			
			if (t.transform.tag == "EnemyMonster") {
				
				if (TimeExDMG == true) {
					t.EnHealthChange (ExDMG);
					TimeExDMG = false;
					Debug.Log ("Excellent");
				}
				else if (TimeVgDMG == true) {
					t.EnHealthChange (VgDMG);
					Debug.Log ("Very Good");
					TimeVgDMG = false;
				}
				else if (TimeGDMG == true) {
					t.EnHealthChange (GDMG);
					Debug.Log ("Good");
					TimeGDMG = false;
				}
				else {
					t.EnHealthChange (2);
					Debug.Log ("fucked up");
					TimeGDMG = false;
				}
				StartTCount = false;
				TimeCount = 0;
			}	
		}
		PlayerAttacked = false;
		TimeTexCount = 0.5f;
	}
}

