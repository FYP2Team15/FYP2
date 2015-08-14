using UnityEngine;
using System.Collections;

public class TouchCamera : MonoBehaviour {
	public CheckGrid CG;
	public Vector3 MinimumConstraint =  new Vector3(0,0,0);
	public Vector3 MaximumConstraint =  new Vector3(0,0,0);
	bool Constraint = true;
	bool Multiplayer = false;
	void Start () {
		if (GameObject.Find ("Multiplayer") != null)
			Multiplayer = true;
		if(MinimumConstraint == MaximumConstraint)
		{
			Constraint = false;
		}
	}
	void TranslateCamera(Vector3 NextPos){
		if (Constraint)
			NextPos = ConstraintPos (NextPos);
		transform.position = NextPos;
	}
	void Update () {
		//if (CG.PlayPause == true) {
		if (Multiplayer)
		{
			if (!GameMultiplayer.disableCameraControl)
				CameraControl ();
		}
		else if (!GameStart.disableCameraControl)
			CameraControl ();
		//}else if (CG.PlayPause == false) {
		//	Time.timeScale = 0;
		//}
	}

	void CameraControl()
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetMouseButton (0) && Time.timeScale > 0) {// && !monsterSel) {	
			Vector3 currPos = transform.position;
			Vector3 currMouseX = new Vector3 (Input.GetAxis ("Mouse X"), 0, Input.GetAxis ("Mouse X"));
			Vector3 currMouseY = new Vector3 (Input.GetAxis ("Mouse Y"), 0, -Input.GetAxis ("Mouse Y"));
			//Debug.Log(Input.GetAxis ("Mouse Y"));
			Vector3 NextPos = currPos + currMouseX - currMouseY;
			TranslateCamera(NextPos);
			
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && transform.position.y <= MinimumConstraint.y
		    || Input.GetAxis ("Mouse ScrollWheel") > 0 && transform.position.y >= MaximumConstraint.y) {
		} else{
			transform.position += new Vector3 (0, Input.GetAxis ("Mouse ScrollWheel"), 0);
			//TO DO TOUCH VERSION
		} 
		#endif
		#if UNITY_IOS
		if (Input.touchCount == 1) {
			Touch currentTouch = Input.GetTouch(0);
			Vector3 currPos,startTouchX,startTouchY;
			currPos = startTouchX = startTouchY = new Vector3(0,0,0);
			if (currentTouch.phase == TouchPhase.Began) {
				currPos = transform.position;
				startTouchX = new Vector3 (currentTouch.position.x, 0, currentTouch.position.x);
				startTouchY = new Vector3 (currentTouch.position.y, 0, -currentTouch.position.y);
			}
			
			if (currentTouch.phase == TouchPhase.Moved) {
				Vector3 currTouchX = new Vector3 (currentTouch.position.x, 0, currentTouch.position.x);
				Vector3 currTouchY = new Vector3 (currentTouch.position.y, 0, -currentTouch.position.y);
				Vector3 NextPos = currPos + (currTouchX - startTouchX) - (currTouchY - startTouchY);
				TranslateCamera(NextPos);
			}
		}
		if (Input.touchCount >= 2) {
			Touch currentTouch = Input.GetTouch(0);
			Touch currentTouch2 = Input.GetTouch(1);
			float CurrentTouchDistance = 0;
			if (currentTouch.phase == TouchPhase.Began || currentTouch.phase == TouchPhase.Began) {
				CurrentTouchDistance = Vector2.Distance(currentTouch.position, currentTouch2.position);
			}
			
			if (currentTouch.phase == TouchPhase.Moved || currentTouch2.phase == TouchPhase.Moved) {
				float NextTouchDistance = Vector2.Distance(currentTouch.position, currentTouch2.position);
				transform.position += new Vector3 (0,(CurrentTouchDistance-NextTouchDistance)/100, 0);
				CurrentTouchDistance = NextTouchDistance;
			}
		} 
		#endif
	}
	Vector3 ConstraintPos(Vector3 Pos){
		if(Pos.x < MinimumConstraint.x)
			Pos.x = MinimumConstraint.x;
		if(Pos.x > MaximumConstraint.x)
			Pos.x = MaximumConstraint.x;
		if(Pos.z < MinimumConstraint.z)
			Pos.z = MinimumConstraint.z;
		if(Pos.z > MaximumConstraint.z)
			Pos.z = MaximumConstraint.z;
		return Pos;
	}
	
}
