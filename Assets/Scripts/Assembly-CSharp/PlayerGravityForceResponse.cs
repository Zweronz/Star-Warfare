using UnityEngine;

internal class PlayerGravityForceResponse : Response
{
	public int targetId;

	public Vector3 pos;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		targetId = bytesBuffer.ReadInt();
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		float x = (float)num / 10f;
		float y = (float)num2 / 10f;
		float z = (float)num3 / 10f;
		pos = new Vector3(x, y, z);
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		if (targetId == Lobby.GetInstance().GetChannelID())
		{
			gameWorld.GetPlayer().OnGravityForce(pos);
			return;
		}
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(targetId);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.OnGravityForce(pos);
		}
	}
}
