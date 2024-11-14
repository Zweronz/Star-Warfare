public class SurvivalCoopHUDBattle : CoopHUDBattle
{
	protected override void OnCreate()
	{
		base.OnCreate();
		HUDBattle hUDBattle = (HUDBattle)GetGameUI();
		hUDBattle.StateSurvival.SetActive(true);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UpdateSurvivalKills();
	}

	private void UpdateSurvivalKills()
	{
		UnitUI unitUI = Res2DManager.GetInstance().vUI[17];
		if (unitUI != null)
		{
			UserStateUI.GetInstance().UpdateSurvivalKills(mPlayer.Kills);
		}
	}
}
