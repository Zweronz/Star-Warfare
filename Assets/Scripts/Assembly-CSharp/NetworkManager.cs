using System;
using UnityEngine;

public class NetworkManager
{
	public const int WRITE_BUF_SIZE = 512;

	public string exceptionMessage;

	public string strIP;

	public int port;

	public Connection conn;

	public RobotConnection robotConn;

	private object lockThis = new object();

	private DateTime lastTime = DateTime.Now;

	private int packetNum;

	private int byteNum;

	private ReceivedPacketCache receivedPacketCache;

	private SendPacketCache sendPacketCache;

	private byte[] writebuffer = new byte[512];

	public bool sending;

	public bool IsDisconnected { get; set; }

	public bool IsDisplayErrorBox { get; set; }

	public NetworkManager()
	{
		conn = new Connection(this);
		strIP = "sw1.freyrgames.com";
		port = 8095;
	}

	public NetworkManager(bool robot)
	{
		robotConn = new RobotConnection(this);
		Config config = new Config();
		config.Load();
		strIP = config.GetIP();
		port = Convert.ToInt32(config.GetPort());
	}

	public ReceivedPacketCache getReceivedPacketCache()
	{
		return receivedPacketCache;
	}

	public SendPacketCache getSendPacketCache()
	{
		return sendPacketCache;
	}

	public void StartNetwork(string ip, int port)
	{
		receivedPacketCache = new ReceivedPacketCache();
		sendPacketCache = new SendPacketCache();
		conn.StartConn(ip, port);
	}

	public void StartNetworkRobot(Robot robot, string ip, int port)
	{
		receivedPacketCache = new ReceivedPacketCache();
		sendPacketCache = new SendPacketCache();
		robotConn.StartConn(ip, port);
	}

	public static void RobotThreadProc(object obj)
	{
	}

	public void SendRequest(object request)
	{
		sendPacketCache.AddPacket(request);
		updatePacket(request);
	}

	public void SendData()
	{
		if (robotConn.IsConnected && !getSendPacketCache().isEmpty() && !sending)
		{
			Request request = (Request)getSendPacketCache().SendPacket();
			robotConn.SendData(request);
		}
	}

	private bool needSend(object request)
	{
		return request.GetType() == typeof(TimeSynchronizeRequest) || request.GetType() == typeof(GetSceneStateRequest) || request.GetType() == typeof(CreateRoomRequest) || request.GetType() == typeof(GetRoomDataRequest) || request.GetType() == typeof(GetRoomListRequest) || request.GetType() == typeof(JoinRoomRequest) || request.GetType() == typeof(LeaveRoomRequest) || request.GetType() == typeof(PlayerLoginRequest) || request.GetType() == typeof(QuickJoinRequest) || request.GetType() == typeof(RoleLoginRequest) || request.GetType() == typeof(RoomTimeSynchronizeRequest) || request.GetType() == typeof(SearchRoomRequest) || request.GetType() == typeof(SetRoomPingRequest) || request.GetType() == typeof(StartGameRequest) || request.GetType() == typeof(EnemySpawnRequest) || request.GetType() == typeof(EnemyOnHitRequest);
	}

	private void updatePacket(object request)
	{
		packetNum++;
		byteNum = byteNum + 2 + ((Request)request).GetBytes()[1];
		TimeSpan timeSpan = DateTime.Now - lastTime;
		if ((float)timeSpan.TotalSeconds > 1f)
		{
			GameApp.GetInstance().AverageSendPacket = (float)packetNum / (float)timeSpan.TotalSeconds;
			GameApp.GetInstance().AverageSendByte = (float)byteNum / (float)timeSpan.TotalSeconds;
			if (GameApp.GetInstance().AverageSendPacket > 50f)
			{
				Debug.Log("Too many packets in one seconds:" + GameApp.GetInstance().AverageSendPacket);
			}
			packetNum = 0;
			byteNum = 0;
			lastTime = DateTime.Now;
		}
	}

	public void ProcessReceivedPackets()
	{
		while (!receivedPacketCache.isEmpty())
		{
			Response response = (Response)receivedPacketCache.RetrivePacket();
			if (response != null)
			{
				if (Application.loadedLevelName == "MultiMenu" && response.responseID > 100)
				{
					break;
				}
				response.ProcessLogic();
			}
		}
	}

	public void ProcessRobotReceivedPackets(Robot robot)
	{
		while (!receivedPacketCache.isEmpty())
		{
			Response response = (Response)receivedPacketCache.RetrivePacket();
			if (response != null)
			{
				if (Application.loadedLevelName == "Lobby" && response.responseID >= 100)
				{
					break;
				}
				response.ProcessRobotLogic(robot);
			}
		}
	}

	public void CloseRobotConnection()
	{
		lock (lockThis)
		{
			if (robotConn != null)
			{
				robotConn.CloseSocket();
			}
			robotConn = null;
			receivedPacketCache.Clear();
			sendPacketCache.Clear();
		}
	}

	public bool IsConnected()
	{
		if (conn != null)
		{
			return conn.IsConnected();
		}
		return false;
	}

	public void CloseConnection()
	{
		lock (lockThis)
		{
			if (conn != null)
			{
				conn.Close();
			}
			conn = null;
			if (receivedPacketCache != null)
			{
				receivedPacketCache.Clear();
			}
			if (sendPacketCache != null)
			{
				sendPacketCache.Clear();
			}
		}
	}
}
