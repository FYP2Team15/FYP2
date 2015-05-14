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
		Debug.Log (this.name);
		if(!GridActive)
			GridInit();
		if(GridActive)
			GridDelete();
		GridActive = !GridActive;
	}
	public void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name,tile, this.transform.position, range, range);
	}
	public void GridDelete ()
	{
		for (int x = 0; x < (range*2-1); x++) {
			for (int y = 0; y < (range*2-1); y++)
			{
				string tile = this.name + (x+y);
				GridGenerator.instance.Delete (tile);
			}
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
	}
	public bool GridCheck (string name)
	{
		for (int x = 0; x < (range*2-1); x++) {
			for (int y = 0; y < (range*2-1); y++)
			{
				string tile = this.name + (x+y);
				if(name == tile)
					return true;
			}
		}
		return false;
	}
}