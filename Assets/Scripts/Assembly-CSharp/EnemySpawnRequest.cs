using UnityEngine;

public class EnemySpawnRequest : Request
{
	protected byte eType;

	protected short x;

	protected short y;

	protected short z;

	protected short round;

	protected byte index;

	protected bool elite;

	protected bool fromGrave;

	public EnemySpawnRequest(EnemyType enemyType, Vector3 pos, short round, byte index, bool elite, bool fromGrave)
	{
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		eType = (byte)enemyType;
		this.round = round;
		this.index = index;
		this.elite = elite;
		this.fromGrave = fromGrave;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(14);
		bytesBuffer.AddByte(104);
		bytesBuffer.AddByte(12);
		bytesBuffer.AddByte(eType);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort(round);
		bytesBuffer.AddByte(index);
		bytesBuffer.AddBool(elite);
		bytesBuffer.AddBool(fromGrave);
		return bytesBuffer.GetBytes();
	}
}
