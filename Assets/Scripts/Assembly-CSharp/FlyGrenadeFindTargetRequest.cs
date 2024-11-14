using UnityEngine;

public class FlyGrenadeFindTargetRequest : Request
{
	protected int ownerId;

	protected byte id;

	protected int targetId;

	protected short x;

	protected short y;

	protected short z;

	public FlyGrenadeFindTargetRequest(int ownerId, byte id, int targetId, Vector3 pos)
	{
		this.ownerId = ownerId;
		this.id = id;
		this.targetId = targetId;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(17);
		bytesBuffer.AddByte(158);
		bytesBuffer.AddByte(15);
		bytesBuffer.AddInt(ownerId);
		bytesBuffer.AddByte(id);
		bytesBuffer.AddInt(targetId);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		return bytesBuffer.GetBytes();
	}
}
