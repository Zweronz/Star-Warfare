using System.Collections.Generic;

public class VSBattleInformation
{
	protected List<WhoKillsWho> whoKillsWhoList = new List<WhoKillsWho>();

	public TopScore TopScore { get; set; }

	public int[] TeamScores { get; set; }

	public VSBattleInformation()
	{
		TopScore = new TopScore();
		TeamScores = new int[2];
	}

	public void AddWhoKillsWho(string killerName, string killedName)
	{
		whoKillsWhoList.Clear();
		WhoKillsWho whoKillsWho = new WhoKillsWho();
		whoKillsWho.killerName = killerName;
		whoKillsWho.killedName = killedName;
		whoKillsWhoList.Add(whoKillsWho);
	}

	public WhoKillsWho GetLastWhoKillsWho()
	{
		if (whoKillsWhoList.Count > 0)
		{
			return whoKillsWhoList[0];
		}
		return null;
	}

	public void ClearAll()
	{
		TopScore.Clear();
		for (int i = 0; i < 2; i++)
		{
			TeamScores[i] = 0;
		}
		whoKillsWhoList.Clear();
	}
}
