public class RoomTimeSynchronizeRequest : Request
{
	private byte mId;

	public RoomTimeSynchronizeRequest(byte id)
	{
		mId = id;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(13);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(mId);
		return bytesBuffer.GetBytes();
	}
}
