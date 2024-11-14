using System.Collections.Generic;
using UnityEngine;

public class SatanMachineGiftBombRequest : Request
{
	protected short enemyID;

	protected byte enemyState;

	protected short x;

	protected short y;

	protected short z;

	protected List<short> forceXList = new List<short>();

	protected List<short> forceYList = new List<short>();

	protected List<short> forceZList = new List<short>();

	protected List<short> typeList = new List<short>();

	public SatanMachineGiftBombRequest(short enemyID, byte enemyState, Vector3 pos, List<Vector3> forceList, List<short> typeList)
	{
		this.enemyID = enemyID;
		this.enemyState = enemyState;
		x = (short)(pos.x * 10f);
		y = (short)(pos.y * 10f);
		z = (short)(pos.z * 10f);
		foreach (Vector3 force in forceList)
		{
			forceXList.Add((short)(force.x * 10f));
			forceYList.Add((short)(force.x * 10f));
			forceZList.Add((short)(force.x * 10f));
		}
		this.typeList = typeList;
	}

	public override byte[] GetBytes()
	{
		int count = forceXList.Count;
		BytesBuffer bytesBuffer = new BytesBuffer(13 + count * 8);
		bytesBuffer.AddByte(147);
		bytesBuffer.AddByte((byte)(11 + count * 8));
		bytesBuffer.AddShort(enemyID);
		bytesBuffer.AddByte(enemyState);
		bytesBuffer.AddShort(x);
		bytesBuffer.AddShort(y);
		bytesBuffer.AddShort(z);
		bytesBuffer.AddShort((short)count);
		for (int i = 0; i < forceXList.Count; i++)
		{
			bytesBuffer.AddShort(forceXList[i]);
			bytesBuffer.AddShort(forceYList[i]);
			bytesBuffer.AddShort(forceZList[i]);
			bytesBuffer.AddShort(typeList[i]);
		}
		return bytesBuffer.GetBytes();
	}
}
