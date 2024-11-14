using UnityEngine;

internal class FLyGrenadeCreateResponse : Response
{
	public int ownerId;

	public byte id;

	public Vector3 pos;

	public Vector3 dir;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		ownerId = bytesBuffer.ReadInt();
		id = bytesBuffer.ReadByte();
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		short num4 = bytesBuffer.ReadShort();
		short num5 = bytesBuffer.ReadShort();
		short num6 = bytesBuffer.ReadShort();
		float x = (float)num / 10f;
		float y = (float)num2 / 10f;
		float z = (float)num3 / 10f;
		pos = new Vector3(x, y, z);
		x = (float)num4 / 10f;
		y = (float)num5 / 10f;
		z = (float)num6 / 10f;
		dir = new Vector3(x, y, z);
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(ownerId);
		if (remotePlayerByUserID != null)
		{
			GameObject original = Resources.Load("SW2_Effect/HotWing_Bullet") as GameObject;
			GameObject gameObject = Object.Instantiate(original, pos, Quaternion.identity) as GameObject;
			gameObject.layer = PhysicsLayer.PROJECTILE;
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = 0.2f;
			sphereCollider.isTrigger = true;
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			FlyGrenadeShotScript flyGrenadeShotScript = gameObject.AddComponent<FlyGrenadeShotScript>();
			flyGrenadeShotScript.ownerId = ownerId;
			flyGrenadeShotScript.grenadeId = id;
			flyGrenadeShotScript.dir = dir;
			flyGrenadeShotScript.life = 5f;
			flyGrenadeShotScript.flySpeed = 10f;
			flyGrenadeShotScript.isLocal = false;
			if (remotePlayerByUserID.FlyGrenadeDic != null && !remotePlayerByUserID.FlyGrenadeDic.ContainsKey(id))
			{
				remotePlayerByUserID.FlyGrenadeDic.Add(id, flyGrenadeShotScript);
			}
		}
	}
}
