public class TimeSynchronizeRequest : Request
{
	private byte mId;

	private long mTime;

	public TimeSynchronizeRequest(byte id, long mTime)
	{
		mId = id;
		this.mTime = mTime;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(11);
		bytesBuffer.AddByte(100);
		bytesBuffer.AddByte(9);
		bytesBuffer.AddByte(mId);
		bytesBuffer.AddLong(mTime);
		return bytesBuffer.GetBytes();
	}
}
