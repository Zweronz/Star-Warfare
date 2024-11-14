public class VSGameEndRequest : Request
{
	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(128);
		bytesBuffer.AddByte(0);
		return bytesBuffer.GetBytes();
	}
}
