using System.Collections.Generic;
using UnityEngine;

public class SatanMachineGiftBombResponse : Response
{
	protected short enemyId;

	protected byte enemyState;

	protected short x;

	protected short y;

	protected short z;

	protected List<Vector3> forceList = new List<Vector3>();

	protected List<short> typeList = new List<short>();

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		enemyId = bytesBuffer.ReadShort();
		enemyState = bytesBuffer.ReadByte();
		x = bytesBuffer.ReadShort();
		y = bytesBuffer.ReadShort();
		z = bytesBuffer.ReadShort();
		int num = bytesBuffer.ReadShort();
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)bytesBuffer.ReadShort() / 10f;
			float num3 = (float)bytesBuffer.ReadShort() / 10f;
			float num4 = (float)bytesBuffer.ReadShort() / 10f;
			forceList.Add(new Vector3(num2, num3, num4));
			typeList.Add(bytesBuffer.ReadShort());
		}
	}

	public override void ProcessLogic()
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + enemyId);
		if (enemyByID != null && enemyByID is SatanMachine)
		{
			SatanMachine satanMachine = enemyByID as SatanMachine;
			if (enemyState == 89 || enemyState == 90)
			{
				satanMachine.ShotGiftBomb(new Vector3((float)x / 10f, (float)y / 10f, (float)z / 10f), forceList, typeList);
			}
			else if (enemyState == 94)
			{
				satanMachine.ThrowBall(new Vector3((float)x / 10f, (float)y / 10f, (float)z / 10f), forceList, typeList);
			}
		}
	}
}
