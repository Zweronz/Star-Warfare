using UnityEngine;

public class PlayerTrackingGrendeRequest : Request
{
	protected short x;

	protected short y;

	protected short z;

	protected short dx;

	protected short dy;

	protected short dz;

	protected int userID;

	protected byte grenadeID;

	public PlayerTrackingGrendeRequest(int userID, byte grenadeID, Vector3 pos, Vector3 dir)
	{
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		dx = (short)(dir.x * 10f);
		dy = (short)(dir.y * 10f);
		dz = (short)(dir.z * 10f);
		this.userID = userID;
		this.grenadeID = grenadeID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(19);
		bytesBuffer.AddByte(146);
		bytesBuffer.AddByte(17);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(dx);
		bytesBuffer.AddShort(dy);
		bytesBuffer.AddShort(dz);
		bytesBuffer.AddInt(userID);
		bytesBuffer.AddByte(grenadeID);
		return bytesBuffer.GetBytes();
	}
}
