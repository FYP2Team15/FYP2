using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState 
{
	None,
	Playing,
	Complete,
	Lost,
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
	private int finalScene = 2;
	public GameState state = GameState.None;
	public static Player player = Player.Player1;
	public static int movesleft = 2;
	public static int turn = 1;
	public string tileName = "Tile";
	private static bool cameraPan = false;
	static GameObject TargetObj = null;
	static GameObject camera;
	private static CameraControl ccamera;
	private bool GameEndScreen = false;
	private string GameEndMessage = "Sample text";
	//TttModel[] board = new Player[9];
	List<TileModel> board = new List<TileModel>();
	
	public GameObject tile;
	public static bool disableGrid = false;

	void Start () {
		turn = 1;
	}

	protected override void Init ()
	{
		GameInit ();
	}
	
	public void GameInit ()
	{
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		//GridGenerator.instance.Generate (tileName,tile, Vector3.zero, 10, 10);
		state = GameState.Playing;
		List<GameObject> list = new List<GameObject> ();
		camera = GameObject.Find ("Camera");
		ccamera = camera.GetComponent <CameraControl> ();
		GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
		movesleft = playerObj.Length;
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
				if (state != GameState.Playing && !GameEndScreen)
					GameEnd();
				if (Input.GetMouseButtonDown (0) && state == GameState.Complete) {
					if(Application.loadedLevel != finalScene)
						Application.LoadLevel (Application.loadedLevel + 1);
					else
						Application.LoadLevel (0);
				}
				if (Input.GetMouseButtonDown (0) && state == GameState.Lost) {
						Application.LoadLevel (0);
				}
				if (movesleft == 0 && state == GameState.Playing) {
					Step();		
				}
				if (cameraPan) {
					if (ccamera.transform.position == TargetObj.transform.position+ccamera.offset) {
						ccamera.target = null;
						cameraPan = false;
					}
				}
		}
	public static void StopCameraPan()
	{
		ccamera.target = null;
		cameraPan = false;
	}
	private void GameEnd()
	{
		switch(state)
		{
		case GameState.Complete:
			GameEndScreen = true;
			GameEndMessage = "Victory";
			//state = GameState.None;
			break;
		case GameState.Lost:
			GameEndScreen = true;
			GameEndMessage = "Defeat";
			//state = GameState.None;
			break;
		case GameState.Draw:
			GameEndScreen = true;
			GameEndMessage = "Draw";
			//state = GameState.None;
			break;
		case GameState.Playing:
		default:
			GameEndScreen = true;
			GameEndMessage = "Error";
			break;
		}
		//Application.LoadLevel(Application.loadedLevel);

	}
	public void Step()
	{
		if(CheckWin ())
		{
			GridCamera.RaycastOff();
			state = GameState.Complete;
			return;
		}

		if(CheckLose ())
		{
			GridCamera.RaycastOff();
			state = GameState.Lost;
			return;
		}

		if(CheckDraw ())
		{
			GridCamera.RaycastOff();
			state = GameState.Draw;
			return;
		}
		if ( state == GameState.Playing)
		NextTurn ();
	}
	
	private bool CheckWin()
	{
		GameObject VTile = GameObject.Find ("VictoryTile");//Get victory tile
		Collider[] hitColliders = Physics.OverlapSphere (VTile.transform.position, 1.0f);//get gameobjects right beside this object
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders [i].name == "Child")//if child is on victory tile win 
				return true;
		}
		return false;
	}

	private bool CheckLose()
	{
		GameObject Child = GameObject.Find ("Child");//find child
		if (Child == null)
			return true;
		GameObject PlayerMonsteer = GameObject.FindGameObjectWithTag("Player");
		if (PlayerMonsteer == null)
			return true;
		return false;
	}

	private bool CheckDraw()
	{
		return false;
	}
	
	public static void NextTurn()
	{

		turn += 1;
		player = (player == Player.Player1) ? Player.Player2 : Player.Player1;
		if (Player1 ()) {
			GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
			foreach(GameObject obj in playerObj)
			{
				obj.GetComponent<MonsterGrid>().turnOver = false;
			}
			movesleft = playerObj.Length;
			ccamera.target = playerObj[0].transform;
			TargetObj = playerObj[0];
			cameraPan = true;
		}
		else {
			GameObject[] enemyObj = GameObject.FindGameObjectsWithTag("EnemyMonster");
			foreach(GameObject obj in enemyObj)
			{
				obj.GetComponent<MonsterGrid>().turnOver = false;
			}
			movesleft = enemyObj.Length;
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
		if(GameEndScreen)
		{
			GUI.Label (new Rect (0, 0, 100, 100), GameEndMessage);
			GUI.Label (new Rect (0, 10, 100, 100), "Click Anything to continue");
		}
		else
		{
			GUI.Label (new Rect (0, 0, 100, 100), turn.ToString());
			GUI.Label (new Rect (0, 10, 100, 100), movesleft.ToString());
			GUI.Label (new Rect (0, 20, 100, 100), player.ToString());
		}
	}
}
