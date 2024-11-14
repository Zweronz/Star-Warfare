public class VIPVersusHUDBattle : VersusHUDBattle
{
	private GameWorld mGameWorld;

	protected override void OnCreate()
	{
		base.OnCreate();
		mGameWorld = GameApp.GetInstance().GetGameWorld();
		mHudBattle.StatePopulation.SetActive(true);
		mHudBattle.StateTeamScore.SetActive(true);
		mHudBattle.StateWinScore.SetActive(true);
		UserStateUI.GetInstance().SetTimerType(UserStateUI.TimeInfoUI.TimeType.VIP, true);
		mHudBattle.StateWinTime.SetActive(true);
		mHudBattle.STContainer.Reposition();
		UserStateUI.GetInstance().SetTeam(mPlayer.Team);
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
		int value = 50 - GameApp.GetInstance().GetGameWorld().VIPClock.GetCurrentTimeSeconds();
		int team = -1;
		float fillAmount = 0f;
		Player playerByUserID = mGameWorld.GetPlayerByUserID(mGameWorld.VIPInPlayerID);
		if (playerByUserID != null)
		{
			team = (int)playerByUserID.Team;
			fillAmount = (float)playerByUserID.Hp / (float)playerByUserID.MaxHp;
		}
		else
		{
			value = -1;
		}
		UserStateUI.GetInstance().UpdateTimerInfo(team, value, fillAmount);
	}
}
