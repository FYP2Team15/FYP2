using UnityEngine;
using System.Collections;

public class TranslateMonster : MonoBehaviour {
	public bool TMonster = false;
	public Vector3 NextPos;
	GameObject camera;
	CameraControl ccamera;
	// Use this for initialization
	void Start () {
		camera = GameObject.Find ("Camera");
		ccamera = camera.GetComponent <CameraControl> ();
	}
	public void Translate(bool T, Vector3 Pos)
	{
		TMonster = T;
		NextPos = Pos;
	}
	// Update is called once per frame
	void Update () {
		if(TMonster)
		{
			ccamera.target = this.transform;
			Vector3 tempPos = this.transform.position;
			if(tempPos.x < NextPos.x && tempPos.x+1 > NextPos.x)
				tempPos.x = NextPos.x;
			if(tempPos.z < NextPos.z && tempPos.z+1 > NextPos.z)
				tempPos.z = NextPos.z;
			if(tempPos.x < NextPos.x)
				tempPos.x = tempPos.x + 0.05f;
			if(tempPos.x > NextPos.x)
				tempPos.x = tempPos.x - 0.05f;
			if(tempPos.z < NextPos.z)
				tempPos.z = tempPos.z + 0.05f;
			if(tempPos.z > NextPos.z)
				tempPos.z = tempPos.z - 0.05f;
			this.transform.position = tempPos;
		}
		if (this.transform.position == NextPos && TMonster) {
			TMonster = false;
			ccamera.target = null;
			GameStart.disableGrid = false;
			//if(this.tag == "Player")
				GameStart.movesleft--;
		}
	}
}
