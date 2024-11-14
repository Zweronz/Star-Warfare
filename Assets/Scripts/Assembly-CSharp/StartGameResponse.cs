using UnityEngine;

internal class StartGameResponse : Response
{
	public short roomID;

	public byte gameMode;

	public byte mapID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		gameMode = bytesBuffer.ReadByte();
		mapID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameApp.GetInstance().GetGameMode().ModePlay = (Mode)gameMode;
		GameApp.GetInstance().GetUserState().SetNetStage(mapID);
		GameObject gameObject = GameObject.Find("MultiMenu");
		if (gameObject != null)
		{
			MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
			if (component != null && Lobby.GetInstance().GetCurrentRoomID() != -1)
			{
				component.StartGame(mapID);
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		robot.sendingStartGameRequest = false;
		robot.SetState(robot.PlayingState);
	}
}
