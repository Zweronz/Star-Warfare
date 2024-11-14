internal class VSUploadStatisticsResponse : Response
{
	protected int m_playerID;

	protected short m_kills;

	protected short m_death;

	protected short m_assist;

	protected int m_score;

	protected int m_bonus;

	protected int m_cash;

	protected short m_secureFlags;

	protected short m_assistFlags;

	protected short m_assistVIP;

	protected short m_giftHitCMI;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerID = bytesBuffer.ReadInt();
		m_kills = bytesBuffer.ReadShort();
		m_death = bytesBuffer.ReadShort();
		m_assist = bytesBuffer.ReadShort();
		m_score = bytesBuffer.ReadInt();
		m_bonus = bytesBuffer.ReadInt();
		m_cash = bytesBuffer.ReadInt();
		m_secureFlags = bytesBuffer.ReadShort();
		m_assistFlags = bytesBuffer.ReadShort();
		m_assistVIP = bytesBuffer.ReadShort();
		m_giftHitCMI = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(m_playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.VSStatistics.Kills = m_kills;
				remotePlayerByUserID.VSStatistics.Death = m_death;
				remotePlayerByUserID.VSStatistics.Assist = m_assist;
				remotePlayerByUserID.VSStatistics.Score = m_score;
				remotePlayerByUserID.VSStatistics.Bonus = m_bonus;
				remotePlayerByUserID.VSStatistics.CashReward = m_cash;
				remotePlayerByUserID.VSStatistics.SecureFlags = m_secureFlags;
				remotePlayerByUserID.VSStatistics.AssistFlags = m_assistFlags;
				remotePlayerByUserID.VSStatistics.VIPAssist = m_assistVIP;
				remotePlayerByUserID.VSStatistics.CMIGiftHit = m_giftHitCMI;
			}
		}
	}
}
