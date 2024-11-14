using UnityEngine;

public class PlayerTrackingGrendeRespone : Response
{
	protected Vector3 pos;

	protected Vector3 dir;

	protected int userID;

	protected byte grenadeID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		short num4 = bytesBuffer.ReadShort();
		short num5 = bytesBuffer.ReadShort();
		short num6 = bytesBuffer.ReadShort();
		userID = bytesBuffer.ReadInt();
		grenadeID = bytesBuffer.ReadByte();
		float x = (float)num / 10f;
		float y = (float)num2 / 10f;
		float z = (float)num3 / 10f;
		pos = new Vector3(x, y, z);
		float x2 = (float)num4 / 10f;
		float y2 = (float)num5 / 10f;
		float z2 = (float)num6 / 10f;
		dir = new Vector3(x2, y2, z2);
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(userID);
		if (remotePlayerByUserID != null)
		{
			if (remotePlayerByUserID.TrackingGrenadeDic.ContainsKey(grenadeID))
			{
				TrackingGrenadeScript trackingGrenadeScript = remotePlayerByUserID.TrackingGrenadeDic[grenadeID];
				trackingGrenadeScript.isTracking = true;
				trackingGrenadeScript.targetPos = pos;
				trackingGrenadeScript.explodeTimer.Do();
				return;
			}
			GameObject original = Resources.Load("Effect/TrackingRobot") as GameObject;
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.transform.position = pos + Vector3.up * 0.05f;
			gameObject.transform.LookAt(gameObject.transform.position + dir);
			Timer timer = new Timer();
			timer.SetTimer(2f, false);
			TrackingGrenadeScript componentInChildren = gameObject.GetComponentInChildren<TrackingGrenadeScript>();
			componentInChildren.damage = 20;
			componentInChildren.explodeTimer = timer;
			componentInChildren.explodeRadius = 5f;
			componentInChildren.trackRadius = 8f;
			componentInChildren.trackSpeed = 10f;
			componentInChildren.trackID = grenadeID;
			componentInChildren.userID = userID;
			componentInChildren.isLocal = false;
			remotePlayerByUserID.TrackingGrenadeDic.Add(grenadeID, componentInChildren);
		}
	}
}
