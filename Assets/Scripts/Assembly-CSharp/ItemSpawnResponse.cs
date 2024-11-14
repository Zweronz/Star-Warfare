using UnityEngine;

internal class ItemSpawnResponse : Response
{
	protected short m_sequenceID;

	protected short m_x;

	protected short m_y;

	protected short m_z;

	protected byte m_itemID;

	protected int m_amount;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_sequenceID = bytesBuffer.ReadShort();
		m_x = bytesBuffer.ReadShort();
		m_y = bytesBuffer.ReadShort();
		m_z = bytesBuffer.ReadShort();
		m_itemID = bytesBuffer.ReadByte();
		m_amount = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Vector3 pos = new Vector3((float)m_x / 10f, (float)m_y / 10f, (float)m_z / 10f);
		GameApp.GetInstance().GetGameWorld().SpawnItem((LootType)m_itemID, pos, m_amount, m_sequenceID);
	}
}
