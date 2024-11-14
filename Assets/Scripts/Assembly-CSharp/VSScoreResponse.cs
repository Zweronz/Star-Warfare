internal class VSScoreResponse : Response
{
	protected byte id;

	protected int score;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		id = bytesBuffer.ReadByte();
		score = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (player != null)
		{
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
			{
				gameWorld.BattleInfo.TeamScores[id] = score;
				gameWorld.BattleInfo.TopScore.score = ((gameWorld.BattleInfo.TopScore.score <= score) ? score : gameWorld.BattleInfo.TopScore.score);
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA || GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
			{
				gameWorld.BattleInfo.TopScore.seatID = id;
				gameWorld.BattleInfo.TopScore.score = ((gameWorld.BattleInfo.TopScore.score <= score) ? score : gameWorld.BattleInfo.TopScore.score);
			}
		}
	}
}
