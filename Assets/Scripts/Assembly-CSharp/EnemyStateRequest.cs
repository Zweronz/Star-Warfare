using UnityEngine;

public class EnemyStateRequest : Request
{
	protected short enemyID;

	protected byte state;

	protected short x;

	protected short y;

	protected short z;

	protected short sx;

	protected short sy;

	protected short sz;

	protected int targetID;

	protected byte targetPointID;

	protected int stateIndex;

	public EnemyStateRequest(short enemyID, byte state, Vector3 pos, Vector3 speed)
	{
		this.enemyID = enemyID;
		this.state = state;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		sx = (short)(speed.x * 10f);
		sy = (short)(speed.y * 10f);
		sz = (short)(speed.z * 10f);
	}

	public EnemyStateRequest(short enemyID, byte state, Vector3 pos, int targetID)
	{
		this.enemyID = enemyID;
		this.state = state;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		this.targetID = targetID;
	}

	public EnemyStateRequest(short enemyID, byte state, Vector3 pos, byte targetPointID)
	{
		this.enemyID = enemyID;
		this.state = state;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		this.targetPointID = targetPointID;
	}

	public EnemyStateRequest(short enemyID, byte state, int stateIndex, int targetID)
	{
		this.enemyID = enemyID;
		this.state = state;
		this.stateIndex = stateIndex;
		this.targetID = targetID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = null;
		switch (state)
		{
		case 1:
		case 2:
			bytesBuffer = new BytesBuffer(11);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(9);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddShort(x);
			bytesBuffer.AddShort(y);
			bytesBuffer.AddShort(z);
			break;
		case 3:
		case 5:
			bytesBuffer = new BytesBuffer(17);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(15);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddShort(x);
			bytesBuffer.AddShort(y);
			bytesBuffer.AddShort(z);
			bytesBuffer.AddShort(sx);
			bytesBuffer.AddShort(sy);
			bytesBuffer.AddShort(sz);
			break;
		case 10:
		case 11:
		case 12:
		case 13:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 61:
		case 62:
		case 63:
		case 80:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
			bytesBuffer = new BytesBuffer(15);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(13);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddShort(x);
			bytesBuffer.AddShort(y);
			bytesBuffer.AddShort(z);
			bytesBuffer.AddInt(targetID);
			break;
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
			bytesBuffer = new BytesBuffer(12);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(10);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddShort(x);
			bytesBuffer.AddShort(y);
			bytesBuffer.AddShort(z);
			bytesBuffer.AddByte(targetPointID);
			break;
		case 93:
			bytesBuffer = new BytesBuffer(13);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(11);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddInt(stateIndex);
			bytesBuffer.AddInt(targetID);
			break;
		default:
			bytesBuffer = new BytesBuffer(11);
			bytesBuffer.AddByte(106);
			bytesBuffer.AddByte(9);
			bytesBuffer.AddByte(state);
			bytesBuffer.AddShort(enemyID);
			bytesBuffer.AddShort(x);
			bytesBuffer.AddShort(y);
			bytesBuffer.AddShort(z);
			break;
		}
		return bytesBuffer.GetBytes();
	}
}
