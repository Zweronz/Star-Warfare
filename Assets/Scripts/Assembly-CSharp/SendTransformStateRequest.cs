using UnityEngine;

public class SendTransformStateRequest : Request
{
	protected short x;

	protected short y;

	protected short z;

	protected short angleY;

	protected int timeStamp;

	public SendTransformStateRequest(Vector3 pos, Vector3 elurAngles)
	{
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		angleY = (short)(elurAngles.y * 10f);
		timeStamp = TimeManager.GetInstance().NetworkTime;
	}

	public SendTransformStateRequest(Vector3 pos, Vector3 elurAngles, int time)
	{
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		angleY = (short)(elurAngles.y * 10f);
		timeStamp = time;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(14);
		bytesBuffer.AddByte(102);
		bytesBuffer.AddByte(12);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(angleY);
		bytesBuffer.AddInt(timeStamp);
		return bytesBuffer.GetBytes();
	}
}
