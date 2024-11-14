using UnityEngine;

internal class SendCMIVSResponse : Response
{
	protected int m_redScore;

	protected int m_blueScore;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_redScore = bytesBuffer.ReadInt();
		m_blueScore = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			if (player != null && gameWorld.BattleInfo != null && gameWorld.BattleInfo.TeamScores != null)
			{
				gameWorld.BattleInfo.TeamScores[0] = m_blueScore;
				gameWorld.BattleInfo.TeamScores[1] = m_redScore;
				gameWorld.BattleInfo.TopScore.score = Mathf.Max(m_blueScore, m_redScore);
			}
		}
	}
}
