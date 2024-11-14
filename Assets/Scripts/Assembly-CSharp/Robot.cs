using System.Collections;
using UnityEngine;

public class Robot
{
	public NetworkManager network;

	public string userName = string.Empty;

	public int id;

	public TimeManager timeMgr = new TimeManager();

	public GameApp gameApp = new GameApp();

	public Lobby lobby = new Lobby();

	public RobotUIScript uiScript;

	public GameObject o = new GameObject();

	private InputInfo inputInfo = new InputInfo();

	public Hashtable enemyList = new Hashtable();

	public int roundNum;

	public Timer spawnEnemyTimer = new Timer();

	public int joinedPlayer;

	public int maxPlayer = 3;

	public RobotState State;

	public RobotState IdleState = new RobotIdleState();

	public RobotState MasterInRoomState = new RobotMasterInRoomState();

	public RobotState InRoomState = new RobotInRoomState();

	public RobotState PlayingState = new RobotPlayingState();

	public Timer sendingTimer = new Timer();

	public bool sendingJoingRequest;

	public bool sendingCreatingRoomRequest;

	public bool sendingStartGameRequest;

	public Robot()
	{
		timeMgr.Init();
		timeMgr.setMaxLoopTimes(-1);
		timeMgr.setPeriod(1f);
		State = IdleState;
		spawnEnemyTimer.SetTimer(1f, false);
		sendingTimer.SetTimer(0.2f, false);
		lobby.IsMasterPlayer = false;
	}

	public void SendInput()
	{
		o.transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360), 0f));
		inputInfo.moveDirection = o.transform.forward;
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			inputInfo.fire = true;
		}
		else
		{
			inputInfo.fire = false;
		}
		SendPlayerInputRequest request = new SendPlayerInputRequest(inputInfo.fire, inputInfo.IsMoving());
		network.SendRequest(request);
	}

	public void Move()
	{
		o.transform.Translate(7f * Time.deltaTime * Vector3.forward, Space.Self);
		if (Vector3.Distance(o.transform.position, Vector3.zero) > 20f)
		{
			o.transform.position = Vector3.zero;
		}
	}

	public void SetState(RobotState state)
	{
		State = state;
		State.EnterStateTime = Time.time;
	}

	public void UpdateEnemy()
	{
		object[] array = new object[enemyList.Count];
		enemyList.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			RobotEnemy robotEnemy = enemyList[array[i]] as RobotEnemy;
			robotEnemy.Loop(this);
		}
	}

	public void RemoveEnemy(short enemyID)
	{
		RobotEnemy robotEnemy = enemyList[enemyID] as RobotEnemy;
		Object.Destroy(robotEnemy.o);
		enemyList.Remove(enemyID);
	}

	public void ClearAllEnemies()
	{
		object[] array = new object[enemyList.Count];
		enemyList.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			RobotEnemy robotEnemy = enemyList[array[i]] as RobotEnemy;
			Object.Destroy(robotEnemy.o);
		}
		enemyList.Clear();
	}

	public void SpawnEnemy()
	{
		if (spawnEnemyTimer.Ready() && enemyList.Count < 8 && lobby.IsMasterPlayer)
		{
			EnemyType enemyType = EnemyType.Drone;
			roundNum++;
			EnemySpawnRequest request = new EnemySpawnRequest(enemyType, Vector3.zero, (short)roundNum, 0, false, false);
			network.SendRequest(request);
			spawnEnemyTimer.Do();
		}
	}
}
