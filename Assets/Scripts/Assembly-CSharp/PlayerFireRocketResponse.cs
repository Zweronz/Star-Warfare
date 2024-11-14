using UnityEngine;

internal class PlayerFireRocketResponse : Response
{
	public byte type;

	public Vector3 pos;

	public Vector3 dir;

	protected int trackingID;

	protected int damage;

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
		damage = bytesBuffer.ReadInt();
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
		string path = "Effect/Projectile";
		switch (type)
		{
		case 3:
			path = "Effect/Projectile";
			break;
		case 9:
			path = "Effect/LightBow_Shot";
			break;
		case 13:
			path = "Effect/Trinity/Bow_Shot";
			break;
		case 14:
			path = "Effect/BlackStar/DAN";
			break;
		case 10:
			path = "Effect/update_effect/effect_fist_attack_002";
			break;
		case 40:
			path = "Effect/update_effect/effect_arrow_t_purple";
			break;
		case 4:
		case 24:
			path = "Effect/GrenadeShot";
			break;
		case 16:
			path = "Effect/update_effect/effect_sword_flying_001";
			break;
		case 19:
			path = "Effect/update_effect/effect_arrow_t";
			break;
		case 20:
			path = "Effect/PingPongShot";
			break;
		case 21:
			path = "Effect/pumpkinProjectile";
			break;
		case 26:
			path = "Effect/SatanMachine/joke_force";
			break;
		case 25:
			path = "Effect/TrackingGrenadeShot";
			break;
		case 41:
			path = "Effect/SniperFireLine";
			break;
		case 42:
			path = "Effect/update_effect/effect_thearrow_attack";
			break;
		}
		GameObject original = Resources.Load(path) as GameObject;
		GameObject gameObject = Object.Instantiate(original, pos + dir, Quaternion.LookRotation(dir)) as GameObject;
		if (type == 16)
		{
			int num = -45;
			if (Random.Range(0, 100) < 50)
			{
				num = 45;
			}
			gameObject.transform.Rotate(new Vector3(0f, 0f, num), Space.Self);
		}
		if (type == 25)
		{
			GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
			TrackingGrenadeShotScript componentInChildren = gameObject.GetComponentInChildren<TrackingGrenadeShotScript>();
			component.dir = dir;
			component.life = 8f;
			componentInChildren.dir = dir;
			componentInChildren.flySpeed = 16f;
			componentInChildren.explodeRadius = 5f;
			componentInChildren.life = 8f;
			componentInChildren.damage = 10;
			componentInChildren.GunType = (WeaponType)type;
			componentInChildren.isLocal = false;
			return;
		}
		if (type == 26)
		{
			gameObject.transform.position = pos + Vector3.up * 0.05f;
			gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
			Timer timer = new Timer();
			timer.SetTimer(0.6f, false);
			GrenadeSlimeScript componentInChildren2 = gameObject.GetComponentInChildren<GrenadeSlimeScript>();
			componentInChildren2.slimeDamage = damage;
			componentInChildren2.disappearTime = 4f;
			componentInChildren2.slimeTimer = timer;
			componentInChildren2.isLocal = false;
			return;
		}
		if (type == 4 || type == 24)
		{
			GrenadePhysicsScript component2 = gameObject.GetComponent<GrenadePhysicsScript>();
			AdvancedGrenadeShotScript componentInChildren3 = gameObject.GetComponentInChildren<AdvancedGrenadeShotScript>();
			component2.dir = dir;
			component2.life = 8f;
			componentInChildren3.slimeDamage = 20;
			componentInChildren3.slimeDisappearTime = 4f;
			componentInChildren3.slime = false;
			componentInChildren3.dir = dir;
			componentInChildren3.flySpeed = 16f;
			componentInChildren3.explodeRadius = 5f;
			componentInChildren3.life = 8f;
			componentInChildren3.damage = 10;
			componentInChildren3.GunType = (WeaponType)type;
			componentInChildren3.isLocal = false;
			return;
		}
		if (type == 41)
		{
			Player playerByUserID = gameWorld.GetPlayerByUserID(trackingID);
			SniperFirelineScript component3 = gameObject.GetComponent<SniperFirelineScript>();
			component3.beginPos = pos + Vector3.up * 1f;
			component3.endPos = playerByUserID.GetTransform().position + Vector3.up * 1f;
			return;
		}
		ProjectileScript component4 = gameObject.GetComponent<ProjectileScript>();
		component4.dir = dir;
		component4.flySpeed = 16f;
		component4.explodeRadius = 5f;
		component4.life = 8f;
		component4.damage = 10;
		component4.GunType = (WeaponType)type;
		component4.isLocal = false;
		if (type == 16)
		{
			component4.isPenerating = true;
		}
		if (type == 20)
		{
			component4.isPenerating = true;
			component4.isReflecting = true;
		}
		if (type == 40)
		{
			GameApp.SpringBulletCount++;
			component4.NeedReflectTime = 3;
			if (GameApp.SpringBulletCount > 100)
			{
				gameObject.transform.GetChild(0).gameObject.SetActive(false);
				gameObject.transform.GetChild(1).gameObject.SetActive(false);
				gameObject.GetComponent<TrailRenderer>().enabled = false;
			}
		}
		if ((trackingID == -1 || type != 19) && type != 40 && type != 10 && type != 13 && type != 3 && type != 9 && type != 42)
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			Player playerByUserID2 = gameWorld.GetPlayerByUserID(trackingID);
			if (playerByUserID2 != null)
			{
				component4.trackingPlayer = playerByUserID2;
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
			{
				CMIGift giftWithId = gameWorld.GetGiftWithId(trackingID);
				if (giftWithId != null)
				{
					component4.trackingGift = giftWithId;
				}
			}
		}
		else
		{
			Enemy enemyByID = gameWorld.GetEnemyByID("E_" + trackingID);
			if (enemyByID != null)
			{
				component4.trackingEnemy = enemyByID;
			}
		}
	}
}
