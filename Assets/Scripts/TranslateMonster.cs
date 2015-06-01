using UnityEngine;
using System.Collections;

public class TranslateMonster : MonoBehaviour {
	public bool TMonster = false;
	public Vector3 NextPos;
	GameObject camera;
	CameraControl ccamera;
	Animation anim;
	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Camera");
		ccamera = camera.GetComponent <CameraControl> ();
		anim = GetComponent<Animation>();
	}
	public void Translate(bool T, Vector3 Pos)
	{
		TMonster = T;
		NextPos = Pos;
		anim.Play ("walk-cicle");
		this.GetComponent<MonsterGrid> ().turnOver = true;
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
				tempPos.x = tempPos.x + 0.05f;
			if(tempPos.x > NextPos.x)
				tempPos.x = tempPos.x - 0.05f;
			if(tempPos.y < NextPos.y)//translate y
				tempPos.y = tempPos.y + 0.02f;
			if(tempPos.y > NextPos.y)
				tempPos.y = tempPos.y - 0.02f;
			if(tempPos.z < NextPos.z)//translate z
				tempPos.z = tempPos.z + 0.05f;
			if(tempPos.z > NextPos.z)
				tempPos.z = tempPos.z - 0.05f;
			this.transform.position = tempPos;
		}
		if (this.transform.position == NextPos && TMonster) {//if same position go to next turn
			nextTurn();
		}
	}
	public void nextTurn()
	{
		TMonster = false;
		ccamera.target = null;//stop camera panning
		GameStart.disableGrid = false;
		//if(this.tag == "Player")
		GameStart.movesleft--;
		anim.Play ("idle");
	}
}
