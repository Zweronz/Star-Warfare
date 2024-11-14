using UnityEngine;

internal class SendTransformStateResponse : Response
{
	public int channelID;

	public Vector3 pos;

	public float eY;

	public int timeStamp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		short num4 = bytesBuffer.ReadShort();
		timeStamp = bytesBuffer.ReadInt();
		float x = (float)num / 10f;
		float y = (float)num2 / 10f;
		float z = (float)num3 / 10f;
		eY = (float)num4 / 10f;
		pos = new Vector3(x, y, z);
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(channelID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.UpdateTransform(pos, eY, timeStamp, remotePlayerByUserID.GetTransform());
			}
			else
			{
				Debug.Log("player is null");
			}
		}
	}
}
