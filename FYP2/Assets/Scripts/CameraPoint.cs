using UnityEngine;
using System.Collections;

public class CameraPoint : MonoBehaviour {
	
	static GameObject camera;//get camera
	static GameObject[]player;//array used to store player monster
	static int currenttarget = -1;// current panned target, -1 denotes no target


	private static CameraControl Pcamera;


	bool Multiplayer = false;
	void Start () {
		if (GameObject.Find ("Multiplayer") != null)
			Multiplayer = true;
		  Init ();
	}


	public static void Init ()
	{
		camera = GameObject.Find ("Camera");//get camera
		Pcamera = camera.GetComponent <CameraControl> ();//get cameracontrol class
		player = GameObject.FindGameObjectsWithTag ("Player");//put all players into array
		currenttarget = -1;
	}

	void NextTarget(){
		if ( currenttarget >= player.Length-1)
		{
			currenttarget=0;//change target
		}
		else
		{
			currenttarget += 1;//change target
		}
		GameStart.currentPan = 1;
		if(player [currenttarget] == null)//if target is destroyed
			Init ();
		else
			Pcamera.target = player [currenttarget].transform;//set target to cameracontrol
	}

	void PrevTarget(){
		if ( currenttarget <= 0)
		{
			currenttarget=player.Length-1;//change target
		}
		else
		{
			currenttarget -=1;//change target
		}
		GameStart.currentPan = 1;
		if(player [currenttarget] == null)//if target is destroyed
			Init ();
		else
			Pcamera.target = player [currenttarget].transform;//set target to cameracontrol
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
		if (currenttarget > -1 && currenttarget < player.Length  && GameStart.currentPan == 1) {
			if(player [currenttarget] == null)//check if current target is deleted
				Init ();//re init
			else if (Pcamera.transform.position == player [currenttarget].transform.position + Pcamera.offset) //check if camera pan is finished
				Pcamera.target = null;//stop camera pan
		}
		if(!Multiplayer)
		{
			if(player.Length > 0 && !GameStart.getCameraPan())
			{
				#if UNITY_EDITOR || UNITY_STANDALONE_WIN
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					NextTarget();	
				}
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					PrevTarget();
				}
				#endif
			}
		}
		else if(player.Length > 0)
		{
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				NextTarget();	
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				PrevTarget();
			}
			#endif
		}
	}
}
