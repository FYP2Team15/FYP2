using UnityEngine;
using System.Collections;

public class MonsterRaycast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	GameObject GetClickedGameObject()
	{
		// Builds a ray from camera point of view to the mouse position
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		// Casts the ray and get the first game object hit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			return hit.transform.gameObject;
		else
			return null;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject clickedGmObj = null;
		if(Input.GetMouseButtonDown(0)){
			clickedGmObj = GetClickedGameObject();
			if(clickedGmObj != null)
				Debug.Log(clickedGmObj.name);
			//display = true;
		}
	} 
}
