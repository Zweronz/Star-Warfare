using System.Collections.Generic;
using UnityEngine;

public class LLVersusBattleHUD : VersusBattleHUD
{
	protected VersusPlayerScoreHUD mPlayerScore;

	protected VersusPlayerScoreHUD mTopPlayerScore;

	protected UIImage mTopTitleImage;

	protected UIImage mPopulationImage;

	protected Rect mPopulationRect;

	public LLVersusBattleHUD(UIStateManager stateMgr)
	{
		base.stateMgr = stateMgr;
	}

	public override bool Create()
	{
		CreateForCTF();
		GameUIManager.GetInstance().LoadHUD(HUDBattle.HUDType.LL, stateMgr, this);
		stateMgr.m_UIPopupManager.Add(msgUI);
		return true;
	}

	public override void Close()
	{
		base.Close();
		if (mPlayerScore != null)
		{
			mPlayerScore.Destroy();
		}
		if (mTopPlayerScore != null)
		{
			mTopPlayerScore.Destroy();
		}
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}

	protected override void UpdateAllHUDWhenWaitingRebirth()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateWinScore();
		UpdateTopScore();
	}

	protected override void UpdateAllHUDWhenFinish()
	{
		base.UpdateAllHUDWhenFinish();
		UpdatePlayerScore();
		UpdateTopScore();
	}

	protected override void UpdateWinScore()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		int num = 40 - GameApp.GetInstance().GetGameWorld().FlagClock.GetCurrentTimeSeconds();
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num / 60;
		int num3 = num - num2 * 60;
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("CathTheFlag");
		if (gameObject == null && gameWorld.FlagInPlayerID == -1)
		{
			mFlagTimeValueNumeric.SetNumeric(unitUI, 5, "--:--");
		}
		else
		{
			mFlagTimeValueNumeric.SetNumeric(unitUI, 5, string.Format("{0:D2}", num2) + ":" + string.Format("{0:D2}", num3));
		}
		float num4 = 1f;
		float currentTime = GameApp.GetInstance().GetGameWorld().FlagClock.GetCurrentTime();
		float num5 = 40f - currentTime;
		if (num5 < 10f && num5 > 0f)
		{
			num4 = 0.7f + (currentTime - (float)Mathf.FloorToInt(currentTime)) * 0.5f;
		}
		mFlagTimeValueNumeric.SetSize(new Vector2((int)(mFlagTimeSize.x * num4), (int)(mFlagTimeSize.y * num4)));
		if (gameWorld.FlagInPlayerID == -1)
		{
			mFlagTimeValueNumeric.SetColor(Color.white);
			mClockImage.SetColor(Color.white);
			return;
		}
		Player playerByUserID = gameWorld.GetPlayerByUserID(gameWorld.FlagInPlayerID);
		if (playerByUserID == null)
		{
			mFlagTimeValueNumeric.SetColor(Color.white);
			mClockImage.SetColor(Color.white);
		}
		else
		{
			mFlagTimeValueNumeric.SetColor(UIConstant.COLOR_PLAYER_ICONS[playerByUserID.GetSeatID()]);
			mClockImage.SetColor(UIConstant.COLOR_PLAYER_ICONS[playerByUserID.GetSeatID()]);
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
				short winValue = Lobby.GetInstance().WinValue;
				mPlayerScore.SetScore(player.VSStatistics.Score, winValue);
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
				short winValue = Lobby.GetInstance().WinValue;
				mTopPlayerScore.SetScore(battleInfo.TopScore.score, winValue);
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
