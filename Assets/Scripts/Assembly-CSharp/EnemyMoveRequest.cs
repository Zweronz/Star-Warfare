using UnityEngine;

public class EnemyMoveRequest : Request
{
	protected short enemyID;

	protected short x;

	protected short y;

	protected short z;

	protected short tx;

	protected short ty;

	protected short tz;

	protected int targetID;

	protected bool fly;

	public EnemyMoveRequest(Vector3 pos, Vector3 targetPos, short enemyID, int targetID, bool fly)
	{
		this.enemyID = enemyID;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		tx = (short)(targetPos.x * 10f);
		ty = (short)(targetPos.y * 10f);
		tz = (short)(targetPos.z * 10f);
		this.targetID = targetID;
		this.fly = fly;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(21);
		bytesBuffer.AddByte(105);
		bytesBuffer.AddByte(19);
		bytesBuffer.AddShort(enemyID);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(tx);
		bytesBuffer.AddShort(ty);
		bytesBuffer.AddShort(tz);
		bytesBuffer.AddInt(targetID);
		bytesBuffer.AddBool(fly);
		return bytesBuffer.GetBytes();
	}
}
