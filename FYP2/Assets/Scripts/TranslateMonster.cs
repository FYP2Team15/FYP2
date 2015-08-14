using UnityEngine;
using System.Collections;

public class TranslateMonster : MonoBehaviour {
	[HideInInspector]public bool TMonster = false;
	public Vector3 NextPos;
	public bool hasAnimation = true;
	public bool hasAudio = false;
	public string walkName = "walk-cicle";
	public string idleName = "idle";
	public Vector3 MoveSpeed = new Vector3(0.05f,0.02f,0.05f);
	public bool translateY = true;
	GameObject camera;
	CameraControl ccamera;
	Animation anim;
	GameObject audio = new GameObject();
	[HideInInspector]public bool notActuallyOver = false;
	public AudioClip translateSound;
	// Use this for initialization
	void Start () {
		audio = GameObject.Find ("Sfx");
		if (audio == null)
			hasAudio = false;
		camera = GameObject.Find ("Camera");
		ccamera = camera.GetComponent <CameraControl> ();
		anim = GetComponent<Animation>();
	}
	
	[RPC]
	public void Translate(bool T, Vector3 Pos, bool tY = false)
	{
		TMonster = T;
		NextPos = Pos;
		translateY = tY;
		if(hasAnimation)
			anim.Play (walkName);
		if(hasAudio)
			audio.GetComponent<AudioScript>().playOnceCustom(translateSound);
		GameObject multi = GameObject.Find ("Multiplayer");
		if (multi != null) {
			NetworkView nv = GetComponent<NetworkView>();
			nv.RPC("TranslateM", RPCMode.All,true, Pos, false);
			this.GetComponent<MMonsterGrid> ().turnOver = true;
			GameMultiplayer.disableCameraControl = true;
		}
		else
		{
			this.GetComponent<MonsterGrid> ().turnOver = true;
			GameStart.disableCameraControl = true;
		}
	}
	
	[RPC]
	public void TranslateM(bool T, Vector3 Pos, bool tY = false)
	{
		
		TMonster = T;
		NextPos = Pos;
		translateY = tY;
		if(hasAnimation)
			anim.Play (walkName);
		this.GetComponent<MMonsterGrid> ().turnOver = true;
		GameMultiplayer.disableCameraControl = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(TMonster)
		{
			ccamera.target = this.transform;
			Vector3 tempPos = this.transform.position;
			if(tempPos.x < NextPos.x+0.1f && tempPos.x+0.1f > NextPos.x)//fix to x
				tempPos.x = NextPos.x;
			if(tempPos.y < NextPos.y+0.1f && tempPos.y+0.1f > NextPos.y)//fix to y
				tempPos.y = NextPos.y;
			if(tempPos.z < NextPos.z+0.1f && tempPos.z+0.1f > NextPos.z)//fix to z
				tempPos.z = NextPos.z;
			if(tempPos.x < NextPos.x)//translate x
				tempPos.x = tempPos.x + MoveSpeed.x;
			if(tempPos.x > NextPos.x)
				tempPos.x = tempPos.x - MoveSpeed.x;
			if(translateY)
			{
				if(tempPos.y < NextPos.y)//translate y
					tempPos.y = tempPos.y + MoveSpeed.y;
				if(tempPos.y > NextPos.y)
					tempPos.y = tempPos.y - MoveSpeed.y;
			}
			if(tempPos.z < NextPos.z)//translate z
				tempPos.z = tempPos.z + MoveSpeed.z;
			if(tempPos.z > NextPos.z)
				tempPos.z = tempPos.z - MoveSpeed.z;
			this.transform.position = tempPos;
		}
		
		if (this.transform.position == NextPos && TMonster 
		    || !translateY && this.transform.position.x == NextPos.x 
		    && this.transform.position.z == NextPos.z && TMonster) {//if same position go to next turn
			GameObject multi = GameObject.Find ("Multiplayer");
			if (multi != null)
			{
				MnextTurn();
				//if (multi != null) {
				//	NetworkView nv = GetComponent<NetworkView>();
				//	nv.RPC("MnextTurn", RPCMode.Others);
				//}
			}
			else
				nextTurn();
		}
	}
	[RPC]
	public void MnextTurn()
	{
		TMonster = false;
		ccamera.target = null;//stop camera panning
		GameMultiplayer.disableGrid = false;
		GameMultiplayer.disableCameraControl = false;
		//if(this.tag == "Player")
		GameMultiplayer.movesleft--;
		if(hasAnimation)
			anim.Play (idleName);
	}
	public void nextTurn()
	{
		TMonster = false;
		ccamera.target = null;//stop camera panning
		GameStart.disableGrid = false;
		GameStart.disableCameraControl = false;
		//if(this.tag == "Player")
		if(!notActuallyOver)
			GameStart.movesleft--;
		else
		{
			this.GetComponent<MonsterGrid> ().turnOver = false;
			notActuallyOver = false;
		}
		if(hasAnimation)
			anim.Play (idleName);
			//anim.Play("idle");
		if(hasAudio)
			audio.GetComponent<AudioSource>().Stop();
	}
	
	public void Stop()
	{
		TMonster = false;
		ccamera.target = null;//stop camera panning
		GameStart.disableGrid = false;
		GameStart.disableCameraControl = false;
		//GameStart.movesleft--;
		if (hasAnimation)
			anim.Play (idleName);
			//anim.Play ("idle");
		if(hasAudio)
			audio.GetComponent<AudioSource>().Stop();
	}
	[RPC]
	public void MStop()
	{
		TMonster = false;
		ccamera.target = null;//stop camera panning
		GameMultiplayer.disableGrid = false;
		GameMultiplayer.disableCameraControl = false;
		if(hasAnimation)
			anim.Play (idleName);
	}
}