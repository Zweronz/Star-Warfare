using System.IO;

public class OperRec121 : IRecordset
{
	private OperatingInfo oper;

	public OperRec121(OperatingInfo oper)
	{
		this.oper = oper;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(oper.MithrilRebirthTime);
	}

	public void LoadData(BinaryReader br)
	{
		oper.MithrilRebirthTime = br.ReadInt32();
	}
}
