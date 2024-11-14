using System;
using UnityEngine;

internal class PlayerLoginResponse : Response
{
	public const byte ACCOUNT_PASS = 0;

	public const byte ACCOUNT_NOT_EXIST = 1;

	public const byte ACCOUNT_PASSWORD_INCORRECT = 2;

	public const byte ACCOUNT_LOCK = 3;

	public const byte ACCOUNT_BLOCKED = 4;

	public const byte VERSION_MISMATCH = 5;

	public const byte ACCOUNT_WAS_DELETED = 6;

	public const byte LOGIN_FAIL = 7;

	public const byte SERVER_BUSYNESS = 8;

	public const byte SERVER_MAINTAINEANCE = 9;

	public const byte GUEST_LOGIN = 11;

	private bool loginSuccess;

	private int channelID;

	private string ip;

	private int port;

	private int userID;

	private int mithril;

	private short timeSpan;

	private int result;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		result = bytesBuffer.ReadByte();
		loginSuccess = false;
		string empty = string.Empty;
		switch (result)
		{
		case 0:
		{
			loginSuccess = true;
			userID = bytesBuffer.ReadInt();
			empty = bytesBuffer.ReadString();
			mithril = bytesBuffer.ReadInt();
			timeSpan = bytesBuffer.ReadShort();
			string[] array = empty.Split(':');
			ip = array[0];
			port = Convert.ToInt32(array[1]);
			break;
		}
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		case 11:
		{
			loginSuccess = true;
			empty = bytesBuffer.ReadString();
			string[] array = empty.Split(':');
			ip = array[0];
			port = Convert.ToInt32(array[1]);
			break;
		}
		case 10:
			break;
		}
	}

	public override void ProcessLogic()
	{
		if (loginSuccess)
		{
			Debug.Log("PlayerLoginResponse: " + ip);
			GameObject gameObject = GameObject.Find("StartMenu");
			if (!(gameObject != null))
			{
				return;
			}
			StartMenuScript component = gameObject.GetComponent<StartMenuScript>();
			if (!(component != null))
			{
				return;
			}
			if (result == 11)
			{
				GameApp.GetInstance().DestoryNetWork();
				LoginGameServer();
				UserState userState = GameApp.GetInstance().GetUserState();
				GuestLoginRequest request = new GuestLoginRequest(Lobby.GetInstance().GetUserName(), userID, 1, userState.GetBossDate(), userState.GetSuccBossStage(), userState.GetSuccBossStageGetMithril());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				UserData userData = component.CreateUserData();
				userData.mithril = mithril;
				UserState userState2 = GameApp.GetInstance().GetUserState();
				GameApp.GetInstance().DestoryNetWork();
				LoginGameServer();
				if (timeSpan > 1)
				{
					userState2.SetTimeSpan(0);
					component.SetGiveRewards(true);
				}
				else if (timeSpan == 1)
				{
					component.SetGiveRewards(true);
				}
				else
				{
					component.SetGiveRewards(false);
				}
				RoleLoginRequest request2 = new RoleLoginRequest(Lobby.GetInstance().GetUserName(), userID, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
			Debug.Log("login game server :" + ip + ":" + port);
			return;
		}
		GameApp.GetInstance().DestoryNetWork();
		GameObject gameObject2 = GameObject.Find("StartMenu");
		if (!(gameObject2 != null))
		{
			return;
		}
		StartMenuScript component2 = gameObject2.GetComponent<StartMenuScript>();
		if (component2 != null)
		{
			switch (result)
			{
			case 9:
				component2.PopupServerMessage(24, MessageBoxUI.EVENT_NET_SERVER_MIANTENANCE);
				break;
			case 3:
			case 4:
			case 7:
				component2.PopupServerMessage(23, MessageBoxUI.EVENT_NET_ACCOUNT_LOCKED);
				break;
			case 5:
				component2.PopupServerMessage(25, MessageBoxUI.EVENT_NET_VERSION_MISMATCH);
				break;
			case 6:
			case 8:
				break;
			}
		}
	}

	public void LoginGameServer()
	{
		GameApp.GetInstance().CreateNetwork(ip, port);
	}

	public override void ProcessRobotLogic(Robot robot)
	{
	}
}
