using UnityEngine;

internal class CreateRoomResponse : Response
{
	public short roomID;

	public byte gameMode;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		roomID = bytesBuffer.ReadShort();
		gameMode = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (roomID == -1)
		{
			return;
		}
		Lobby.GetInstance().SetCurrentRoomID(roomID);
		Lobby.GetInstance().IsMasterPlayer = true;
		if (Lobby.GetInstance().CurrentRoomMaxPlayer != 1)
		{
			if (!GameApp.GetInstance().GetGameMode().IsCoopMode((Mode)gameMode))
			{
				return;
			}
			GameObject gameObject = GameObject.Find("MultiMenu");
			if (gameObject != null)
			{
				MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
				if (component != null)
				{
					component.GotoReadyGame();
				}
			}
		}
		else
		{
			Debug.Log("Only one player, ignore readygame ui");
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		if (roomID != -1)
		{
			robot.lobby.SetCurrentRoomID(roomID);
			robot.lobby.IsMasterPlayer = true;
			robot.sendingCreatingRoomRequest = false;
			robot.SetState(robot.MasterInRoomState);
			Debug.Log("Robot Created Room:" + roomID);
			RobotRoom robotRoom = new RobotRoom();
			robotRoom.roomID = roomID;
			robotRoom.rrState = RobotRoomState.Idle;
			robot.uiScript.AddRoom(robotRoom);
		}
	}
}
