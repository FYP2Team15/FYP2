using UnityEngine;
using System.Collections;

public class CameraEnemy : MonoBehaviour {

	//static GameObject TargetObj = null;
	static GameObject camera;
	static GameObject[]EnemyMonster;
	static int currenttarget = -1;

	private static CameraControl Pcamera;

	bool Multiplayer = false;
	void Start () {
		if (GameObject.Find ("Multiplayer") != null)
			Multiplayer = true;
		Init ();
	}

	public static void Init (){
		camera = GameObject.Find ("Camera");
		Pcamera = camera.GetComponent <CameraControl> ();
		EnemyMonster = GameObject.FindGameObjectsWithTag ("EnemyMonster");
		currenttarget = -1;
	}
	void NextTarget(){
		if (currenttarget >= EnemyMonster.Length - 1) {
			currenttarget = 0;
		} else {
			currenttarget += 1;
		}
		GameStart.currentPan = 2;
		if(EnemyMonster[currenttarget] == null)
			Init ();
		else
			Pcamera.target = EnemyMonster [currenttarget].transform;
	}

	void PrevTarget(){
		if (currenttarget <= 0) {
			currenttarget = EnemyMonster.Length - 1;
		} else {
			currenttarget -= 1;
		}
		GameStart.currentPan = 2;
		if(EnemyMonster[currenttarget] == null)
			Init ();
		else
			Pcamera.target = EnemyMonster [currenttarget].transform;

	}
	// Update is called once per frame
	void Update () {
		if (Multiplayer)
		{
			if (!GameMultiplayer.disableCameraControl)
				CameraControl ();
		}
		else if (!GameStart.disableCameraControl)
			CameraControl ();
	}
	void CameraControl()
	{
		if (currenttarget > -1 && currenttarget < EnemyMonster.Length && GameStart.currentPan == 2) {
			if(EnemyMonster[currenttarget] == null)
				Init ();
			else if (Pcamera.transform.position == EnemyMonster [currenttarget].transform.position + Pcamera.offset) 
				Pcamera.target = null;
		}
		if(!Multiplayer)
		{
			if(EnemyMonster.Length > 0 && !GameStart.getCameraPan())
			{
				#if UNITY_EDITOR || UNITY_STANDALONE_WIN
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					NextTarget();	
				}
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					PrevTarget();
				}
				#endif
			}
		}
		else if(EnemyMonster.Length > 0)
		{
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				NextTarget();	
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				PrevTarget();
			}
			#endif
		}
	}

}
