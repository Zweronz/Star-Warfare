public class CTFVersusHUDBattle : VersusHUDBattle
{
	private GameWorld mGameWorld;

	protected override void OnCreate()
	{
		base.OnCreate();
		mGameWorld = GameApp.GetInstance().GetGameWorld();
		mHudBattle.StatePopulation.SetActive(true);
		mHudBattle.StateTeamScore.SetActive(true);
		UserStateUI.GetInstance().SetTimerType(UserStateUI.TimeInfoUI.TimeType.Flag);
		mHudBattle.StateWinTime.SetActive(true);
		mHudBattle.StateWinScore.SetActive(true);
		UserStateUI.GetInstance().SetTeam(mPlayer.Team);
		mHudBattle.STContainer.Reposition();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		mHudBattle.HandleRemotePlayer();
		UpdateBattleInformation();
		UpdateWinTime();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		mGameWorld = null;
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
		int value = 40 - GameApp.GetInstance().GetGameWorld().FlagClock.GetCurrentTimeSeconds();
		int team = -1;
		Player playerByUserID = mGameWorld.GetPlayerByUserID(mGameWorld.FlagInPlayerID);
		if (playerByUserID != null)
		{
			team = (int)playerByUserID.Team;
		}
		UserStateUI.GetInstance().UpdateTimerInfo(team, value);
	}
}
