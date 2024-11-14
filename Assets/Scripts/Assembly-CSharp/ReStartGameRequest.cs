public class ReStartGameRequest : Request
{
	public override byte[] GetBytes()
	{
		byte b = 0;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(131);
		bytesBuffer.AddByte(b);
		return bytesBuffer.GetBytes();
	}
}
