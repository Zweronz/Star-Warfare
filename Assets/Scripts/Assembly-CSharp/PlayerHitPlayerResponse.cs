using UnityEngine;

internal class PlayerHitPlayerResponse : Response
{
	protected int playerID;

	protected int m_hp;

	protected bool block;

	protected bool criticalDamage;

	protected byte weaponType;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
		block = bytesBuffer.ReadBool();
		criticalDamage = bytesBuffer.ReadBool();
		weaponType = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			if (block)
			{
				player.ShowDefent();
				return;
			}
			player.UnderAttackSetHP(m_hp);
			if (weaponType == 18 || weaponType == 38)
			{
				player.SlowDownEffect();
			}
			if (weaponType == 39)
			{
				player.SetAimEffect();
			}
			float skill = player.GetSkills().GetSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT);
			if (skill > 0f)
			{
				player.AddTempBuff(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT, 2, skill);
			}
			return;
		}
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID == null)
		{
			return;
		}
		remotePlayerByUserID.UnderAttackSetHP(m_hp);
		if (!block)
		{
			if (weaponType == 18 || weaponType == 38)
			{
				remotePlayerByUserID.SlowDownEffect();
			}
			if (weaponType == 39)
			{
				remotePlayerByUserID.SetAimEffect();
			}
			float skill2 = remotePlayerByUserID.GetSkills().GetSkill(SkillsType.SPEEDUP_WHEN_GOT_HIT);
			if (skill2 > 0f)
			{
				remotePlayerByUserID.AddTempBuff(EffectsType.SPEED_BOOTH_WHEN_GOT_HIT, 2, skill2);
			}
			if (criticalDamage)
			{
				remotePlayerByUserID.CreateDeadBlood();
			}
			else if (remotePlayerByUserID.GetLastBloodEffectTimer().Ready())
			{
				gameWorld.GetHitBloodPool().CreateObject(remotePlayerByUserID.GetTransform().position + Vector3.up * 1.5f, Vector3.zero, Quaternion.identity);
				remotePlayerByUserID.GetLastBloodEffectTimer().Do();
			}
		}
	}
}
