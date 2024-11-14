public class LLVersusHUDBattle : VersusHUDBattle
{
	protected override void OnCreate()
	{
		base.OnCreate();
		mHudBattle.StatePersonalPopulation.SetActive(true);
		mHudBattle.StateLL.SetActive(true);
		UserStateUI.GetInstance().SetTimerType(UserStateUI.TimeInfoUI.TimeType.Flag);
		mHudBattle.StateWinTime.SetActive(true);
		mHudBattle.STContainer.Reposition();
		UserStateUI.GetInstance().SetTotalScore(Lobby.GetInstance().WinValue);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		mHudBattle.HandleRemotePlayer();
		UpdateWinTime();
		UpdatePlayerScore();
		UpdateBattleInformation();
	}

	private void UpdateWinTime()
	{
		int value = 40 - GameApp.GetInstance().GetGameWorld().FlagClock.GetCurrentTimeSeconds();
		UserStateUI.GetInstance().UpdateTimerInfo(-1, value);
	}

	private void UpdatePlayerScore()
	{
		UserStateUI.GetInstance().SetPlayerScore(mPlayer.VSStatistics.Score);
	}
}
