public class FFAVersusHUDBattle : VersusHUDBattle
{
	private bool mShowClock;

	protected override void OnCreate()
	{
		base.OnCreate();
		mHudBattle.StatePersonalPopulation.SetActive(true);
		mHudBattle.StateFFA.SetActive(true);
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
		mHudBattle.STContainer.Reposition();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		mHudBattle.HandleRemotePlayer();
		if (mShowClock)
		{
			UpdateWinTime();
		}
		UpdatePlayerScore();
		UpdateBattleInformation();
	}

	private void UpdateWinTime()
	{
		int value = Lobby.GetInstance().WinValue * 60 - Lobby.GetInstance().GetVSClock().GetCurrentTimeSeconds();
		UserStateUI.GetInstance().UpdateTimerInfo(-1, value);
	}

	private void UpdatePlayerScore()
	{
		UserStateUI.GetInstance().SetPlayerScore(mPlayer.VSStatistics.Score);
	}
}
