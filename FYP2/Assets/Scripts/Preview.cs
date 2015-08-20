using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour {

	public GameObject tile;
	public GameObject tile2;
	public int range = 5;		//player movement range
	[HideInInspector]public bool GridActive = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseEnter ()//when mouse hover over
	{
		if(GameObject.Find("Multiplayer") != null && !GameMultiplayer.disableGrid && Time.timeScale > 0 && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled)
			GridInitM ();
		else if(GameObject.Find("Multiplayer") == null && !GameStart.disableGrid && Time.timeScale > 0 && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled)//only draw when grid is enabled
			GridInit ();
	}
	void OnMouseExit ()//when mouse hover out
	{
		if(GridActive)
		GridDelete ();
	}
	#endif	
	void GridInitM ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		switch(this.GetComponent<MMonsterGrid>().type)
		{
			case 0:
				GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MMonsterGrid>().Offset, range, range,this.GetComponent<MMonsterGrid>().Obstacles,this.transform);
				break;
			case 1:
				GridGenerator.instance.GenerateSq (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MMonsterGrid>().Offset, range, range,this.GetComponent<MMonsterGrid>().Obstacles,this.transform);
				break;
			default:
				GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MMonsterGrid>().Offset, range, range,this.GetComponent<MMonsterGrid>().Obstacles,this.transform);
				break;
		}
		GridActive = true;
		if(this.tag == "Player")
			this.GetComponent<PlayerHealth>().displayStat = true;
		if(this.tag == "EnemyMonster")
			this.GetComponent<EnemyHealth>().displayStat = true;
	}
	void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		switch(this.GetComponent<MonsterGrid>().type)
		{
			case 0:
				GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MonsterGrid>().Offset, range, range,this.GetComponent<MonsterGrid>().Obstacles,this.transform);
				break;
			case 1:
				GridGenerator.instance.GenerateSq (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MonsterGrid>().Offset, range, range,this.GetComponent<MonsterGrid>().Obstacles,this.transform);
				break;
			default:
				GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MonsterGrid>().Offset, range, range,this.GetComponent<MonsterGrid>().Obstacles,this.transform);
				break;
		}
		//GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position+this.GetComponent<MonsterGrid>().Offset, range, range,this.GetComponent<MonsterGrid>().Obstacles,this.transform);
		GridActive = true;
		if(this.tag == "Player")
			this.GetComponent<PlayerHealth>().displayStat = true;
		if(this.tag == "EnemyMonster")
			this.GetComponent<EnemyHealth>().displayStat = true;
	}
	public void GridDelete ()
	{
		for (int x = 0; x < (range*range); x++)
		{
			string tile = this.name + "_p_T"  + (x);
			GridGenerator.instance.Delete (tile);
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
		GridActive = false;
		if(this.tag == "Player")
			this.GetComponent<PlayerHealth>().displayStat = false;
		if(this.tag == "EnemyMonster")
			this.GetComponent<EnemyHealth>().displayStat = false;
	}
}
