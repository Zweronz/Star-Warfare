internal class PlayerBuffResponse : Response
{
	protected byte m_buff;

	protected int m_PlayerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_buff = bytesBuffer.ReadByte();
		m_PlayerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(m_PlayerID);
		if (remotePlayerByUserID != null)
		{
			if (m_buff == 0)
			{
				remotePlayerByUserID.PowerUp(true, 0);
			}
			else if (m_buff == 1)
			{
				remotePlayerByUserID.PowerUp(true, 1);
			}
			else if (m_buff == 2)
			{
				remotePlayerByUserID.PowerUp(true, 2);
			}
			else if (m_buff == 3)
			{
				remotePlayerByUserID.PowerUp(true, 3);
			}
			else if (m_buff == 4)
			{
				remotePlayerByUserID.PowerUp(true, 4);
			}
			else if (m_buff == 5)
			{
				remotePlayerByUserID.PowerUp(true, 5);
			}
			else if (m_buff == 6)
			{
				remotePlayerByUserID.PowerUp(true, 6);
			}
			else if (m_buff == 8)
			{
				remotePlayerByUserID.PowerUp(true, 8);
			}
			else if (m_buff == 9)
			{
				remotePlayerByUserID.PowerUp(true, 6);
			}
		}
	}
}
