public class CatchTheFlagRequest : Request
{
	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2);
		bytesBuffer.AddByte(135);
		bytesBuffer.AddByte(0);
		return bytesBuffer.GetBytes();
	}
}
