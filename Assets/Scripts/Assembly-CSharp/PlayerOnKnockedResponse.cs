internal class PlayerOnKnockedResponse : Response
{
	private int playerID;

	private short speed;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		speed = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID != channelID)
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.OnKnocked((float)speed / 100f);
			}
		}
	}
}
