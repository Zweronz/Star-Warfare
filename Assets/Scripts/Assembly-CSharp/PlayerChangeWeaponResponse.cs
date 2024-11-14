internal class PlayerChangeWeaponResponse : Response
{
	protected int playerID;

	protected byte bagIndex;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		bagIndex = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.ChangeWeaponInBag(bagIndex);
			}
		}
	}
}
