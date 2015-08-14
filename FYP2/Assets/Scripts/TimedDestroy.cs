using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
	float timerDestroy;
	public float UpTime = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timerDestroy += Time.deltaTime;
		if(timerDestroy >= UpTime)
			Destroy(this.gameObject);
	}
}
