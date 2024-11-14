internal class GetVIPSceneStateResponse : Response
{
	protected int m_redScore;

	protected int m_blueScore;

	protected int m_vipPlayerId;

	protected int m_vipTime;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_redScore = bytesBuffer.ReadInt();
		m_blueScore = bytesBuffer.ReadInt();
		m_vipPlayerId = bytesBuffer.ReadInt();
		m_vipTime = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			if (player != null)
			{
				gameWorld.BattleInfo.TeamScores[0] = m_blueScore;
				gameWorld.BattleInfo.TeamScores[1] = m_redScore;
				gameWorld.VIPClock.SetCurrentTime(m_vipTime / 1000);
				gameWorld.VIPInPlayerID = m_vipPlayerId;
			}
		}
	}
}
