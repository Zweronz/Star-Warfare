using System.Collections.Generic;
using UnityEngine;

internal class GetRoomListResponse : Response
{
	private List<Room> roomList = new List<Room>();

	private byte recommendRoomNumber;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		byte b = bytesBuffer.ReadByte();
		recommendRoomNumber = bytesBuffer.ReadByte();
		for (int i = 0; i < b; i++)
		{
			short roomID = bytesBuffer.ReadShort();
			string roomName = bytesBuffer.ReadString();
			byte mapID = bytesBuffer.ReadByte();
			byte gameMode = bytesBuffer.ReadByte();
			byte maxPlayerNum = bytesBuffer.ReadByte();
			byte joinedPlayer = bytesBuffer.ReadByte();
			byte b2 = bytesBuffer.ReadByte();
			short ping = bytesBuffer.ReadShort();
			byte minJoinRankID = bytesBuffer.ReadByte();
			byte maxJoinRankID = bytesBuffer.ReadByte();
			short roomPassword = -1;
			if (b2 == 1)
			{
				roomPassword = bytesBuffer.ReadShort();
			}
			Room room = new Room(maxPlayerNum);
			room.setRoomID(roomID);
			room.setRoomName(roomName);
			room.setMapID(mapID);
			room.setGameMode(gameMode);
			room.setJoinedPlayer(joinedPlayer);
			room.setHasPassword(b2);
			room.setPing(ping);
			room.setMinJoinRankID(minJoinRankID);
			room.setMaxJoinRankID(maxJoinRankID);
			room.setRoomPassword(roomPassword);
			roomList.Add(room);
		}
	}

	public override void ProcessLogic()
	{
		GameObject gameObject = GameObject.Find("MultiMenu");
		if (gameObject != null)
		{
			MultiMenuScript component = gameObject.GetComponent<MultiMenuScript>();
			if (component != null)
			{
				component.SetRoomList(roomList, recommendRoomNumber);
			}
		}
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		robot.uiScript.ClearRoom();
		foreach (Room room in roomList)
		{
			if (room.getJoinedPlayer() < room.getMaxPlayer())
			{
				RobotRoom robotRoom = new RobotRoom();
				robotRoom.roomID = room.getRoomID();
				robotRoom.rrState = RobotRoomState.Idle;
				robot.uiScript.AddRoom(robotRoom);
			}
		}
	}
}
