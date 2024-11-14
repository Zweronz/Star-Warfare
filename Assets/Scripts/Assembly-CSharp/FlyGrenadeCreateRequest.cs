using UnityEngine;

public class FlyGrenadeCreateRequest : Request
{
	protected int ownerId;

	protected byte id;

	protected short x;

	protected short y;

	protected short z;

	protected short dx;

	protected short dy;

	protected short dz;

	public FlyGrenadeCreateRequest(int ownerId, byte id, Vector3 pos, Vector3 dir)
	{
		this.ownerId = ownerId;
		this.id = id;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(19);
		bytesBuffer.AddByte(157);
		bytesBuffer.AddByte(17);
		bytesBuffer.AddInt(ownerId);
		bytesBuffer.AddByte(id);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(dx);
		bytesBuffer.AddShort(dy);
		bytesBuffer.AddShort(dz);
		return bytesBuffer.GetBytes();
	}
}
