using UnityEngine;

internal class RequestFlagPositionResponse : Response
{
	public override void ReadData(byte[] data)
	{
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.FLAG_RESPAWN);
			int num = Random.Range(0, array.Length);
			DropTheFlagRequest request = new DropTheFlagRequest(-1, array[num].transform.position + Vector3.up * 0f, true);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}
}
