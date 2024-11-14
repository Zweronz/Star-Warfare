using System.IO;

public class BattleRec120 : IRecordset
{
	private UserState state;

	public BattleRec120(UserState state)
	{
		this.state = state;
	}

	public void SaveData(BinaryWriter bw)
	{
		for (int i = 0; i < 3; i++)
		{
			state.GetBattleStates()[i].Save(bw);
		}
	}

	public void LoadData(BinaryReader br)
	{
		for (int i = 0; i < 3; i++)
		{
			state.GetBattleStates()[i].Load(br);
		}
	}
}
