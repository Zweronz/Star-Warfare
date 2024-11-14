internal class SendPlayerShootAngleVResponse : Response
{
	public int playerID;

	public short angleV;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		angleV = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.TargetAngleV = angleV;
			}
		}
	}
}
