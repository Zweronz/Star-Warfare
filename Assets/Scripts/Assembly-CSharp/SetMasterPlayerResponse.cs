internal class SetMasterPlayerResponse : Response
{
	public int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		if (playerID == Lobby.GetInstance().GetChannelID())
		{
			Lobby.GetInstance().IsMasterPlayer = true;
		}
		else
		{
			Lobby.GetInstance().IsMasterPlayer = false;
		}
	}
}
