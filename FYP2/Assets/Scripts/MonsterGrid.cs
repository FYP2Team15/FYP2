using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGrid : MonoBehaviour
{
	public GameObject tile;//normal tile
	public GameObject tile2;//collided tile
	public GameObject tile3;//combined tile
	public GameObject specialTile;//special tile
	CheckGrid CG;
	public bool PlPaused;
	public string[] Obstacles;
	public string[] SpecialException;
	private string[] SpecialObstacles;
	public bool hasSTile = true;
	public int range = 5;	//Player movement range
	public int type = 1;//0 = t shape (Cross Shape), 1 = square shape( X Shape)
	public Vector3 Offset = new Vector3(0,0,0);
	[HideInInspector]public bool GridActive = false;
	[HideInInspector]public bool turnOver = false;
	[HideInInspector]public bool throwOver = false;
	int rightClickState = 0; 
	[HideInInspector]public GameObject specialTarget = null;//special target
	private Vector3 stoPosition = new Vector3(0,0,0) ;//special target original pos
	public int MonsterType = 1;//1 is throw monster, 2 is archer
	//protected override void Init()
	//{
	//	GridInit();
	//}
	void Start () {
		GridActive = false;
		turnOver = false;
		PlPaused = false;
		//type = 1;
		if (SpecialException.Length > 0) {
			SpecialObstacles = new string[Obstacles.Length];
			int k = 0;
			for (int i = 0; i < Obstacles.Length; i++) {
				bool remove = false;
				for (int j = 0; j < SpecialException.Length; j++) {
					if (Obstacles [i] == SpecialException [j]) {
						remove = true;
					}
					if (remove)
						break;
				}
				if (!remove) {
					SpecialObstacles [k] = Obstacles [i];
					k++;
				}
			}
			int SOlength = 0;
			bool stop = false;
			for (int i = 0; i < SpecialObstacles.Length; i++) {
				if (SpecialObstacles [i] != null)
					SOlength++;
				else
					break;
			}
			string[] TSObstacle = new string[SOlength];
			for (int i = 0; i < TSObstacle.Length; i++) {
				TSObstacle [i] = SpecialObstacles [i];
			}
			SpecialObstacles = TSObstacle;
		}
		else
		{
			SpecialObstacles = Obstacles;
		}
	}
	void CreateGrid()
	{
		if (GameStart.Player1 () && this.tag == "Player" && !turnOver  && Time.timeScale > 0) {
			GameObject PTExist = GameObject.FindGameObjectWithTag ("PlayerTile");
			if (PTExist == null || GridCheck (PTExist.name)) {
				if (!GridActive && !GameStart.disableGrid) {
					if (this.GetComponent<Preview> ().GridActive)
						this.GetComponent<Preview> ().GridDelete ();
					GridInit ();
					GameStart.disableGrid = true;
				}
				if (GridActive) {
					GridDelete ();
					GameStart.disableGrid = false;
				}
				GridActive = !GridActive;
			}
			
			
		}
	}

	#if UNITY_ANDROID
	void OnTouchDown ()
	{
		CreateGrid ();
		
	}
	#endif

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	void OnMouseDown ()
	{
		if(!GameObject.Find("BattleCamera").GetComponent<Camera>().enabled)
			CreateGrid ();
	}

	void OnMouseOver () {
		if (Input.GetMouseButton(1) && hasSTile  && Time.timeScale > 0) {
			if (MonsterType == 1)
				ThrowAlly();
			else if (MonsterType == 2)
				Archer();
		}
		else 
		{
			rightClickState = 0;
			//specialTarget = null;
		}
	}

	void ThrowAlly()
	{
		if(GameStart.Player1() && this.tag == "Player" && !throwOver && rightClickState == 0 )
		{
			if(!GameStart.disableGrid)
			{
				Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 0.7f);//get gameobjects right beside this object
				foreach(Collider obj in hitColliders)
				{
					if(obj.tag == "Player" && obj.gameObject != this.gameObject && specialTarget == null && obj.GetComponent<ThrowObject>() == null)
					{
						specialTarget = obj.gameObject;
						stoPosition = specialTarget.transform.position;
						Vector3 newPos = this.transform.position+new Vector3(0,1,0);
						specialTarget.transform.position = newPos;
						break;
					}
				}
			}
			if (specialTarget != null) {
				rightClickState = 1;
				GameObject PTExist = GameObject.FindGameObjectWithTag ("SpecialTile");
				if (PTExist == null || GridCheck (PTExist.name)) {
					if (!GridActive && !GameStart.disableGrid)
					{
						if(this.GetComponent<Preview>().GridActive)
							this.GetComponent<Preview>().GridDelete();
						GridInitSpecial ();
						GameStart.disableGrid = true;
					}
					if (GridActive)
					{
						GridDelete ();
						specialTarget.transform.position = stoPosition;
						specialTarget = null;
						GameStart.disableGrid = false;
					}
					GridActive = !GridActive;
				}
			}
		}
	}

	void Archer()
	{
		if(GameStart.Player1() && this.tag == "Player" && !throwOver && rightClickState == 0 )
		{
			rightClickState = 1;
			GameObject PTExist = GameObject.FindGameObjectWithTag ("ArcherTile");
			if (PTExist == null || GridCheck (PTExist.name)) {
				if (!GridActive && !GameStart.disableGrid)
				{
					if(this.GetComponent<Preview>().GridActive)
						this.GetComponent<Preview>().GridDelete();
					GridInitSpecial ();
					GameStart.disableGrid = true;
				}
				if (GridActive)
				{
					GridDelete ();
					GameStart.disableGrid = false;
				}
				GridActive = !GridActive;
			}
		}
	}
	#endif	
	public void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		switch(type)
		{
			case 0:
				GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform,false,tile3,GetComponent<CheckGrid>().nameLength);
				break;
			case 1:
				GridGenerator.instance.GenerateSq (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform,false,tile3,GetComponent<CheckGrid>().nameLength);
				break;
			default:
				GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform,false,tile3,GetComponent<CheckGrid>().nameLength);
				break;
		}
		if(this.tag == "Player")
			this.GetComponent<PlayerHealth>().displayStat = true;
		if(this.tag == "EnemyMonster")
			this.GetComponent<EnemyHealth>().displayStat = true;
	}

	public void GridInitSpecial ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		switch(type)
		{
			case 0:
				GridGenerator.instance.GenerateT (this.name,specialTile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
				break;
			case 1:
				GridGenerator.instance.GenerateSq (this.name,specialTile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
				break;
			default:
				GridGenerator.instance.GenerateT (this.name,specialTile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
				break;
		}
	}

	public void GridDelete ()
	{
		switch(type)
		{
		case 0:
			for (int x = 0; x < (range*4-3); x++)
			{
				string tile = this.name + "_T"  + (x);
				GridGenerator.instance.Delete (tile);
			}
			break;
		case 1:
			for (int x = 0; x < (range*range); x++)
			{
				string tile = this.name + "_T"  + (x);
				GridGenerator.instance.Delete (tile);
			}
			break;
		default:
			for (int x = 0; x < (range*4-3); x++)
			{
				string tile = this.name + "_T"  + (x);
				GridGenerator.instance.Delete (tile);
			}
			break;
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
		if(this.tag == "Player")
			this.GetComponent<PlayerHealth>().displayStat = false;
		if(this.tag == "EnemyMonster")
			this.GetComponent<EnemyHealth>().displayStat = false;
	}
	public bool GridCheck (string name)
	{
		for (int x = 0; x < (range*4-3); x++)
		{
			string tile = this.name + "_T" + (x);
			if(name == tile)
				return true;
		}
		return false;
	}
}