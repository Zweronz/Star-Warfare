using System.IO;

public class OperRec122 : IRecordset
{
	private OperatingInfo oper;

	public OperRec122(OperatingInfo oper)
	{
		this.oper = oper;
	}

	public void SaveData(BinaryWriter bw)
	{
		bw.Write(oper.MithrilRebirthTime);
		bw.Write(oper.payDollars);
		bw.Write(oper.UDID);
	}

	public void LoadData(BinaryReader br)
	{
		oper.MithrilRebirthTime = br.ReadInt32();
		oper.payDollars = br.ReadInt32();
		oper.UDID = br.ReadString();
	}
}
