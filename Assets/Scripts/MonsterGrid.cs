using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGrid : MonoBehaviour
{
	public GameObject tile;//normal tile
	public GameObject tile2;//collided tile
	public GameObject specialTile;//special tile
	public int range = 5;
	public bool GridActive = false;
	public bool turnOver = false;
	int rightClickState = 0; 
	public GameObject specialTarget = null;//special target
	private Vector3 stoPosition = new Vector3(0,0,0) ;//special target original pos
	//protected override void Init()
	//{
	//	GridInit();
	//}
	void Start () {
		GridActive = false;
		turnOver = false;
	}
	void OnMouseDown ()
	{
		if (GameStart.Player1() && this.tag == "Player" && !turnOver) {
						GameObject PTExist = GameObject.FindGameObjectWithTag ("PlayerTile");
 						if (PTExist == null || GridCheck (PTExist.name)) {
								if (!GridActive && !GameStart.disableGrid)
								{
										if(this.GetComponent<Preview>().GridActive)
											this.GetComponent<Preview>().GridDelete();
										GridInit ();
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
	void OnMouseOver () {
		if (Input.GetMouseButton(1)) {
			if(GameStart.Player1() && this.tag == "Player" && !turnOver && rightClickState == 0)
			{
				Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1);//get gameobjects right beside this object
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
		else 
		{
			rightClickState = 0;
		}
	}
	public void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name,tile,tile2, this.transform.position, range, range);

	}

	public void GridInitSpecial ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name,specialTile,tile2, this.transform.position, range, range);
		
	}

	public void GridDelete ()
	{
		for (int x = 0; x < (range*4-3); x++)
		{
				string tile = this.name + "_T"  + (x);
				GridGenerator.instance.Delete (tile);
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
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