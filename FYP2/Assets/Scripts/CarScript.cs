using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour {
	public bool Multiplayer = false;
	public GameObject tile;
	public int range = 15;
	public int CarSize = 2;
	public Vector3 Offset;
	[HideInInspector]public int passengerAmt = 0;
	int rightClickState = 0;
	Vector3[] passengerSize;
	public Vector3[] passengerSpawn;
	GameObject[] passengers;
	GameObject currentTargetPassenger = new GameObject();

	void Start () {
		currentTargetPassenger = this.gameObject;
		passengerSize = new Vector3[CarSize];
		passengers = new GameObject[CarSize];

		GetComponent<MonsterGrid>().turnOver = true;
		if(GameObject.Find ("Multiplayer") != null)
		{
			Multiplayer = true;
			GameMultiplayer.movesleft--;
		}
	}

	// Update is called once per frame
	void Update () {
		//transform.localScale = new Vector3(size, size, size);

		if(Multiplayer)
		{
			multi();
		}
		else
			single();
	}

	void single()
	{
		if (GetComponent<TranslateMonster> ().TMonster) {
			Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 2.0f);
			foreach(Collider obj in hitColliders)
			{
				if(obj.tag == "EnemyMonster")
				{
					Destroy(obj.gameObject);//play squash sound
				}
			}
		}
		if(currentTargetPassenger != this.gameObject)
		{
			AllowTargetToEnter(false);
		}
	}

	void multi()
	{
		if (GetComponent<TranslateMonster> ().TMonster) {
			Collider[] hitColliders = Physics.OverlapSphere (this.transform.position, 1.0f);
			foreach(Collider obj in hitColliders)
			{
				if(obj.tag == "EnemyMonster" && this.tag == "Player" || this.tag == "EnemyMonster" && obj.tag == "Player" )
				{
					Destroy(obj.gameObject);//play squash sound
					ExitCar();
					Destroy(this.gameObject);
				}
			}
		}
		if(currentTargetPassenger != this.gameObject)
		{
			AllowTargetToEnter(true);
		}
	}
	[RPC]
	void EnterCar(string name, int passengerAmt, Vector3 pos, string parent)
	{
		GameObject target = GameObject.Find (name);
		passengerSize[passengerAmt] = target.transform.localScale;
		target.transform.position = pos;
		target.transform.localScale = new Vector3(0,0,0);
		target.transform.parent = GameObject.Find (parent).transform;
		passengers[passengerAmt] = target;
		passengerAmt++;
	}

	public void EnterWhenReached(GameObject target)
	{
		currentTargetPassenger = target;
	}

	void AllowTargetToEnter(bool multiplayer)
	{
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position+Offset, 1.5f);
		foreach(Collider obj in hitColliders)
		{
			if(obj.gameObject == currentTargetPassenger && currentTargetPassenger != this.gameObject)
			{
				obj.gameObject.GetComponent<TranslateMonster>().Stop();
				if(Multiplayer)
					obj.gameObject.GetComponent<MMonsterGrid>().turnOver = false;
				else
					obj.gameObject.GetComponent<MonsterGrid>().turnOver = false;
				passengerSize[passengerAmt] = obj.transform.localScale;
				obj.transform.position = this.transform.position;
				obj.transform.localScale = new Vector3(0,0,0);
				obj.transform.parent = this.transform;
				passengers[passengerAmt] = obj.gameObject;
				if(passengerAmt <= 0)
				{
					if(multiplayer)
					{
						this.GetComponent<MMonsterGrid>().turnOver = false;
						GameMultiplayer.movesleft++;
					}
					else
					{
						this.GetComponent<MonsterGrid>().turnOver = false;
						GameStart.movesleft++;
					}
				}
				passengerAmt++;
				if(multiplayer)
				{
					NetworkView nv = GetComponent<NetworkView>();
					nv.RPC("EnterCar", RPCMode.Others,obj.name,passengerAmt,this.transform.position,this.name);
				}
				currentTargetPassenger = this.gameObject;
			}
		}
	}

	[RPC]
	void ExitCar()
	{
		bool someoneExit = false;
		for(int i = 0; i < passengers.Length; i++)
		{
			if(passengers[i] == null)
			{
				if(i+1 < passengers.Length)
					passengers[i] = passengers[i+1];
			}
			if(passengers[i] != null)
			{
				passengers[i].transform.position = this.transform.position + passengerSpawn[i] + Offset;
				passengers[i].transform.localScale = passengerSize[i]/this.transform.localScale.x;
				if(this.tag == "Player")
					passengers[i].transform.parent = GameObject.Find("Player").transform;
				else
					passengers[i].transform.parent = GameObject.Find("Enemy").transform;
				passengers[i] = null;
				passengerAmt--;
				someoneExit = true;
			}
		}
		if(someoneExit)
		{
			if(Multiplayer)
			{
				GameMultiplayer.movesleft--;
				GetComponent<MMonsterGrid>().GridDelete();
				GetComponent<MMonsterGrid>().turnOver = true;
			}
			else
			{
				GameStart.movesleft--;
				GetComponent<MonsterGrid>().GridDelete();
				GetComponent<MonsterGrid>().turnOver = true;
			}
		}
	}
	void OnMouseOver () {
		if (Input.GetMouseButton(1)) {
			if (rightClickState == 0)
			{
				rightClickState = 1;
				if(Multiplayer)
				{
					NetworkView nv = GetComponent<NetworkView>();
					nv.RPC("ExitCar", RPCMode.Others);
				}
				ExitCar();
			}
		}
		else
			rightClickState = 0;
	}
}
