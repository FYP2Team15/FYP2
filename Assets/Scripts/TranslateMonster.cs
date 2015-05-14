using UnityEngine;
using System.Collections;

public class TranslateMonster : MonoBehaviour {
	public bool TMonster = false;
	public Vector3 NextPos;
	// Use this for initialization
	void Start () {
	
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
			Vector3 tempPos = this.transform.position;
			if(tempPos.x < NextPos.x && tempPos.x+1 > NextPos.x)
				tempPos.x = NextPos.x;
			if(tempPos.z < NextPos.z && tempPos.z+1 > NextPos.z)
				tempPos.z = NextPos.z;
			if(tempPos.x < NextPos.x)
				tempPos.x = tempPos.x + 1;
			if(tempPos.x > NextPos.x)
				tempPos.x = tempPos.x - 1;
			if(tempPos.z < NextPos.z)
				tempPos.z = tempPos.z + 1;
			if(tempPos.z > NextPos.z)
				tempPos.z = tempPos.z - 1;
			this.transform.position = tempPos;
		}
		if (this.transform.position == NextPos)
						TMonster = false;
	}
}
