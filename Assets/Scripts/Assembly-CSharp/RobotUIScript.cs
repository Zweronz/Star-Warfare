using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RobotUIScript : MonoBehaviour
{
	protected List<Robot> robots = new List<Robot>();

	protected List<RobotRoom> rooms = new List<RobotRoom>();

	protected Queue<Task> tasks = new Queue<Task>();

	protected Timer getRoomListTimer = new Timer();

	protected RobotStatistics robotStatistics = new RobotStatistics();

	protected Timer doStatisticsTimer = new Timer();

	protected int userID;

	protected string startUserID = "0";

	public string exceptionStr = string.Empty;

	private bool running;

	protected RobotRoom GetRobotRoom(short id)
	{
		foreach (RobotRoom room in rooms)
		{
			if (room.roomID == id)
			{
				return room;
			}
		}
		return null;
	}

	private void Start()
	{
		getRoomListTimer.SetTimer(0.05f, false);
		doStatisticsTimer.SetTimer(2f, false);
		StartCoroutine(LoginUsers(100));
		running = true;
	}

	public void StopThreadReceiver()
	{
	}

	public void StopThreadSender()
	{
	}

	public void Close()
	{
		if (running)
		{
			try
			{
			}
			catch (IOException)
			{
			}
			running = false;
		}
	}

	public bool IsRunning()
	{
		return running;
	}

	public void SetRunning(bool run)
	{
		running = run;
	}

	public RobotRoom FindIdleRoom()
	{
		foreach (RobotRoom room in rooms)
		{
			if (room.rrState == RobotRoomState.Idle)
			{
				return room;
			}
		}
		return null;
	}

	public void ClearRoom()
	{
		rooms.Clear();
	}

	public void AddRoom(RobotRoom room)
	{
		rooms.Add(room);
	}

	private void Update()
	{
		if (getRoomListTimer.Ready())
		{
			RefreshRoomList();
			getRoomListTimer.Do();
		}
		if (doStatisticsTimer.Ready())
		{
			DoStatistics();
			doStatisticsTimer.Do();
		}
		foreach (Robot robot in robots)
		{
			robot.State.NextState(robot);
			robot.network.SendData();
			robot.network.ProcessRobotReceivedPackets(robot);
		}
		if (tasks.Count > 0)
		{
			Task task = tasks.Dequeue();
			if (task != null && task.taskID != 100 && task.taskID != 101)
			{
			}
		}
	}

	public void MasterStartGame(Robot robot)
	{
		if (robot.lobby.IsMasterPlayer)
		{
			StartGameRequest request = new StartGameRequest();
			robot.network.SendRequest(request);
		}
	}

	public void LoginUser()
	{
		NetworkManager networkManager = new NetworkManager(true);
		Robot robot = new Robot();
		robot.network = networkManager;
		networkManager.StartNetworkRobot(robot, "192.168.1.112", 8095);
		string empty = string.Empty;
		userID++;
		empty = "TestUser" + userID;
		PlayerLoginRequest playerLoginRequest = new PlayerLoginRequest();
		playerLoginRequest.userName = empty;
		playerLoginRequest.passWord = "123";
		networkManager.SendRequest(playerLoginRequest);
		robot.userName = empty;
		robot.o.name = empty;
		robot.id = userID;
		robot.timeMgr.Init();
		robot.uiScript = this;
		robots.Add(robot);
		robotStatistics.robotLoginNumber++;
	}

	private IEnumerator LoginUsers(int number)
	{
		userID = int.Parse(startUserID);
		for (int i = 0; i < number; i++)
		{
			LoginUser();
			yield return new WaitForSeconds(0.5f);
		}
	}

	public void RefreshRoomList()
	{
		foreach (Robot robot in robots)
		{
			GetRoomListRequest request = new GetRoomListRequest(0, 0);
			robot.network.SendRequest(request);
		}
	}

	public void DoStatistics()
	{
		robotStatistics.robotIdleNumber = 0;
		robotStatistics.robotInGameNumber = 0;
		robotStatistics.robotInRoomWaitingNumber = 0;
		foreach (Robot robot in robots)
		{
			if (robot.State == robot.PlayingState)
			{
				robotStatistics.robotInGameNumber++;
			}
			else if (robot.State == robot.InRoomState || robot.State == robot.MasterInRoomState)
			{
				robotStatistics.robotInRoomWaitingNumber++;
			}
			else if (robot.State == robot.IdleState)
			{
				robotStatistics.robotIdleNumber++;
			}
		}
	}

	private void OnGUI()
	{
		startUserID = GUI.TextField(new Rect(10f, 170f, 200f, 30f), startUserID);
		GUI.Label(new Rect(10f, 210f, 200f, 30f), "\ufffd\u0735\ufffd¼\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd" + robotStatistics.robotLoginNumber);
		GUI.Label(new Rect(10f, 250f, 200f, 30f), "\ufffd\ufffd\ufffd\ufffd\ufffd\ufffdϷ\ufffdĻ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd" + robotStatistics.robotInGameNumber);
		GUI.Label(new Rect(10f, 290f, 200f, 30f), "\ufffd\ufffd\ufffd\ufffd\ufffdڵȴ\ufffd\ufffdĻ\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd" + robotStatistics.robotInRoomWaitingNumber);
		GUI.Label(new Rect(10f, 330f, 200f, 30f), "\ufffd\ufffd\ufffdл\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd\ufffd" + robotStatistics.robotIdleNumber);
		GUI.Label(new Rect(10f, 370f, 400f, 200f), exceptionStr);
		if (GUI.Button(new Rect(10f, 10f, 200f, 30f), "Login One Player"))
		{
			LoginUsers(1);
		}
		if (GUI.Button(new Rect(10f, 50f, 200f, 30f), "Login 10 Players"))
		{
			LoginUsers(10);
		}
		if (GUI.Button(new Rect(10f, 90f, 200f, 30f), "Login 30 Players"))
		{
			tasks.Enqueue(new Task(100));
		}
		if (GUI.Button(new Rect(250f, 10f, 200f, 30f), "Each Player Create A Game 2"))
		{
			foreach (Robot robot in robots)
			{
				if (robot.State == robot.IdleState)
				{
					CreateRoomRequest request = new CreateRoomRequest(robot.userName + "'s room", -1, 2, false, 0, 0, 1, 500, 0, 11);
					robot.network.SendRequest(request);
				}
			}
		}
		if (GUI.Button(new Rect(250f, 50f, 200f, 30f), "Each Player Create A Game 3"))
		{
			foreach (Robot robot2 in robots)
			{
				if (robot2.State == robot2.IdleState)
				{
					CreateRoomRequest request2 = new CreateRoomRequest(robot2.userName + "'s room", -1, 3, false, 0, 0, 1, 500, 0, 11);
					robot2.network.SendRequest(request2);
				}
			}
		}
		if (GUI.Button(new Rect(250f, 90f, 200f, 30f), "Each Player Join A Game"))
		{
			tasks.Enqueue(new Task(101));
		}
		if (GUI.Button(new Rect(10f, 130f, 200f, 30f), "Refresh RoomList"))
		{
			RefreshRoomList();
		}
		if (!GUI.Button(new Rect(250f, 130f, 200f, 30f), "Master Start Game"))
		{
			return;
		}
		foreach (Robot robot3 in robots)
		{
			MasterStartGame(robot3);
		}
	}

	private void OnApplicationQuit()
	{
		foreach (Robot robot in robots)
		{
			if (robot.network != null)
			{
				robot.network.CloseRobotConnection();
				Debug.Log("Robot " + robot.userName + " logout");
			}
		}
		Close();
	}
}
