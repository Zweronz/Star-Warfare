public class PlayerBuffRequest : Request
{
	protected byte buffID;

	public PlayerBuffRequest(byte buffID)
	{
		this.buffID = buffID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(3);
		bytesBuffer.AddByte(123);
		bytesBuffer.AddByte(1);
		bytesBuffer.AddByte(buffID);
		return bytesBuffer.GetBytes();
	}
}
