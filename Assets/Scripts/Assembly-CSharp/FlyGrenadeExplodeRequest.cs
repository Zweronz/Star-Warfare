using UnityEngine;

public class FlyGrenadeExplodeRequest : Request
{
	protected int ownerId;

	protected byte id;

	protected short x;

	protected short y;

	protected short z;

	public FlyGrenadeExplodeRequest(int ownerId, byte id, Vector3 pos)
	{
		this.ownerId = ownerId;
		this.id = id;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(13);
		bytesBuffer.AddByte(159);
		bytesBuffer.AddByte(11);
		bytesBuffer.AddInt(ownerId);
		bytesBuffer.AddByte(id);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		return bytesBuffer.GetBytes();
	}
}
