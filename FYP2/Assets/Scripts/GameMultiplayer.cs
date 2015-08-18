using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
public enum GameStateM 
{
	None,
	Playing,
	Complete,
	Lost,
	Draw
}

public enum PlayerM
{
	None,
	Player1,
	Player2
}

public class GameMultiplayer : MonoSingleton<GameMultiplayer>
{
	//private int finalScene = 2;
	public GameObject WinScreen;
	public GameObject LoseScreen;
	public GameStateM state = GameStateM.None;
	public static PlayerM player = PlayerM.Player1;
	public static PlayerM you = PlayerM.Player1;
	public static int movesleft = 2;
	public static int turn = 1;
	public string tileName = "Tile";
	private static bool cameraPan = false;
	static GameObject TargetObj = null;
	static GameObject camera;
	private static CameraControl ccamera;
	private static bool GameEndScreen = false;
	private static string GameEndMessage = "Sample text";
	public static int currentPan = 0;
	private bool CMS = false;
	private bool CPort = false;
	private bool CSPort = false;
	private string newIpVal = "127.0.0.1";
	private string newPortVal = "23466";
	private string ServerPort = "25000";
	private bool gameStarted = false;
	public string MainMenu = "MainPage";
	bool joinServer = false;
	//TttModel[] board = new Player[9];
	//List<TileModel> board = new List<TileModel>();
	
	//public GameObject Map;
	public static bool disableGrid = true;
	public static bool disableCameraControl = true;

	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";
	
