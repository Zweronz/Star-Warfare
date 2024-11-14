using UnityEngine;

internal class ReflectionEnemyResponse : Response
{
	protected int reflectionID;

	protected int enemyID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		reflectionID = bytesBuffer.ReadInt();
		enemyID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Enemy enemyByID = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + enemyID);
		Enemy enemyByID2 = GameApp.GetInstance().GetGameWorld().GetEnemyByID("E_" + reflectionID);
		if (enemyByID2 != null && enemyByID != null)
		{
			Ray ray = default(Ray);
			Vector3 normalized = (enemyByID2.GetPosition() - enemyByID.GetPosition()).normalized;
			ray = new Ray(enemyByID.GetPosition() + normalized * 1.8f, normalized);
			GameObject original = Resources.Load("Effect/SniperFireLine") as GameObject;
			GameObject gameObject = Object.Instantiate(original, enemyByID2.GetPosition(), Quaternion.LookRotation(enemyByID2.GetPosition())) as GameObject;
			SniperFirelineScript component = gameObject.GetComponent<SniperFirelineScript>();
			component.beginPos = enemyByID.GetPosition() + Vector3.up * 1f;
			component.endPos = enemyByID2.GetPosition() + Vector3.up * 1f;
			GameObject original2 = Resources.Load("Effect/LaserHit") as GameObject;
			GameObject gameObject2 = Object.Instantiate(original2, enemyByID2.GetPosition(), Quaternion.identity) as GameObject;
		}
	}
}
