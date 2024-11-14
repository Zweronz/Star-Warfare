using UnityEngine;

internal class GetRoomDataResponse : Response
{
	private Room room;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		room = new Room(b);
		RoomPlayer[] allPlayers = room.GetAllPlayers();
		byte b2 = 0;
		for (int i = 0; i < b; i++)
		{
			string text = bytesBuffer.ReadString();
			if (text == null)
			{
				allPlayers[i] = null;
				bytesBuffer.ReadByte();
				continue;
			}
			byte rankID = bytesBuffer.ReadByte();
			allPlayers[i] = new RoomPlayer();
			allPlayers[i].setPlayerName(text);
			allPlayers[i].RankID = rankID;
			b2++;
		}
		room.setJoinedPlayer(b2);
		Lobby.GetInstance().SetCurrentRoom(room);
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("MultiMenu");
		if (gameObject != null)
		{
			MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
			if (component != null)
			{
				component.SetRoomData(room);
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		robot.joinedPlayer = room.getJoinedPlayer();
		robot.maxPlayer = room.getMaxPlayer();
	}
}
