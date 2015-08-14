using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public Transform target = null;
	public Vector3 offset = new Vector3(3,5,-3);
	private float time;
	private Vector3 desired;
	public Vector3 normalRotation = new Vector3(45,315,0);

	void Update () {
		if (target != null) {
			normalPan();
		}
	}

	void normalPan()
	{
		Vector3 newDesired = target.position;
		if (desired != newDesired) {
			time = 0;
			desired = newDesired;
		}
		time += Time.deltaTime * 5;
		Vector3 lerpTarget = Vector3.Lerp (transform.position - offset, desired, time);
		transform.position = lerpTarget + offset;
		transform.LookAt (lerpTarget);
	}

	public void SetTarget(Transform t) {
		enabled = true;
		target = t;
	}
}
