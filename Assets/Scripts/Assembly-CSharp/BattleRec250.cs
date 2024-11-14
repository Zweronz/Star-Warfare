using System.IO;

public class BattleRec250 : IRecordset
{
	private UserState state;

	public BattleRec250(UserState state)
	{
		this.state = state;
	}

	public void SaveData(BinaryWriter bw)
	{
		for (int i = 0; i < 4; i++)
		{
			state.GetBattleStates()[i].Save(bw);
		}
	}

	public void LoadData(BinaryReader br)
	{
		for (int i = 0; i < 4; i++)
		{
			state.GetBattleStates()[i].Load(br);
		}
	}
}
