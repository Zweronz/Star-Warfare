public class TestRequest : Request
{
	public override byte[] GetBytes()
	{
		byte b = 20;
		BytesBuffer bytesBuffer = new BytesBuffer(2 + b);
		bytesBuffer.AddByte(50);
		bytesBuffer.AddByte(b);
		for (int i = 0; i < 5; i++)
		{
			bytesBuffer.AddInt(1);
		}
		return bytesBuffer.GetBytes();
	}
}
