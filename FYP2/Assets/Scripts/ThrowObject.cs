using UnityEngine;
using System.Collections;

public class ThrowObject : MonoBehaviour {
	public GameObject tile1;
	public GameObject tile2;
	public Vector3 Offset = new Vector3(0,0,0);
	public int range = 5;
	public string[] Targets;
	public GameObject thrownObject;
	int rightClickState = 0;
	bool Multiplayer = false;
	// Use this for initialization
	public bool hasAudio = false;
	GameObject audio = new GameObject();
	public AudioClip translateSound;

	void Start () {
		audio = GameObject.Find ("Sfx");
		if (audio == null)
			hasAudio = false;
		if(GameObject.Find("Multiplayer") != null)
		{
			Multiplayer = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseOver () {
		if (Input.GetMouseButton(1) ) {
			if(Multiplayer && rightClickState == 0)
				multi();
			else if(rightClickState == 0)
				single();
			rightClickState = 1;
		}
		else 
		{
			rightClickState = 0;
			//specialTarget = null;
		}
	}
	#endif	

	void multi()
	{
		if(GameMultiplayer.Player1() && Network.isServer && this.tag == "Player" && !this.GetComponent<MMonsterGrid>().turnOver && rightClickState == 0 ||
		   GameMultiplayer.Player2() && Network.isClient && this.tag == "EnemyMonster" && !this.GetComponent<MMonsterGrid>().turnOver && rightClickState == 0 )
		{
			
			if(this.GetComponent<Preview>().GridActive)
				this.GetComponent<Preview>().GridDelete();
			GridInitSpecial ();
			GameMultiplayer.disableGrid = true;
			
			if (this.GetComponent<MMonsterGrid>().GridActive)
			{
				this.GetComponent<MMonsterGrid>().GridDelete ();
				GameMultiplayer.disableGrid = false;
			}
			this.GetComponent<MMonsterGrid>().GridActive = !this.GetComponent<MMonsterGrid>().GridActive;
		}
	}
	void single()
	{
		if(GameStart.Player1() && this.tag == "Player" && !this.GetComponent<MonsterGrid>().throwOver && rightClickState == 0 )
		{
			if(this.GetComponent<Preview>().GridActive)
				this.GetComponent<Preview>().GridDelete();
			GridInitSpecial ();
			GameStart.disableGrid = true;
			
			if (this.GetComponent<MonsterGrid>().GridActive)
			{
				this.GetComponent<MonsterGrid>().GridDelete ();
				GameStart.disableGrid = false;
			}
			this.GetComponent<MonsterGrid>().GridActive = !this.GetComponent<MonsterGrid>().GridActive;
		}
	}
	public void GridInitSpecial ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name,tile1,tile2, this.transform.position+Offset, range, range,Targets,this.transform, true);
		
	}
	public void Throw(Vector3 Pos)
	{
		if(hasAudio)
			audio.GetComponent<AudioScript>().playOnceCustom(translateSound);
		GridDelete ();
		GameObject go1 = Instantiate (thrownObject, this.transform.position, Quaternion.identity) as GameObject;
		go1.name = name + "_T";
		this.GetComponent<MonsterGrid> ().throwOver = true;
		go1.transform.SetParent(this.transform);
		go1.GetComponent<TranslateMonster>().Translate(true, Pos-go1.GetComponent<MonsterGrid>().Offset);
	}

	public void GridDelete ()
	{
		for (int x = 0; x < (range*4-3); x++)
		{
			string tile = this.name + "_T"  + (x);
			GridGenerator.instance.Delete (tile);
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
		GameStart.disableGrid = false;
		this.GetComponent<MonsterGrid> ().GridActive = false;
	}
}
