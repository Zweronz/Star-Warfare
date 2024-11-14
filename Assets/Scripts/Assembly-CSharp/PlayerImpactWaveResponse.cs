using UnityEngine;

internal class PlayerImpactWaveResponse : Response
{
	public byte type;

	public Vector3 pos;

	public Vector3 dir;

	protected int trackingID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		type = bytesBuffer.ReadByte();
		short num = bytesBuffer.ReadShort();
		short num2 = bytesBuffer.ReadShort();
		short num3 = bytesBuffer.ReadShort();
		short num4 = bytesBuffer.ReadShort();
		short num5 = bytesBuffer.ReadShort();
		short num6 = bytesBuffer.ReadShort();
		trackingID = bytesBuffer.ReadInt();
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
		if (gameWorld != null)
		{
			string path = "Effect/update_effect/effect_wave_002";
			GameObject original = Resources.Load(path) as GameObject;
			GameObject gameObject = Object.Instantiate(original, pos + dir, Quaternion.LookRotation(dir)) as GameObject;
			ImpactWaveScript component = gameObject.GetComponent<ImpactWaveScript>();
			component.dir = dir;
			component.flySpeed = 10f;
			component.explodeRadius = 3f;
			component.hitForce = 20f;
			component.life = 8f;
			component.damage = 10;
			component.gunType = (WeaponType)type;
			component.isLocal = false;
			component.isPenerating = true;
		}
	}
}
