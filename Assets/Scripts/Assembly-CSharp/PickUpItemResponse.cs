internal class PickUpItemResponse : Response
{
	protected short m_sequenceID;

	protected byte m_pickup;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_sequenceID = bytesBuffer.ReadShort();
		m_pickup = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		if (m_pickup == 1)
		{
			GameApp.GetInstance().GetGameWorld().GetPlayer()
				.OnPickUp(m_sequenceID);
			GameApp.GetInstance().GetGameWorld().DestroyLoot(m_sequenceID);
		}
		else if (m_pickup == 0)
		{
			GameApp.GetInstance().GetGameWorld().DestroyLoot(m_sequenceID);
		}
	}
}
