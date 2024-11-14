using UnityEngine;

internal class SendPlayerInputResponse : Response
{
	public int playerID;

	public bool fire;

	public bool isMoving;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		fire = bytesBuffer.ReadBool();
		isMoving = bytesBuffer.ReadBool();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.inputController.inputInfo.fire = fire;
			if (isMoving)
			{
				remotePlayerByUserID.inputController.inputInfo.moveDirection = new Vector3(1f, 0f, 1f);
			}
			else
			{
				remotePlayerByUserID.inputController.inputInfo.moveDirection = Vector3.zero;
			}
		}
	}
}
