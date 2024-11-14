using UnityEngine;

internal class JoinRoomResponse : Response
{
	protected short roomID;

	protected byte mapID;

	protected byte gameMode;

	protected byte winCondition;

	protected short winValue;

	protected byte autoBalance;

	protected byte seatID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		mapID = bytesBuffer.ReadByte();
		if (roomID != -1)
		{
			gameMode = bytesBuffer.ReadByte();
			winCondition = bytesBuffer.ReadByte();
			winValue = bytesBuffer.ReadShort();
			autoBalance = bytesBuffer.ReadByte();
			seatID = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		if (roomID != -1)
		{
			Lobby.GetInstance().IsMasterPlayer = false;
			Lobby.GetInstance().SetCurrentRoomID(roomID);
			Lobby.GetInstance().SetCurrentRoomMapID(mapID);
			GameApp.GetInstance().GetGameMode().ModePlay = (Mode)gameMode;
			Lobby.GetInstance().WinCondition = winCondition;
			Lobby.GetInstance().WinValue = winValue;
			Lobby.GetInstance().AutoBalance = autoBalance;
			Lobby.GetInstance().CurrentSeatID = seatID;
			Lobby.GetInstance().GetVSClock().SetTotalGameSeconds(winValue * 60);
			GameObject gameObject = GameObject.Find("MultiMenu");
			if (gameObject != null)
			{
				MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
				if (component != null)
				{
					component.GotoReadyGame();
				}
			}
			Debug.Log("JoinRoomResponse: " + GameApp.GetInstance().GetGameMode().ModePlay);
			return;
		}
		Lobby.GetInstance().SetCurrentRoomID(-1);
		GameObject gameObject2 = GameObject.Find("MultiMenu");
		if (gameObject2 != null)
		{
			MultiMenuScript component2 = gameObject2.GetComponent<MultiMenuScript>();
			if (component2 != null)
			{
				component2.HideNetLoading();
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		if (roomID != -1)
		{
			robot.SetState(robot.InRoomState);
			Debug.Log("Robot " + robot.userName + " Joined Room.");
		}
		else
		{
			Debug.Log("Robot " + robot.userName + " Joined Room Failed.");
		}
	}
}
