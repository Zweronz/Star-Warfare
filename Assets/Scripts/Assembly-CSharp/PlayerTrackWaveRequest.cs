using UnityEngine;

public class PlayerTrackWaveRequest : Request
{
	protected byte type;

	protected short x;

	protected short y;

	protected short z;

	protected short dx;

	protected short dy;

	protected short dz;

	protected int trackingID;

	public PlayerTrackWaveRequest(byte type, Vector3 pos, Vector3 dir)
	{
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		trackingID = -1;
	}

	public PlayerTrackWaveRequest(byte type, Vector3 pos, Vector3 dir, int trackingID)
	{
		this.type = type;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		this.trackingID = trackingID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(19);
		bytesBuffer.AddByte(154);
		bytesBuffer.AddByte(17);
		bytesBuffer.AddByte(type);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(dx);
		bytesBuffer.AddShort(dy);
		bytesBuffer.AddShort(dz);
		bytesBuffer.AddInt(trackingID);
		return bytesBuffer.GetBytes();
	}
}
