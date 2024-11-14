public class UploadOperatingInfo : Request
{
	private int mithrilRebirthTime;

	private int expendDollar;

	private string udid;

	public UploadOperatingInfo(int rebirthTime, int expendDols, string udid)
	{
		mithrilRebirthTime = rebirthTime;
		expendDollar = expendDols;
		this.udid = udid;
	}

	public override byte[] GetBytes()
	{
		byte b = (byte)(8 + udid.Length + 1);
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(23);
		bytesBuffer.AddByte(b);
		bytesBuffer.AddInt(mithrilRebirthTime);
		bytesBuffer.AddInt(expendDollar);
		bytesBuffer.AddString(udid);
		return bytesBuffer.GetBytes();
	}
}
