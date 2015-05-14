using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState 
{
	None,
	Playing,
	Complete,
	Draw
}

public enum Player
{
	None,
	Player1,
	Player2
}


public class GameStart : MonoSingleton<GameStart>
{
	public GameState state = GameState.None;
	public Player player = Player.Player1;
	public int turn = 0;
	public string tileName = "Tile";
	//TttModel[] board = new Player[9];
	List<TileModel> board = new List<TileModel>();
	
	public GameObject tile;

	protected override void Init ()
	{
		GameInit ();
	}
	
	public void GameInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		GridGenerator.instance.Generate (tileName,tile, Vector3.zero, 10, 10);

		List<GameObject> list = new List<GameObject> ();
	}
	//public void Start()
	//{
	//	GridCamera.RaycastOn();
	//	
	//	turn = 0;
	//	player = Player.Player1;
	//	state = GameState.Playing;
	//	board = new List<TileModel>();
	//	GridGenerator.instance.Clear();
	//	GridGenerator.instance.Generate(tile,Vector3.zero, 3,3);
	//	
	//	int index = 0;
	//	foreach(GameObject go in GridGenerator.instance.tiles)
	//	{
	//		TileModel tm = go.GetComponent<TileModel>() as TileModel;
	//		tm.index = index;
	//		board.Add(tm);
	//		index++;
	//	}
	//	
	//}
	
	public void Step()
	{
		if(CheckWin ())
		{
			GridCamera.RaycastOff();
			state = GameState.Complete;
			return;
		}
		
		if(CheckDraw ())
		{
			GridCamera.RaycastOff();
			state = GameState.Draw;
			return;
		}
		
		NextTurn ();
	}
	
	private bool CheckWin()
	{
		return false;
	}
	
	private bool CheckDraw()
	{
		return false;
	}
	
	private void NextTurn()
	{
		turn += 1;
		player = (player == Player.Player1) ? Player.Player2 : Player.Player1;
	}
	
	public void UpdateBoard(int index, Player value)
	{
	}
}
