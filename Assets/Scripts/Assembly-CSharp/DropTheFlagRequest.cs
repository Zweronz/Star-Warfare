using UnityEngine;

public class DropTheFlagRequest : Request
{
	protected int playerID;

	protected short x;

	protected short y;

	protected short z;

	protected bool bInit;

	public DropTheFlagRequest(int playerID, Vector3 pos, bool bGetScore)
	{
		this.playerID = playerID;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		bInit = bGetScore;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(13);
		bytesBuffer.AddByte(134);
		bytesBuffer.AddByte(11);
		bytesBuffer.AddInt(playerID);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddBool(bInit);
		return bytesBuffer.GetBytes();
	}
}
