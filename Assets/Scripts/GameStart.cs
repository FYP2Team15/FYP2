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
	public static Player player = Player.Player1;
	public static int movesleft = 2;
	public static int turn = 0;
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
	void Update () {
		if (movesleft == 0)
			NextTurn ();
		}
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
	
	public static void NextTurn()
	{
		movesleft = 2;
		turn += 1;
		player = (player == Player.Player1) ? Player.Player2 : Player.Player1;
		if (Player1 ()) {
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
			GameObject camera = GameObject.Find ("Camera");
			CameraControl ccamera = camera.GetComponent <CameraControl> ();
			ccamera.target = playerObj.transform;
		}
	}

	public static bool Player1()
	{
		if (player == Player.Player1)
			return true;
		return false;
	}
	public void UpdateBoard(int index, Player value)
	{
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), turn.ToString());
		GUI.Label (new Rect (0, 10, 100, 100), movesleft.ToString());
		GUI.Label (new Rect (0, 20, 100, 100), player.ToString());
	}
}
