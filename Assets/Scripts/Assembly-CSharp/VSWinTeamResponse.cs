internal class VSWinTeamResponse : Response
{
	protected byte teamId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		teamId = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		TeamName teamName = (TeamName)teamId;
		if (gameWorld.GetPlayer().Team == teamName)
		{
			gameWorld.GetPlayer().AddTempBuff(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG, 10, 0.5f);
		}
		AudioManager.GetInstance().PlaySound("Audio/pickup/win_flag");
		foreach (RemotePlayer remotePlayer in gameWorld.GetRemotePlayers())
		{
			if (remotePlayer != null && remotePlayer.Team == teamName)
			{
				remotePlayer.AddTempBuff(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG, 10, 0.5f);
			}
		}
	}
}
