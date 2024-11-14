public class ChangeSeatRequest : Request
{
	public byte seatID;

	public ChangeSeatRequest(byte seatId)
	{
		seatID = seatId;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(20);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(seatID);
		return bytesBuffer.GetBytes();
	}
}
