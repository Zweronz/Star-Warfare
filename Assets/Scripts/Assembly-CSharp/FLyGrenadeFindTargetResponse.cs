using UnityEngine;

internal class FLyGrenadeFindTargetResponse : Response
{
	public int ownerId;

	public byte id;

	public int targetId;

	public Vector3 pos;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		ownerId = bytesBuffer.ReadInt();
		id = bytesBuffer.ReadByte();
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
		if (gameWorld != null)
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(ownerId);
			if (remotePlayerByUserID != null && remotePlayerByUserID.FlyGrenadeDic != null && remotePlayerByUserID.FlyGrenadeDic.ContainsKey(id) && remotePlayerByUserID.FlyGrenadeDic[id] != null)
			{
				remotePlayerByUserID.FlyGrenadeDic[id].OnChangeTarget(targetId, pos);
			}
		}
	}
}
