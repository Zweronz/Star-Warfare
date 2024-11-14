using System.Collections.Generic;

internal class GetCMISceneStateResponse : Response
{
	protected int m_blueScore;

	protected int m_redScore;

	protected int m_hpSmall;

	protected int m_hpMiddle;

	protected int m_hpLarge;

	protected int m_hpRandom;

	protected short m_scoreSmall;

	protected short m_scoreMiddle;

	protected short m_scoreLarge;

	protected short m_scoreRandomPositive;

	protected short m_scoreRandomNegative;

	protected List<CMIGift> m_gifts = new List<CMIGift>();

	protected byte m_specialGiftType;

	protected short m_specialGiftId;

	protected short m_specialGiftActive;

	protected byte m_specialGiftPointId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_blueScore = bytesBuffer.ReadInt();
		m_redScore = bytesBuffer.ReadInt();
		m_hpSmall = bytesBuffer.ReadInt();
		m_hpMiddle = bytesBuffer.ReadInt();
		m_hpLarge = bytesBuffer.ReadInt();
		m_hpRandom = bytesBuffer.ReadInt();
		m_scoreSmall = bytesBuffer.ReadShort();
		m_scoreMiddle = bytesBuffer.ReadShort();
		m_scoreLarge = bytesBuffer.ReadShort();
		m_scoreRandomPositive = bytesBuffer.ReadShort();
		m_scoreRandomNegative = bytesBuffer.ReadShort();
		m_specialGiftType = bytesBuffer.ReadByte();
		m_specialGiftId = bytesBuffer.ReadShort();
		m_specialGiftPointId = bytesBuffer.ReadByte();
		m_specialGiftActive = bytesBuffer.ReadByte();
		int num = bytesBuffer.ReadByte();
		for (int i = 0; i < num; i++)
		{
			int type = bytesBuffer.ReadByte();
			int id = bytesBuffer.ReadShort();
			byte pointId = bytesBuffer.ReadByte();
			CMIGift cMIGift = new CMIGift(id, (CMIGiftType)type);
			cMIGift.SetPointId(pointId);
			m_gifts.Add(cMIGift);
		}
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player != null)
		{
			if (gameWorld.BattleInfo != null && gameWorld.BattleInfo.TeamScores != null)
			{
				gameWorld.BattleInfo.TeamScores[0] = m_blueScore;
				gameWorld.BattleInfo.TeamScores[1] = m_redScore;
			}
			CMIConfig.HP_SMALL = m_hpSmall;
			CMIConfig.HP_MIDDLE = m_hpMiddle;
			CMIConfig.HP_LARGE = m_hpLarge;
			CMIConfig.HP_RANDOM = m_hpRandom;
			CMIConfig.SCORE_SMALL = m_scoreSmall;
			CMIConfig.SCORE_MIDDLE = m_scoreMiddle;
			CMIConfig.SCORE_LARGE = m_scoreLarge;
			CMIConfig.SCORE_RANDOM_POSITIVE = m_scoreRandomPositive;
			CMIConfig.SCORE_RANDOM_NEGATIVE = m_scoreRandomNegative;
			gameWorld.SetCMISpecialGift((CMIGiftType)m_specialGiftType, m_specialGiftId, m_specialGiftActive == 1, m_specialGiftPointId);
			gameWorld.SetCMIGifts(m_gifts);
		}
	}
}
