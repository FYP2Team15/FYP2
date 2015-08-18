using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
	private int TurnLeft = 0;
	public int totalTurn = 30;
	public string NextScene = "MainPage";
	public GameObject VictoryTile;
	public GameObject startingPosition;
	bool startOPCD = false;
	float countDownAP = 0;
	bool VictoryPan = false;
	public GameObject WinScreen;
	public GameObject LoseScreen;
	public string file = "Assets/Save.txt";
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
	public static int currentPan = 0;
	//TttModel[] board = new Player[9];
	//List<TileModel> board = new List<TileModel>();
	
	public GameObject tile;
	public static bool disableGrid = false;
	public static bool disableCameraControl = false;
	public PauseButton_InGame PsB;
	CheckGrid CG;
	MonsterGrid MG;
	public GameObject PauseMenu;

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
		//List<GameObject> list = new List<GameObject> ();
		camera = GameObject.Find ("Camera");
		ccamera = camera.GetComponent <CameraControl> ();
		GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
		int Excluded = 0;

		foreach(GameObject obj in playerObj)
		{
			if(obj.GetComponent<CarScript>() != null)
			{
				if(obj.GetComponent<CarScript>().passengerAmt <=0)
				{
					Excluded ++;
					obj.GetComponent<MonsterGrid>().turnOver = true;
				}
			}
			obj.GetComponent<MonsterGrid>().turnOver = false;
		}
		TurnLeft = totalTurn;
		movesleft = playerObj.Length-Excluded;
		disableGrid = true;
		disableCameraControl = true;
	}
	public void PanToVictory()
	{
		if (VictoryTile != null)
		{
			ccamera.target = VictoryTile.transform;
			TargetObj = VictoryTile;
			cameraPan = true;
			VictoryPan = true;
		}
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
		if (startOPCD)
		{
			countDownAP += Time.deltaTime;
			if(countDownAP > 2)
			{
				ccamera.target = startingPosition.transform;
				TargetObj = startingPosition;
				cameraPan = true;
				startOPCD = false;
				disableGrid = false;
				disableCameraControl = false;
			}
		}
				#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetKeyDown ("f") && Player1 () && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled 
		    && !disableCameraControl)
					movesleft = 0;
				#endif
				if (state != GameState.Playing && !GameEndScreen)
					GameEnd();
				if (Input.GetMouseButtonDown (0) && state == GameState.Complete) {
					var sr = File.CreateText(file);
					sr.WriteLine ("1");
					sr.Close();
					Application.LoadLevel (NextScene);
				}
				if (Input.GetMouseButtonDown (0) && state == GameState.Lost) {
						Application.LoadLevel ("MainPage");
				}
		if (movesleft <= 0 && state == GameState.Playing && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled) {
					Step();		
				}
				if (cameraPan && TargetObj != null) {
					if (ccamera.transform.position == TargetObj.transform.position+ccamera.offset) {
						ccamera.target = null;
						cameraPan = false;
						if (VictoryPan)
						{
							startOPCD = true;
							VictoryPan = false;
							
						}
					}
				}



		if (PsB.OnPaused == true) {
			Time.timeScale = 0;
//			CG.PlayPause = false;
//			MG.PlPaused = true;
		} else if (PsB.OnPaused == false) {
			Time.timeScale = 1;
//			CG.PlayPause = true;
//			MG.PlPaused = false;
		}

	}
	public static void StopCameraPan()
	{
		ccamera.target = null;
		cameraPan = false;
	}
	public static bool getCameraPan(){
		return cameraPan;
	}
	private void GameEnd()
	{
		switch(state)
		{
		case GameState.Complete:
			GameEndScreen = true;
			GameEndMessage = "Victory";
			WinScreen.GetComponent<Image>().enabled = true;
			//state = GameState.None;
			break;
		case GameState.Lost:
			GameEndScreen = true;
			GameEndMessage = "Defeat";
			LoseScreen.GetComponent<Image>().enabled = true;
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
		GameObject EnemyMonsteer = GameObject.FindGameObjectWithTag("EnemyMonster");
		if (EnemyMonsteer == null)
			return true;
		return false;
	}

	private bool CheckLose()
	{
		if(totalTurn <= turn)
			return true;
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
	
	public void NextTurn()
	{

		turn += 1;
		player = (player == Player.Player1) ? Player.Player2 : Player.Player1;
		if (Player1 ()) {
			TurnLeft--;
			GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
			int Excluded = 0;
			foreach(GameObject obj in playerObj)
			{
				if(obj.GetComponent<CarScript>() != null)
				{
					if(obj.GetComponent<CarScript>().passengerAmt <=0)
						Excluded ++;
				}
				obj.GetComponent<MonsterGrid>().turnOver = false;
				obj.GetComponent<MonsterGrid>().throwOver = false;
			}
			movesleft = playerObj.Length-Excluded;
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
		if (movesleft <= 0)
		{
			if(Player1 ())
				state = GameState.Lost;
			else
				state = GameState.Complete;
			GameEnd();
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
		GUI.skin.font = (Font)Resources.Load("Animal Silence");
		GUI.color = Color.yellow;
		if(GameEndScreen)
		{
			GUI.Label (new Rect (0, 50, 100, 100), GameEndMessage);
			GUI.Label (new Rect (0, 65, 100, 100), "Click Anything to continue");
		}
		else
		{
			GUI.Label (new Rect (0, 50, 300, 100), "Turns left:" + (TurnLeft).ToString());
			GUI.Label (new Rect (0, 65, 300, 100), "Moves left:" + movesleft.ToString());
			GUI.Label (new Rect (0, 80, 300, 100), player.ToString() + "'s Turn");
		}
	}
}
