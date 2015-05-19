using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGrid : MonoBehaviour
{
	public GameObject tile;
	public int range = 5;
	public bool GridActive = false;
	//protected override void Init()
	//{
	//	GridInit();
	//}
	void OnMouseDown ()
	{
		if (GameStart.Player1()) {
						GameObject PTExist = GameObject.FindGameObjectWithTag ("PlayerTile");
						if (PTExist == null || GridCheck (PTExist.name)) {
								if (!GridActive)
										GridInit ();
								if (GridActive)
										GridDelete ();
						}
						GridActive = !GridActive;
				}
	}
	public void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name,tile, this.transform.position, range, range);

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