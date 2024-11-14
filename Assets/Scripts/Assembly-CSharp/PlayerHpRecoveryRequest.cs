public class PlayerHpRecoveryRequest : Request
{
	protected byte type;

	protected short point;

	public PlayerHpRecoveryRequest(byte type, short point)
	{
		this.type = type;
		this.point = point;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(5);
		bytesBuffer.AddByte(118);
		bytesBuffer.AddByte(3);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(point);
		return bytesBuffer.GetBytes();
	}
}