	private void StartServer()
	{
		Network.InitializeServer(4, int.Parse(ServerPort), !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	private HostData[] hostList;
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}
	
	private void JoinServer(HostData hostData)
	{
		if(!joinServer)
		{
			Network.Connect(hostData);
			joinServer = true;
		}
	}
	
	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}

	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		// When the player disconnects remove them across the network, so nobody can see them
		Network.RemoveRPCs(networkPlayer);
		Network.Disconnect();
		MasterServer.UnregisterHost();
		Application.LoadLevel(MainMenu);
	}

	NetworkPlayer playersName; string weaponPickedUp;
	void OnPlayerConnected()
	{
		Debug.Log("Player Joined");
		GameInit (false);
		GetComponent<NetworkView>().RPC("GameInit", RPCMode.All, true);
	}

	[RPC]
	public void TranslateM(GameObject obj,bool T, Vector3 Pos)
	{
		GameObject target = GameObject.Find (obj.name);
		target.GetComponent<TranslateMonster> ().Translate (T, Pos);
	}

	[RPC]
	public void StartLevel( NetworkPlayer player, string levelName) {
		Application.LoadLevel(levelName);
	}

	void Awake () {
	}
	void Start () {
		MasterServer.ipAddress = "67.225.180.24";
	}

	[RPC]
	public void GameInit (bool client)
	{
		//GUI.skin.font = (Font)Resources.Load("Animal Silence");
		turn = 1;
		if(client)
		you = PlayerM.Player2;
		GridCamera.RaycastOn ();
		//GridGenerator.instance.Clear ();
		//GridGenerator.instance.Generate (tileName,tile, Vector3.zero, 10, 10);
		state = GameStateM.Playing;
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
			obj.GetComponent<MMonsterGrid>().turnOver = false;
		}
		movesleft = playerObj.Length-Excluded;
		gameStarted = true;
		disableGrid = false;
		disableCameraControl = false;
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
		if (!Network.isClient && !Network.isServer && gameStarted) {
			Network.Disconnect();
			Application.LoadLevel(MainMenu);
		}
		if (!Network.isClient && !Network.isServer && CMS) {
			ChangeMasterServer();
		}
		else if (!Network.isClient && !Network.isServer && CPort) {
			ChangePort();
		}
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetKeyDown ("f") && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled 
		    && !disableCameraControl)
		{
			if(Player1() && Network.isServer ||
			Player2() && Network.isClient)
			{
				movesleft = 0;
				GetComponent<NetworkView>().RPC("zeroMoves", RPCMode.Others);
			}
		}
		#endif
		if (movesleft == 0 && state == GameStateM.Playing && !GameObject.Find("BattleCamera").GetComponent<Camera>().enabled) {
			Step();		
		}
		if (Input.GetMouseButtonDown (0) && state == GameStateM.Complete
		    || Input.GetMouseButtonDown (0) && state == GameStateM.Lost
		   	|| Input.GetMouseButtonDown (0) && state == GameStateM.Draw) {
			Network.Disconnect();
			if (Network.isServer)
			MasterServer.UnregisterHost();
			Application.LoadLevel(MainMenu);
		}
		if (cameraPan && TargetObj != null) {
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

	public void Forfeit()
	{
		GetComponent<NetworkView>().RPC("Win", RPCMode.Others);
		state = GameStateM.Lost;
		GameEnd ();
	}

	[RPC]
	public void Win()
	{
		state = GameStateM.Complete;
		GameEnd ();
	}
	public static bool getCameraPan(){
		return cameraPan;
	}
	private void GameEnd()
	{
		switch(state)
		{
		case GameStateM.Complete:
			GameEndScreen = true;
			GameEndMessage = "Victory";
			WinScreen.GetComponent<Image>().enabled = true;
			//state = GameState.None;
			break;
		case GameStateM.Lost:
			GameEndScreen = true;
			GameEndMessage = "Defeat";
			LoseScreen.GetComponent<Image>().enabled = true;
			//state = GameState.None;
			break;
		case GameStateM.Draw:
			GameEndScreen = true;
			GameEndMessage = "Draw";
			//state = GameState.None;
			break;
		case GameStateM.Playing:
		default:
			GameEndScreen = true;
			GameEndMessage = "Error";
			break;
		}
		//Application.LoadLevel(Application.loadedLevel);
		
	}
	public void Step()
	{
		if(Network.isServer)
		{
			if(CheckWin ())
			{
				GridCamera.RaycastOff();
				state = GameStateM.Complete;
				GameEnd();
				return;
			}
				
			if(CheckLose ())
			{
				GridCamera.RaycastOff();
				state = GameStateM.Lost;
				GameEnd();
				return;
			}
				
			if(CheckDraw ())
			{
				GridCamera.RaycastOff();
				state = GameStateM.Draw;
				return;
			}
		}

		if ( state == GameStateM.Playing && Network.isServer)
		{
			NextTurn ();
			GetComponent<NetworkView>().RPC("NextTurnM", RPCMode.Others);
		}
	}
	
	private bool CheckWin()
	{
		GameObject VTile = GameObject.Find ("VictoryTile");//Get victory tile
		Collider[] hitColliders = Physics.OverlapSphere (VTile.transform.position, 1.0f);//get gameobjects right beside this object
		for (int i = 0; i < hitColliders.Length; i++) {
			if (hitColliders [i].name == "Child")//if child is on victory tile win 
				return true;
		}
		GameObject EnemyMonster = GameObject.FindGameObjectWithTag("EnemyMonster");
		if (EnemyMonster == null)
			return true;
		return false;
	}
	
	private bool CheckLose()
	{
		GameObject Child = GameObject.Find ("Child");//find child
		if (Child == null)
			return true;
		GameObject PlayerMonster = GameObject.FindGameObjectWithTag("Player");
		if (PlayerMonster == null)
			return true;
		return false;
	}
	
	private bool CheckDraw()
	{
		return false;
	}
	[RPC]
	public void zeroMoves()
	{
		movesleft = 0;
	}
	[RPC]
	public void NextTurnM()
	{
		turn += 1;
		player = (player == PlayerM.Player1) ? PlayerM.Player2 : PlayerM.Player1;
		if (Player1 ()) {
			GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
			int Excluded = 0;
			foreach(GameObject obj in playerObj)
			{
				if(obj.GetComponent<CarScript>() != null)
				{
					if(obj.GetComponent<CarScript>().passengerAmt <=0)
						Excluded ++;
				}
				obj.GetComponent<MMonsterGrid>().turnOver = false;
			}
			movesleft = playerObj.Length-Excluded;
			ccamera.target = playerObj[0].transform;
			TargetObj = playerObj[0];
			cameraPan = true;
		}
		else {
			GameObject[] enemyObj = GameObject.FindGameObjectsWithTag("EnemyMonster");
			int Excluded = 0;
			foreach(GameObject obj in enemyObj)
			{
				if(obj.GetComponent<CarScript>() != null)
				{
					if(obj.GetComponent<CarScript>().passengerAmt <=0)
						Excluded ++;
				}
				obj.GetComponent<MMonsterGrid>().turnOver = false;
			}
			movesleft = enemyObj.Length-Excluded;
			ccamera.target = enemyObj[0].transform;
			TargetObj = enemyObj[0];
			cameraPan = true;
		}
	}
	public void NextTurn()
	{
		
		turn += 1;
		player = (player == PlayerM.Player1) ? PlayerM.Player2 : PlayerM.Player1;
		if (Player1 ()) {
			GameObject[] playerObj = GameObject.FindGameObjectsWithTag("Player");
			int Excluded = 0;
			foreach(GameObject obj in playerObj)
			{
				if(obj.GetComponent<CarScript>() != null)
				{
					if(obj.GetComponent<CarScript>().passengerAmt <=0)
						Excluded ++;
				}
				obj.GetComponent<MMonsterGrid>().turnOver = false;
			}
			movesleft = playerObj.Length-Excluded;
			ccamera.target = playerObj[0].transform;
			TargetObj = playerObj[0];
			cameraPan = true;
		}
		else {
			GameObject[] enemyObj = GameObject.FindGameObjectsWithTag("EnemyMonster");
			int Excluded = 0;
			foreach(GameObject obj in enemyObj)
			{
				if(obj.GetComponent<CarScript>() != null)
				{
					if(obj.GetComponent<CarScript>().passengerAmt <=0)
						Excluded ++;
				}
				obj.GetComponent<MMonsterGrid>().turnOver = false;
			}
			movesleft = enemyObj.Length-Excluded;
			ccamera.target = enemyObj[0].transform;
			TargetObj = enemyObj[0];
			cameraPan = true;
		}
		if (movesleft <= 0)
		{
			if(Player1 ())
				state = GameStateM.Lost;
			else
				state = GameStateM.Complete;
			GameEnd();
		}
	}
	
	public static bool Player1()
	{
		if (player == PlayerM.Player1)
			return true;
		return false;
	}

	public static bool Player2()
	{
		if (player == PlayerM.Player2)
			return true;
		return false;
	}
	public void UpdateBoard(int index, PlayerM value)
	{
	}

	float ScaleToHeight(float value)
	{
		float newValue = value / 768;
		return (float)((value / 768) * Screen.height);
	}

	float ScaleToWidth(float value)
	{
		float newValue = value / 1024;
		return (float)((value / 1024) * Screen.width);
	}

	string GetIP()
	{
		string strHostName = "";
		strHostName = System.Net.Dns.GetHostName();
		
		IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
		
		IPAddress[] addr = ipEntry.AddressList;
		
		return addr[addr.Length-1].ToString();
		
	}

	void InitChangeMasterServer()
	{
		CMS = true;
	}

	void ChangeMasterServer()
	{
		for (KeyCode i = KeyCode.Keypad0; i < KeyCode.Keypad9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				newIpVal+= Value;
			}
		}

		for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				newIpVal+= Value;
			}
		}

		if(Input.GetKeyDown (KeyCode.Period) || Input.GetKeyDown (KeyCode.KeypadPeriod))
		{
			newIpVal+= ".";
		}

		if(Input.GetKeyDown (KeyCode.Backspace))
		{
			newIpVal = newIpVal.Substring(0,newIpVal.Length-1);
		}

	}

	void InitChangePort()
	{
		CPort = true;
	}
	
	void ChangePort()
	{
		for (KeyCode i = KeyCode.Keypad0; i < KeyCode.Keypad9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				newPortVal+= Value;
			}
		}
		
		for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				newPortVal+= Value;
			}
		}
		
		if(Input.GetKeyDown (KeyCode.Backspace))
		{
			newPortVal = newPortVal.Substring(0,newPortVal.Length-1);
		}
		
	}

	void InitChangeServerPort()
	{
		CSPort = true;
	}
	
	void ChangeServerPort()
	{
		for (KeyCode i = KeyCode.Keypad0; i < KeyCode.Keypad9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				ServerPort+= Value;
			}
		}
		
		for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha9+1; i ++)
		{			
			if(Input.GetKeyDown (i))
			{
				string Value  = i.ToString();
				Value = Value[Value.Length-1].ToString();
				ServerPort+= Value;
			}
		}
		
		if(Input.GetKeyDown (KeyCode.Backspace))
		{
			ServerPort = ServerPort.Substring(0,ServerPort.Length-1);
		}
		
	}

	void OnGUI()
	{
		GUI.color = Color.black;
		if (Network.isServer)
			GUILayout.Label("Running as a server");
		else if (Network.isClient)
			GUILayout.Label("Running as a client");
		if(GameEndScreen)
		{
			GUI.Label (new Rect (0, 0, ScaleToWidth(100), ScaleToHeight(100)), GameEndMessage);
			GUI.Label (new Rect (0, ScaleToHeight(10), ScaleToWidth(100), ScaleToHeight(100)), "Click Anything to continue");
		}
		else
		{
			GUI.Label (new Rect (0, ScaleToHeight(20), ScaleToWidth(100), ScaleToHeight(100)), turn.ToString());
			GUI.Label (new Rect (0, ScaleToHeight(40), ScaleToWidth(100), ScaleToHeight(100)), movesleft.ToString());
			GUI.Label (new Rect (0, ScaleToHeight(60), ScaleToWidth(100), ScaleToHeight(100)), player.ToString());
		}

		if (!Network.isClient && !Network.isServer && !CMS && !CPort && !CSPort)
		{
			GUI.Button(new Rect(ScaleToWidth(100), ScaleToHeight(400), ScaleToWidth(250), ScaleToHeight(100)), "This computer's Ip Address:\n" + GetIP());

			if (GUI.Button(new Rect(ScaleToWidth(100), ScaleToHeight(550), ScaleToWidth(250), ScaleToHeight(100)), "Get Host from:\n" + MasterServer.ipAddress + ":" + MasterServer.port))
			{	
				InitChangeMasterServer();
				InitChangePort();
			}

			if (GUI.Button(new Rect(ScaleToWidth(100), ScaleToHeight(100), ScaleToWidth(250), ScaleToHeight(100)), "Start Server"))
				InitChangeServerPort();
			
			if (GUI.Button(new Rect(ScaleToWidth(100), ScaleToHeight(250), ScaleToWidth(250), ScaleToHeight(100)), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(ScaleToWidth(400), ScaleToHeight(100 + (110 * i)), ScaleToWidth(300), ScaleToHeight(100)), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}

		}
		else if (!Network.isClient && !Network.isServer && CSPort)
		{
			if (GUI.Button(new Rect(ScaleToWidth(275), ScaleToHeight(100), ScaleToWidth(300), ScaleToHeight(200)), "Select Port and host:\n" + ServerPort))
			{
				StartServer();
			}
		}
		else if (!Network.isClient && !Network.isServer) {
			if(CMS)
			{
				if (GUI.Button(new Rect(ScaleToWidth(275), ScaleToHeight(100), ScaleToWidth(375), ScaleToHeight(200)), "New Host server:\n" + newIpVal))
				{
					CMS = false;
					MasterServer.ipAddress = newIpVal;
				}
			}
			else if (CPort) {
				if (GUI.Button(new Rect(ScaleToWidth(275), ScaleToHeight(100), ScaleToWidth(375), ScaleToHeight(200)), "New Host Port:\n" + newPortVal))
				{
					CPort = false;
					MasterServer.port = int.Parse(newPortVal);
				}
			}
		}

	}
}
