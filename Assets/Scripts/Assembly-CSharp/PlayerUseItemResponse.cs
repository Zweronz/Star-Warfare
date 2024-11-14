internal class PlayerUseItemResponse : Response
{
	protected int playerID;

	protected byte itemID;

	protected byte buffValue;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		itemID = bytesBuffer.ReadByte();
		buffValue = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		float hpRate = (float)(int)buffValue * 0.01f;
		if (playerID == channelID)
		{
			player.UseItemByItemID(itemID, hpRate);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.UseItemByItemID(itemID, hpRate);
		}
	}
}
