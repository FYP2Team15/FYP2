using UnityEngine;
using System.Collections;

public class MapTileSpawn : MonoBehaviour {
	public GameObject tile;
	public int HorizontalRange = 5;
	public int VerticallRange = 5;
	public int HorizontalOrVertical = 0;
	public bool HorizontalIsZero = true;
	public bool VerticalIsOne = true;
	public bool BoxIsTwo = true;
	public bool BoundaryIsThree = true;

	// Use this for initialization
	void Start () {
		switch(HorizontalOrVertical){
		case 0:
			GridGenerator.instance.GenerateH(this.name,tile, this.transform.position, HorizontalRange,0,this.transform);
			break;
		case 1:
			GridGenerator.instance.GenerateV(this.name,tile, this.transform.position, VerticallRange,0,this.transform);
			break;
		case 2:
			GridGenerator.instance.Generate(this.name,tile, this.transform.position, HorizontalRange,VerticallRange,this.transform);
			break;
		case 3:
			GridGenerator.instance.GenerateB(this.name,tile, this.transform.position, HorizontalRange,VerticallRange,this.transform);
			break;
		default:
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
