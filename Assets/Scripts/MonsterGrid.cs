using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterGrid : MonoSingleton<MonsterGrid>
{
	public GameObject tile;
	public int range = 5;

	protected override void Init()
	{
		GridInit();
	}
	public void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (tile, this.transform.position, range, range);
	}
	public void GridDelete ()
	{
		GridGenerator.instance.Clear ();
	}
}