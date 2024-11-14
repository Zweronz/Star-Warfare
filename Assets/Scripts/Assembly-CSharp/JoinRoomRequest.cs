public class JoinRoomRequest : Request
{
	public short roomID;

	public byte rankID;

	public short ping;

	public JoinRoomRequest(short id, byte rankID, short ping)
	{
		roomID = id;
		this.rankID = rankID;
		this.ping = ping;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(7);
		bytesBuffer.AddByte(5);
		bytesBuffer.AddByte(5);
		bytesBuffer.AddShort(roomID);
		bytesBuffer.AddByte(rankID);
		bytesBuffer.AddShort(ping);
		return bytesBuffer.GetBytes();
	}
}
