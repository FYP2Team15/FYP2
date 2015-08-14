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
	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	public override void OnLeftClick ()
	{
		Interact ();
	}
	#endif	

	#if UNITY_ANDROID
	public override void OnTouch ()
	{
		Interact ();
	}
	#endif	

	void Interact ()
	{
		if (GameObject.Find ("Multiplayer") != null) 
		{
			if(this.tag == "PlayerTile" || this.tag == "EnemyTile")
			{
				MMonsterGrid MonsterScript = this.transform.parent.gameObject.GetComponent<MMonsterGrid>();
				MonsterScript.GridActive = false;
				MonsterScript.GridDelete ();
				TranslateMonster TMonster = this.transform.parent.gameObject.GetComponent<TranslateMonster>();
				GameMultiplayer.disableGrid = true;
				TMonster.Translate (true, this.transform.position-TMonster.GetComponent<MMonsterGrid>().Offset);
			}
			if(this.tag == "SpecialTile")
			{
				MMonsterGrid MonsterScript = this.transform.parent.gameObject.GetComponent<MMonsterGrid>();
				
				MonsterScript.GridActive = false;
				MonsterScript.GridDelete ();
				TranslateMonster TMonster = MonsterScript.specialTarget.GetComponent<TranslateMonster>();
				GameMultiplayer.disableGrid = true;
				TMonster.Translate (true, this.transform.position-TMonster.GetComponent<MMonsterGrid>().Offset);
				MonsterScript.specialTarget = null;
			}
		}
		else if(this.tag == "PlayerTile")
		{
			MonsterGrid MonsterScript = this.transform.parent.gameObject.GetComponent<MonsterGrid>();
			
			MonsterScript.GridActive = false;
			MonsterScript.GridDelete ();
			TranslateMonster TMonster = this.transform.parent.gameObject.GetComponent<TranslateMonster>();
			GameStart.disableGrid = true;
			TMonster.Translate (true, this.transform.position-TMonster.GetComponent<MonsterGrid>().Offset);
		}
		else if(this.tag == "SpecialTile")
		{
			MonsterGrid MonsterScript = this.transform.parent.gameObject.GetComponent<MonsterGrid>();
			
			MonsterScript.GridActive = false;
			MonsterScript.GridDelete ();
			TranslateMonster TMonster = MonsterScript.specialTarget.GetComponent<TranslateMonster>();
			GameStart.disableGrid = true;
			TMonster.Translate (true, this.transform.position-TMonster.GetComponent<MonsterGrid>().Offset, true);
			MonsterScript.specialTarget = null;
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
