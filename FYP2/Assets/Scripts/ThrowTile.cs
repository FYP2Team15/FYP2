using UnityEngine;
using System.Collections;

public class ThrowTile : TileModel {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnLeftClick (){
		transform.parent.GetComponent<ThrowObject>().
		Throw(this.transform.position-transform.parent.GetComponent<ThrowObject>().Offset);
	}
}
