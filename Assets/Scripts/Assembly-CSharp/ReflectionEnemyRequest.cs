using UnityEngine;

public class ReflectionEnemyRequest : Request
{
	protected int reflectionID;

	protected int enemyID;

	public ReflectionEnemyRequest(int enemyID, int reflectionID)
	{
		Debug.Log("enemyID = " + enemyID);
		Debug.Log("reflectionID = " + reflectionID);
		this.reflectionID = reflectionID;
		this.enemyID = enemyID;
	}

	public override byte[] GetBytes()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(10);
		bytesBuffer.AddByte(155);
		bytesBuffer.AddByte(8);
		bytesBuffer.AddInt(enemyID);
		bytesBuffer.AddInt(reflectionID);
		return bytesBuffer.GetBytes();
	}
}
