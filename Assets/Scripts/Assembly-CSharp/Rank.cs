public class Rank
{
	public string name;

	public int exp;

	public byte rankID;

	public Rank(int id)
	{
		rankID = (byte)id;
	}

	public void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[17];
		if (unitDataTable != null)
		{
			name = unitDataTable.GetData(rankID, 0, string.Empty, false);
			exp = unitDataTable.GetData(rankID, 1, 0, false);
		}
	}
}
