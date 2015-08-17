using UnityEngine;
using System.Collections;

public class MMonsterGrid : MonoBehaviour {
	public GameObject tile;//normal tile
	public GameObject tile2;//collided tile
	public GameObject specialTile;//special tile
	public string[] Obstacles;
	public bool hasSTile = true;
	public int range = 5;
	public int type = 1;//0 = t shape, 1 = square shape
	public Vector3 Offset = new Vector3(0,0,0);
	[HideInInspector]public bool GridActive = false;
	[HideInInspector]public bool turnOver = false;
	int rightClickState = 0; 
	[HideInInspector]public GameObject specialTarget = null;//special target
	private Vector3 stoPosition = new Vector3(0,0,0) ;//special target original pos
	//protected override void Init()
	//{
	//	GridInit();
	//}
	void Start () {
		GridActive = false;
		turnOver = false;
	}
	void CreateGrid()
	{
		if (GameMultiplayer.Player1() && Network.isServer && this.tag == "Player" && !turnOver ||
		    GameMultiplayer.Player2() && Network.isClient && this.tag == "EnemyMonster" && !turnOver ) {
			GameObject PTExist = GameObject.FindGameObjectWithTag (tile.tag);
			if (PTExist == null || GridCheck (PTExist.name)) {
				if (!GridActive && !GameMultiplayer.disableGrid)
				{
					if(this.GetComponent<Preview>().GridActive)
						this.GetComponent<Preview>().GridDelete();
					GridInit ();
					GameMultiplayer.disableGrid = true;
				}
				if (GridActive)
				{
					GridDelete ();
					GameMultiplayer.disableGrid = false;
				}
				GridActive = !GridActive;
			}
		}
	}

	#if UNITY_ANDROID
	void OnTouch ()
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
		if (Input.GetMouseButton(1) && hasSTile) {
			if(GameMultiplayer.Player1() && Network.isServer && this.tag == "Player" && !turnOver && rightClickState == 0 ||
			   GameMultiplayer.Player2() && Network.isClient && this.tag == "EnemyMonster" && !turnOver && rightClickState == 0 )
			{
				if(!GameMultiplayer.disableGrid)
				{
					Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 0.7f);//get gameobjects right beside this object
					foreach(Collider obj in hitColliders)
					{
						if(obj.tag == "Player" && obj.gameObject != this.gameObject && specialTarget == null)
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
						if (!GridActive && !GameMultiplayer.disableGrid)
						{
							if(this.GetComponent<Preview>().GridActive)
								this.GetComponent<Preview>().GridDelete();
							GridInitSpecial ();
							GameMultiplayer.disableGrid = true;
						}
						if (GridActive)
						{
							GridDelete ();
							specialTarget.transform.position = stoPosition;
							specialTarget = null;
							GameMultiplayer.disableGrid = false;
						}
						GridActive = !GridActive;
					}
				}
			}
		}
		else 
		{
			rightClickState = 0;
			//specialTarget = null;
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
			GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
			break;
		case 1:
			GridGenerator.instance.GenerateSq (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
			break;
		default:
			GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
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
			GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position+Offset, range, range,Obstacles,this.transform);
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
