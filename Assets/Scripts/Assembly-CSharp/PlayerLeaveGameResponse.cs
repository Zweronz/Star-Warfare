internal class PlayerLeaveGameResponse : Response
{
	protected int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			gameWorld.RemoveRemotePlayer(playerID);
			gameWorld.VSTimeStopResume();
		}
	}
}
