using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileModel : GridModel {
	
	public Player player = Player.None;
	public int index;
	public GridController controller;

	List<GameObject> withinRange = new List<GameObject> ();

	public void Selected ()
	{
		controller.view.SetState (ViewState.Selected);
		
		List<GameObject> withinRange = GridGenerator.instance.FetchArea (coord, 2);
		
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Possible ();
		}
	}
	
	public override void OnLeftClick ()
	{
		GameObject[] Monster = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject go in Monster ) {
			if(go != null)
			{
				MonsterGrid MonsterScript = go.GetComponent<MonsterGrid>();
				if(MonsterScript.GridCheck (this.name) && this.tag == "PlayerTile")
				{
					//Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.5f);
					//for (int i = 0; i < hitColliders.Length; i++) {
					//	if(hitColliders[i].name != "Terrain" || hitColliders[i].name != "Trigger")
					//	Debug.Log(hitColliders[i].name);
					//}
					MonsterScript.GridActive = false;
					MonsterScript.GridDelete ();
					Debug.Log (this.name);
					TranslateMonster TMonster = go.GetComponent<TranslateMonster>();
					GameStart.disableGrid = true;
					TMonster.Translate (true, this.transform.position);
				}
			}
		}
	}
	
	public void Possible ()
	{
		controller.view.SetState (ViewState.Possible);
		//show alpha   
	}
	
	public override void StartHover ()
	{
		controller.view.SetState (ViewState.Acceptable);
		// show full   
		//Debug.Log(coord.ToString());
		withinRange = GridGenerator.instance.FetchArea (coord, 1);//sbgame.range
		
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Possible ();
		}
	}
	
	public void SelectArea ()
	{
		controller.view.SetState (ViewState.Acceptable);
		withinRange = GridGenerator.instance.FetchArea (coord, 2);
		
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Possible ();
		}
	}
	
	public void SelectLine ()
	{
		controller.view.SetState (ViewState.Acceptable);
		withinRange = GridGenerator.instance.FetchLine (coord, 5);
		
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Possible ();
		}
	}
	
	public void SelectCone ()
	{
		controller.view.SetState (ViewState.Acceptable);
		withinRange = GridGenerator.instance.FetchCone (coord, 4);
		
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Possible ();
		}
	}
	
	public override void StopHover ()
	{
		controller.view.SetState (ViewState.Default);
		// go back to alpha, or Clear.
		foreach (GameObject go in withinRange) {
			go.GetComponent<TileModel> ().Clear ();
		}
	}
	
	public void Clear ()
	{
		controller.view.SetState (ViewState.Default);
	}
}
