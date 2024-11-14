public class OperatingInfo : IOperating
{
	public int MithrilRebirthTime;

	public int payDollars;

	public string UDID = string.Empty;

	public void WriteToBuffer(BytesBuffer buffer)
	{
		UDID = GameApp.GetInstance().UUID;
		buffer.AddInt(MithrilRebirthTime);
		buffer.AddInt(payDollars);
		buffer.AddString(UDID);
	}
}
