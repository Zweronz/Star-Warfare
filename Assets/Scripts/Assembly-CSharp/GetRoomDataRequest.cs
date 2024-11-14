public class GetRoomDataRequest : Request
{
	protected short roomID;

	public GetRoomDataRequest(short roomID)
	{
		this.roomID = roomID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(8);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddShort(roomID);
		return bytesBuffer.GetBytes();
	}
}
