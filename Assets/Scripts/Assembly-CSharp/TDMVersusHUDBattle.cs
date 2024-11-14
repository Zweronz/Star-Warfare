public class TDMVersusHUDBattle : VersusHUDBattle
{
	private bool mShowClock;

	protected override void OnCreate()
	{
		base.OnCreate();
		mHudBattle.StatePopulation.SetActive(true);
		mHudBattle.StateTeamScore.SetActive(true);
		if (Lobby.GetInstance().WinCondition == 0)
		{
			mShowClock = true;
			UserStateUI.GetInstance().SetTimerType(UserStateUI.TimeInfoUI.TimeType.Clock);
			mHudBattle.StateWinTime.SetActive(true);
		}
		else
		{
			mShowClock = false;
			mHudBattle.StateWinScore.SetActive(true);
		}
		UserStateUI.GetInstance().SetTeam(mPlayer.Team);
		mHudBattle.STContainer.Reposition();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		mHudBattle.HandleRemotePlayer();
		UpdateBattleInformation();
		if (mShowClock)
		{
			UpdateWinTime();
		}
	}

	public override void DoAutoBalance()
	{
		base.DoAutoBalance();
		UserStateUI.GetInstance().SetTeam(mPlayer.Team);
		UserStateUI.GetInstance().SetPlayerSeatID(mPlayer.GetSeatID());
		mHudBattle.StateWaitVSRebirth.DoAutoBalance();
	}

	private void UpdateWinTime()
	{
		int value = Lobby.GetInstance().WinValue * 60 - Lobby.GetInstance().GetVSClock().GetCurrentTimeSeconds();
		UserStateUI.GetInstance().UpdateTimerInfo(-1, value);
	}
}
