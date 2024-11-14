public class StartGameRequest : Request
{
	public override byte[] GetBytes()
	{
		byte b = 0;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(9);
		bytesBuffer.AddByte(b);
		return bytesBuffer.GetBytes();
	}
}
