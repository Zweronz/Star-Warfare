public class LeaveRoomRequest : Request
{
	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(6);
		bytesBuffer.AddByte(0);
		return bytesBuffer.GetBytes();
	}
}
