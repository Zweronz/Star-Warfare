public class WeaponUpgrade
{
	protected int[] damage;

	protected int[] price;

	public void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[18];
		damage = new int[unitDataTable.sRows];
		price = new int[unitDataTable.sRows];
		for (int i = 0; i < unitDataTable.sRows; i++)
		{
			price[i] = unitDataTable.GetData(i, 0, 0, false);
			damage[i] = unitDataTable.GetData(i, 1, 0, false);
		}
	}

	public int GetDamage(int level)
	{
		return damage[level];
	}

	public int GetPrice(int level)
	{
		return price[level];
	}
}
