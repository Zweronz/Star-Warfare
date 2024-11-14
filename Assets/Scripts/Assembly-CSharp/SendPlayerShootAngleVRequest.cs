public class SendPlayerShootAngleVRequest : Request
{
	protected short m_angleV;

	public SendPlayerShootAngleVRequest(short angleV)
	{
		m_angleV = angleV;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(124);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddShort(m_angleV);
		return bytesBuffer.GetBytes();
	}
}
