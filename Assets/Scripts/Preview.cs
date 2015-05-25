using UnityEngine;
using System.Collections;

public class Preview : MonoBehaviour {

	public GameObject tile;
	public GameObject tile2;
	public int range = 5;
	public bool GridActive = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseEnter ()//when mouse hover over
	{
		if(!GameStart.disableGrid)//only draw when grid is enabled
			GridInit ();
	}
	void OnMouseExit ()//when mouse hover out
	{
		if(GridActive)
		GridDelete ();
	}
	void GridInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.GenerateT (this.name+"_p",tile,tile2, this.transform.position, range, range);
		GridActive = true;
		
	}
	public void GridDelete ()
	{
		for (int x = 0; x < (range*4-3); x++)
		{
			string tile = this.name + "_p_T"  + (x);
			GridGenerator.instance.Delete (tile);
		}
		GridGenerator.instance.tiles.RemoveAll(item => item == null);
		GridActive = false;
	}
}
