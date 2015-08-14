using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {
	bool Multiplayer = false;
	public GameObject EParticle;
	public bool ExplodeAfterTranslate = false;
	bool startedMoving = false;
	public bool hasAudio = false;
	public string target = "NonAlly";
	GameObject audio = new GameObject();
	public AudioClip translateSound;
	// Use this for initialization
	void Start () {
		audio = GameObject.Find ("Sfx");
		if (audio == null)
			hasAudio = false;
		if(GameObject.Find("Multiplayer") != null)
			Multiplayer = true;
	}

	[RPC]
	public void MDestroy(string name, string tag, bool parent)
	{
		GameObject target = GameObject.Find (name);
		bool unableToFind = true;
		if(target != null)
		{
			if(target.tag == tag)
			{
				Instantiate (EParticle, transform.position, Quaternion.identity);
				unableToFind = false;
				if(parent)
				{
					if(target.transform.parent != null)//if parent exist
						Destroy(target.transform.parent.gameObject);//destroy parent
					else
						Destroy(target);
				}
				else
					Destroy(target);
			}
		}
		if(GameObject.Find("BattleCamera").GetComponent<Camera>().enabled)
		{
			GameObject.Find("BattleCamera").GetComponent<Camera>().enabled = false;
			GameObject.Find("Camera").GetComponent<Camera>().enabled = true;
		}
		if (this.GetComponent<MMonsterGrid>().GridActive)
		{
			this.GetComponent<MMonsterGrid>().GridDelete();
		}
		if (this.GetComponent<Preview>().GridActive)
		{
			this.GetComponent<Preview>().GridDelete();
		}
		if(unableToFind)
			Debug.Log("Unable to find and destroy " + name);
	}
	// Update is called once per frame
	void Update () {
		if(ExplodeAfterTranslate)
		{
			if(GetComponent<TranslateMonster>().TMonster && !startedMoving)
				startedMoving = true;
			if(!GetComponent<TranslateMonster>().TMonster && startedMoving)
				Explosion(target);
		}
	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseOver () {
		if (Input.GetMouseButton (1)) {//when rightclick
			if(Multiplayer)
			{
				if (this.tag == "Player")
					Explosion("EnemyMonster");
				else if (this.tag == "EnemyMonster")
					Explosion("Player");
			}
			else if(this.tag == "Player")
				Explosion("NonAlly");
		}
	}
	#endif

	void Explosion(string target) {
		Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 2.5f);
		foreach(Collider obj in hitColliders)//find target in aoe
		{
			if (target == "All")
			{//if target is enemy
				if(Multiplayer)
				{
					NetworkView nv = GetComponent<NetworkView>();
					nv.RPC("MDestroy", RPCMode.Others,obj.name,obj.tag, true);//send to client
				}
				//Instantiate (EParticle, transform.position, Quaternion.identity);
				if(obj.transform.parent != null)//if parent exist
					Destroy(obj.transform.parent.gameObject);//destroy parent
				else
					Destroy(obj.gameObject);//destroy target
			}
			if (target == "NonAlly" && obj.tag != this.tag && obj.tag != "Terrain" && obj.tag != "Compulsory" && obj.tag != "MainCamera" && obj.tag != "BattleCamera")
			{//if target is enemy
				if(Multiplayer)
				{
					NetworkView nv = GetComponent<NetworkView>();
					nv.RPC("MDestroy", RPCMode.Others,obj.name,obj.tag, true);//send to client
				}
				//Instantiate (EParticle, transform.position, Quaternion.identity);
				if(obj.transform.parent != null && obj.name == "Trigger")//if parent exist
					Destroy(obj.transform.parent.gameObject);//destroy parent
				else
					Destroy(obj.gameObject);//destroy target
			}
			else if (obj.tag == target)
			{//if target is enemy
				if(Multiplayer)
				{
					NetworkView nv = GetComponent<NetworkView>();
					nv.RPC("MDestroy", RPCMode.Others,obj.name,obj.tag, false);//send to client
				}
				if(obj.transform.parent != null && obj.name == "Trigger")//if parent exist
					Destroy(obj.transform.parent.gameObject);//destroy parent
				else
					Destroy(obj.gameObject);//destroy target
			}
		}
		if (Multiplayer) {
			if (this.GetComponent<MMonsterGrid> ().GridActive) {
				this.GetComponent<MMonsterGrid> ().GridDelete ();
			}
		}
		else if (this.GetComponent<MonsterGrid> ().GridActive) {
			this.GetComponent<MonsterGrid> ().GridDelete ();
		}
		if (this.GetComponent<Preview>().GridActive)
		{
			this.GetComponent<Preview>().GridDelete();
		}
		if(Multiplayer)
		{
			NetworkView nv = GetComponent<NetworkView>();
			nv.RPC("MDestroy", RPCMode.Others,this.name,this.tag, false);//send to client
		}
		Instantiate (EParticle, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
		if(GameObject.Find("BattleCamera").GetComponent<Camera>().enabled)
		{
			GameObject.Find("BattleCamera").GetComponent<Camera>().enabled = false;
			GameObject.Find("Camera").GetComponent<Camera>().enabled = true;
		}
		if(hasAudio)
			audio.GetComponent<AudioScript>().playOnceCustom(translateSound);
	}
}
