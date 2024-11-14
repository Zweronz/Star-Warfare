using System.Collections.Generic;
using UnityEngine;

public class FFAVersusBattleHUD : VersusBattleHUD
{
	protected VersusPlayerScoreHUD mPlayerScore;

	protected VersusPlayerScoreHUD mTopPlayerScore;

	protected UIImage mTopTitleImage;

	protected UIImage mPopulationImage;

	protected Rect mPopulationRect;

	public FFAVersusBattleHUD(UIStateManager stateMgr)
	{
		base.stateMgr = stateMgr;
	}

	public override bool Create()
	{
		base.Create();
		GameUIManager.GetInstance().LoadHUD(HUDBattle.HUDType.FFA, stateMgr, this);
		stateMgr.m_UIPopupManager.Add(msgUI);
		return true;
	}

	public override void Close()
	{
		base.Close();
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}

	protected override void UpdateAllHUDWhenWaitingRebirth()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateTopScore();
	}

	protected override void UpdateAllHUDWhenFinish()
	{
		base.UpdateAllHUDWhenFinish();
		UpdatePlayerScore();
	}

	protected override void UpdateWinScore()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null || mShowClock)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			VSBattleInformation battleInfo = GameApp.GetInstance().GetGameWorld().BattleInfo;
			if (battleInfo != null)
			{
				short winValue = Lobby.GetInstance().WinValue;
				mWinValueNumeric.SetNumeric(unitUI, 5, battleInfo.TopScore.score + "/" + winValue);
			}
		}
	}

	protected void UpdatePlayerScore()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			Player player = gameWorld.GetPlayer();
			if (player != null && mPlayerScore != null)
			{
				mPlayerScore.SetScore(player.VSStatistics.Score);
			}
		}
	}

	protected void UpdateTopScore()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[27];
		if (unitUI == null || unitUI2 == null)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			VSBattleInformation battleInfo = GameApp.GetInstance().GetGameWorld().BattleInfo;
			if (battleInfo != null && mTopPlayerScore != null)
			{
				mTopPlayerScore.SetIconID(battleInfo.TopScore.seatID);
				mTopPlayerScore.SetIconColor(UIConstant.COLOR_PLAYER_ICONS[battleInfo.TopScore.seatID]);
				mTopPlayerScore.SetScore(battleInfo.TopScore.score);
			}
		}
	}

	protected override void UpdatePopulation()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		int num = 0;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null)
			{
				num++;
			}
		}
		mPopulationImage.SetTexture(unitUI, 8, num);
		mPopulationImage.SetColor(UIConstant.COLOR_POPULATION);
		mPopulationImage.Rect = mPopulationRect;
	}
}
