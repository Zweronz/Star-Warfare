internal class SetPlayerHealthResponse : Response
{
	protected int m_PlayerID;

	protected int m_Hp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_PlayerID = bytesBuffer.ReadInt();
		m_Hp = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player playerByUserID = gameWorld.GetPlayerByUserID(m_PlayerID);
			if (playerByUserID != null)
			{
				playerByUserID.Hp = m_Hp;
			}
		}
	}
}
