public class GetRoomListRequest : Request
{
	public byte roomType;

	public byte rankID;

	public GetRoomListRequest(byte roomType, byte rankID)
	{
		this.roomType = roomType;
		this.rankID = rankID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(7);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddByte(roomType);
		bytesBuffer.AddByte(rankID);
		return bytesBuffer.GetBytes();
	}
}
