using System.Collections.Generic;
using UnityEngine;

public class CTFVersusBattleHUD : VersusBattleHUD
{
	protected Rect mPlayerIconRect;

	protected UIImage mPlayerIconImage;

	protected Rect mLeftBadgeRect;

	protected Rect mRightBadgeRect;

	protected UIImage mLeftPopulationImage;

	protected UIImage mRightPopulationImage;

	protected Rect mLeftPopulationRect;

	protected Rect mRightPopulationRect;

	protected UINumeric mLeftTeamScoreNumeric;

	protected UINumeric mRightTeamScoreNumeric;

	protected TeamName mMyTeamName;

	protected TeamName mOppositeTeamName;

	public CTFVersusBattleHUD(UIStateManager stateMgr)
	{
		base.stateMgr = stateMgr;
	}

	public override void Close()
	{
		base.Close();
		GameUIManager.GetInstance().RemoveAll();
	}

	public override bool Create()
	{
		CreateForCTF();
		GameUIManager.GetInstance().LoadHUD(HUDBattle.HUDType.CTF, stateMgr, this);
		stateMgr.m_UIPopupManager.Add(msgUI);
		return true;
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}

	protected override void UpdateAllHUDWhenWaitingRebirth()
	{
		base.UpdateAllHUDWhenWaitingRebirth();
		UpdateTeamScores();
	}

	protected override void UpdateAllHUDWhenFinish()
	{
		base.UpdateAllHUDWhenFinish();
		UpdateTeamScores();
	}

	public override void AddWhoKillsWho(int killerID, HUDAction action, int killedID)
	{
		base.AddWhoKillsWho(killerID, action, killedID);
		UpdateTeamScores();
	}

	protected override void UpdateWinValue()
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
			if (gameWorld.LastFlagInPlayerID == -1)
			{
				mClockImage.SetColor(Color.white);
			}
			else
			{
				Player playerByUserID = gameWorld.GetPlayerByUserID(gameWorld.LastFlagInPlayerID);
				if (playerByUserID == null)
				{
					mClockImage.SetColor(Color.white);
				}
				else
				{
					mClockImage.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)playerByUserID.Team]);
				}
			}
		}
		else
		{
			Player playerByUserID2 = gameWorld.GetPlayerByUserID(gameWorld.FlagInPlayerID);
			if (playerByUserID2 == null)
			{
				mFlagTimeValueNumeric.SetColor(Color.white);
				mClockImage.SetColor(Color.white);
			}
			else
			{
				mFlagTimeValueNumeric.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)playerByUserID2.Team]);
				mClockImage.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)playerByUserID2.Team]);
			}
		}
		UpdateWinScore();
	}

	protected override void UpdateWinScore()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		VSBattleInformation battleInfo = GameApp.GetInstance().GetGameWorld().BattleInfo;
		if (battleInfo != null)
		{
			short winValue = Lobby.GetInstance().WinValue;
			int num = battleInfo.TeamScores[0];
			if (battleInfo.TeamScores[1] > num)
			{
				num = battleInfo.TeamScores[1];
			}
			mWinValueNumeric.SetNumeric(unitUI, 5, num + "/" + winValue);
		}
	}

	protected void UpdateTeamScores()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		if (unitUI == null)
		{
			return;
		}
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		VSBattleInformation battleInfo = GameApp.GetInstance().GetGameWorld().BattleInfo;
		short winValue = Lobby.GetInstance().WinValue;
		if (battleInfo != null)
		{
			if (mLeftTeamScoreNumeric != null)
			{
				mLeftTeamScoreNumeric.SetNumeric(unitUI, 4, battleInfo.TeamScores[(int)mMyTeamName] + "/" + winValue);
				mLeftTeamScoreNumeric.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mMyTeamName]);
			}
			if (mRightTeamScoreNumeric != null)
			{
				mRightTeamScoreNumeric.SetNumeric(unitUI, 4, battleInfo.TeamScores[(int)mOppositeTeamName] + "/" + winValue);
				mRightTeamScoreNumeric.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mOppositeTeamName]);
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
		int num = 1;
		int num2 = 0;
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null)
			{
				if (player.IsSameTeam(item))
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		mLeftPopulationImage.SetTexture(unitUI, 9, num + 4 + 1);
		mRightPopulationImage.SetTexture(unitUI, 9, num2);
		mLeftPopulationImage.Rect = mLeftPopulationRect;
		mRightPopulationImage.Rect = mRightPopulationRect;
		mLeftPopulationImage.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mMyTeamName]);
		mRightPopulationImage.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mOppositeTeamName]);
	}

	public override void DoAutoBalance()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[0];
		UnitUI unitUI2 = Res2DManager.GetInstance().vUI[27];
		if (unitUI != null && unitUI2 != null)
		{
			mMyTeamName = player.Team;
			mPlayerIconImage.SetTexture(unitUI2, 1, player.GetSeatID());
			mPlayerIconImage.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mMyTeamName]);
			mPlayerIconImage.Rect = mPlayerIconRect;
			if (mMyTeamName == TeamName.Blue)
			{
				mOppositeTeamName = TeamName.Red;
			}
			else
			{
				mOppositeTeamName = TeamName.Blue;
			}
			mLeftTeamScoreNumeric.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mMyTeamName]);
			mRightTeamScoreNumeric.SetColor(UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)mOppositeTeamName]);
			UpdatePopulation();
			if (mVSRebirth != null)
			{
				mVSRebirth.DoAutoBalance();
			}
		}
	}
}
