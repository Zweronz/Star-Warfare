using UnityEngine;

public class EnemyShotRequest : Request
{
	protected byte enemyType;

	protected short x;

	protected short y;

	protected short z;

	protected short sx;

	protected short sy;

	protected short sz;

	public EnemyShotRequest(byte enemyType, Vector3 pos, Vector3 speed)
	{
		this.enemyType = enemyType;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		sx = (short)(speed.x * 10f);
		sy = (short)(speed.y * 10f);
		sz = (short)(speed.z * 10f);
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(15);
		bytesBuffer.AddByte(116);
		bytesBuffer.AddByte(13);
		bytesBuffer.AddByte(enemyType);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(sx);
		bytesBuffer.AddShort(sy);
		bytesBuffer.AddShort(sz);
		return bytesBuffer.GetBytes();
	}
}
