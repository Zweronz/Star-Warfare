public class SetRoomPingRequest : Request
{
	private short mRoomId;

	private short mPing;

	public SetRoomPingRequest(short roomId, short ping)
	{
		mRoomId = roomId;
		mPing = ping;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(6);
		bytesBuffer.AddByte(14);
		bytesBuffer.AddByte(4);
		bytesBuffer.AddShort(mRoomId);
		bytesBuffer.AddShort(mPing);
		return bytesBuffer.GetBytes();
	}
}
