public class TowerDefenceBattleHUD : BattleHUD
{
	public TowerDefenceBattleHUD(UIStateManager stateMgr)
	{
		base.stateMgr = stateMgr;
	}

	public override bool Create()
	{
		base.Create();
		stateMgr.m_UIManager.Add(menuBtn);
		if (UIConstant.Is16By9())
		{
			stateMgr.m_UIManager.Add(joystickbgimg);
			stateMgr.m_UIManager.Add(shootjoystickbgimg);
		}
		else
		{
			stateMgr.m_UIManager.Add(battleImg);
		}
		stateMgr.m_UIManager.Add(joystickImg);
		stateMgr.m_UIManager.Add(shootjoystickImg);
		stateMgr.m_UIManager.Add(hpBGImg);
		stateMgr.m_UIManager.Add(hpEFXImg);
		stateMgr.m_UIManager.Add(hpImg);
		stateMgr.m_UIManager.Add(swapWeapon);
		stateMgr.m_UIManager.Add(useProps);
		stateMgr.m_UIManager.Add(aimImg);
		stateMgr.m_UIManager.Add(aimOnFireImg);
		return true;
	}

	protected override void UpdateAllHUD()
	{
		base.UpdateAllHUD();
	}
}
