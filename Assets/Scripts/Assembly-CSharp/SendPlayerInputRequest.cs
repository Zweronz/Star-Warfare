public class SendPlayerInputRequest : Request
{
	protected bool m_Fire;

	protected bool m_IsMoving;

	public SendPlayerInputRequest(bool fire, bool isMoving)
	{
		m_Fire = fire;
		m_IsMoving = isMoving;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(4);
		bytesBuffer.AddByte(103);
		bytesBuffer.AddByte(2);
		bytesBuffer.AddBool(m_Fire);
		bytesBuffer.AddBool(m_IsMoving);
		return bytesBuffer.GetBytes();
	}
}
