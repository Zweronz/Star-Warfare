internal class PlayerOnHitResponse : Response
{
	protected int playerID;

	protected int m_hp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			player.UnderAttackSetHP(m_hp);
			float skill = player.GetSkills().GetSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT);
			if (skill > 0f)
			{
				player.AddTempBuff(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT, 2, skill);
			}
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.UnderAttackSetHP(m_hp);
			float skill2 = remotePlayerByUserID.GetSkills().GetSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT);
			if (skill2 > 0f)
			{
				remotePlayerByUserID.AddTempBuff(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT, 2, skill2);
			}
		}
	}
}
