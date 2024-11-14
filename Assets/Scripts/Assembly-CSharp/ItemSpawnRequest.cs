using UnityEngine;

public class ItemSpawnRequest : Request
{
	protected byte itemID;

	protected short x;

	protected short y;

	protected short z;

	protected int amount;

	public ItemSpawnRequest(byte itemID, Vector3 pos, int amount)
	{
		this.itemID = itemID;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		this.amount = amount;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(13);
		bytesBuffer.AddByte(119);
		bytesBuffer.AddByte(11);
		bytesBuffer.AddByte(itemID);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddInt(amount);
		return bytesBuffer.GetBytes();
	}
}
