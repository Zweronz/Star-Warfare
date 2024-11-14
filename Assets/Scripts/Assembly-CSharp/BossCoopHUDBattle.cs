public class BossCoopHUDBattle : CoopHUDBattle
{
	protected override void OnCreate()
	{
		base.OnCreate();
		HUDBattle hUDBattle = (HUDBattle)GetGameUI();
		hUDBattle.StateBossHp.SetActive(true);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UpdateBossHp();
	}

	private void UpdateBossHp()
	{
		Enemy boss = GameApp.GetInstance().GetGameWorld().GetBoss(0);
		UserStateUI.GetInstance().UpdateBossHpPercent((float)boss.HP / (float)boss.MaxHP);
	}
}
