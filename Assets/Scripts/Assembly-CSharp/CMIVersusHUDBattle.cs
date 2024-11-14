public class CMIVersusHUDBattle : VersusHUDBattle
{
	private GameWorld mGameWorld;

	protected override void OnCreate()
	{
		base.OnCreate();
		mGameWorld = GameApp.GetInstance().GetGameWorld();
		mHudBattle.StatePopulation.SetActive(true);
		mHudBattle.StateTeamScore.SetActive(true);
		mHudBattle.StateWinScore.SetActive(true);
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
	}
}
