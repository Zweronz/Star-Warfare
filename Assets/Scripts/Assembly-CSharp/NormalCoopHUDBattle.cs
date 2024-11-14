public class NormalCoopHUDBattle : CoopHUDBattle
{
	private new HUDBattle mHudBattle;

	private bool mIsDoubleBoss;

	protected override void OnCreate()
	{
		base.OnCreate();
		mHudBattle = (HUDBattle)GetGameUI();
		if (mUserState.GetNetStage() == Global.DOUBLE_MANTIS_STAGE)
		{
			mIsDoubleBoss = true;
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (GameApp.GetInstance().GetGameWorld().InBossBattle)
		{
			if (mIsDoubleBoss)
			{
				if (mHudBattle.StateEnemyProcess.activeSelf)
				{
					mHudBattle.StateEnemyProcess.SetActive(false);
				}
				if (mHudBattle.StateBossHp.activeSelf)
				{
					mHudBattle.StateBossHp.SetActive(false);
				}
				if (!mHudBattle.StateDoubleBossHp.activeSelf)
				{
					mHudBattle.StateDoubleBossHp.SetActive(true);
				}
				UpdateDoubleBossHp();
			}
			else
			{
				if (mHudBattle.StateEnemyProcess.activeSelf)
				{
					mHudBattle.StateEnemyProcess.SetActive(false);
				}
				if (!mHudBattle.StateBossHp.activeSelf)
				{
					mHudBattle.StateBossHp.SetActive(true);
				}
				if (mHudBattle.StateDoubleBossHp.activeSelf)
				{
					mHudBattle.StateDoubleBossHp.SetActive(false);
				}
				UpdateBossHp();
			}
		}
		else
		{
			if (!mHudBattle.StateEnemyProcess.activeSelf)
			{
				mHudBattle.StateEnemyProcess.SetActive(true);
			}
			if (mHudBattle.StateBossHp.activeSelf)
			{
				mHudBattle.StateBossHp.SetActive(false);
			}
			if (mHudBattle.StateDoubleBossHp.activeSelf)
			{
				mHudBattle.StateDoubleBossHp.SetActive(false);
			}
			UpdateWaveProcess();
		}
	}

	private void UpdateWaveProcess()
	{
		if (GameApp.GetInstance().GetGameWorld().TotalEnemyCount < 1)
		{
			UserStateUI.GetInstance().SetWaveProcess(0f);
		}
		else
		{
			UserStateUI.GetInstance().SetWaveProcess((float)GameApp.GetInstance().GetGameWorld().EnemyID / (float)GameApp.GetInstance().GetGameWorld().TotalEnemyCount);
		}
	}

	private void UpdateBossHp()
	{
		Enemy boss = GameApp.GetInstance().GetGameWorld().GetBoss(0);
		UserStateUI.GetInstance().UpdateBossHpPercent((float)boss.HP / (float)boss.MaxHP);
	}

	private void UpdateDoubleBossHp()
	{
		Enemy boss = GameApp.GetInstance().GetGameWorld().GetBoss(0);
		UserStateUI.GetInstance().UpdateBossHpPercent((float)boss.HP / (float)boss.MaxHP);
		Enemy boss2 = GameApp.GetInstance().GetGameWorld().GetBoss(1);
		UserStateUI.GetInstance().Update2ndBossHpPercent((float)boss2.HP / (float)boss2.MaxHP);
	}
}
