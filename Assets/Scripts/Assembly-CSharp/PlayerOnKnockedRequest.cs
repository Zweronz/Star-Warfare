public class PlayerOnKnockedRequest : Request
{
	protected short speed;

	public PlayerOnKnockedRequest(float speed)
	{
		this.speed = (short)(speed * 100f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(121);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddShort(speed);
		return bytesBuffer.GetBytes();
	}
}
