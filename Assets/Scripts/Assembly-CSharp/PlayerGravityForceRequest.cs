using UnityEngine;

public class PlayerGravityForceRequest : Request
{
	protected int targetId;

	protected short x;

	protected short y;

	protected short z;

	public PlayerGravityForceRequest(int targetId, Vector3 pos)
	{
		this.targetId = targetId;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(12);
		bytesBuffer.AddByte(160);
		bytesBuffer.AddByte(10);
		bytesBuffer.AddInt(targetId);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		return bytesBuffer.GetBytes();
	}
}
