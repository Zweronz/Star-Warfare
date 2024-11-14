using UnityEngine;

public class PlayerFireRocketRequest : Request
{
	protected byte type;

	protected short x;

	protected short y;

	protected short z;

	protected short dx;

	protected short dy;

	protected short dz;

	protected int trackingID;

	protected int damage;

	public PlayerFireRocketRequest(byte type, Vector3 pos, Vector3 dir)
	{
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		trackingID = -1;
		damage = -1;
	}

	public PlayerFireRocketRequest(byte type, Vector3 pos, Vector3 dir, int damage, int trackingID)
	{
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		this.trackingID = trackingID;
		this.damage = damage;
	}

	public PlayerFireRocketRequest(byte type, Vector3 pos, Vector3 dir, int trackingID)
	{
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		this.trackingID = trackingID;
		damage = -1;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(23);
		bytesBuffer.AddByte(114);
		bytesBuffer.AddByte(21);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(dx);
		bytesBuffer.AddShort(dy);
		bytesBuffer.AddShort(dz);
		bytesBuffer.AddInt(trackingID);
		bytesBuffer.AddInt(damage);
		return bytesBuffer.GetBytes();
	}
}
