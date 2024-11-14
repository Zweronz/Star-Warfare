internal class PlayerUploadStatisticsResponse : Response
{
	protected int m_playerID;

	protected int m_killCash;

	protected int m_pickupCash;

	protected int m_pickupEnegy;

	protected int m_comboCash;

	protected int m_bossCash;

	protected int m_bossMithril;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_playerID = bytesBuffer.ReadInt();
		m_killCash = bytesBuffer.ReadInt();
		m_pickupCash = bytesBuffer.ReadInt();
		m_pickupEnegy = bytesBuffer.ReadInt();
		m_comboCash = bytesBuffer.ReadInt();
		m_bossCash = bytesBuffer.ReadInt();
		m_bossMithril = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(m_playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.SetMonsterCash(m_killCash);
				remotePlayerByUserID.SetPickupCash(m_pickupCash);
				remotePlayerByUserID.SetPickupEnegy(m_pickupEnegy);
				remotePlayerByUserID.SetBounsCash(m_comboCash);
				remotePlayerByUserID.SetBossCash(m_bossCash);
				remotePlayerByUserID.SetBossMithril(m_bossMithril);
			}
		}
	}
}
