public class WeaponUpgradeVerify
{
	protected int[] damage;

	public WeaponUpgradeVerify()
	{
		LoadConfig();
	}

	public void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[18];
		damage = new int[unitDataTable.sRows];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			damage[i] = unitDataTable.GetData(i, 1, 0, false);
		}
	}

	public int GetDamage(int level)
	{
		return damage[level];
	}
}
