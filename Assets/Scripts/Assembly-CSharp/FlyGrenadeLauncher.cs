using UnityEngine;

public class FlyGrenadeLauncher : Weapon
{
	protected static int ID;

	protected GameObject vfxObj;

	protected static int sbulletCount;

	public override int BulletCount
	{
		get
		{
			return 0;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public FlyGrenadeLauncher()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.FlyGrenadeLauncher;
	}

	public override string GetAnimationSuffix()
	{
		return "_grenade_launcher";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_shotgun";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void GunOn()
	{
		base.GunOn();
		if (vfxObj != null)
		{
			vfxObj.SetActive(true);
		}
		SetWeaponShootSpeed(1f);
	}

	public override void GunOff()
	{
		base.GunOff();
		if (vfxObj != null)
		{
			vfxObj.SetActive(false);
		}
	}

	public override void Init(Player player)
	{
		base.Init(player);
		vfxObj = gun.transform.Find("HotWing_TX").gameObject;
		if (vfxObj != null)
		{
			vfxObj.SetActive(false);
		}
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.GRENADE_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.GRENADE_BOOTH));
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
		AudioManager.GetInstance().PlaySoundAt("Audio/gl/grenade_launcher_fire", player.GetTransform().position);
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		PlayGunAnimation("attack", WrapMode.ClampForever);
		GameObject original = Resources.Load("SW2_Effect/HotWing_TX_bao") as GameObject;
		gunfireObj = Object.Instantiate(original, gun.transform.position, Quaternion.identity) as GameObject;
		gunfireObj.transform.parent = gun.transform;
		gunfireObj.transform.localRotation = Quaternion.identity;
		gunfireObj.name = "HotWing_TX_bao";
		AutoDestroyScript autoDestroyScript = gunfireObj.AddComponent<AutoDestroyScript>();
		autoDestroyScript.life = 3f;
		Ray ray = default(Ray);
		Vector3 vector = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, (float)Screen.height - gameCamera.ReticlePosition.y, 50f));
		Vector3 normalized = (vector - cameraTransform.position).normalized;
		ray = new Ray(cameraTransform.position + normalized * 1.8f, normalized);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000f, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.REMOTE_PLAYER) | (1 << PhysicsLayer.GIFT)))
		{
			aimTarget = hitInfo.point;
		}
		else
		{
			aimTarget = cameraTransform.TransformPoint(0f, 0f, 1000f);
		}
		Vector3 normalized2 = (aimTarget - gunfire.position).normalized;
		normalized2.y += 0.2f;
		CreateFlyGrenade(normalized2);
	}

	private void CreateFlyGrenade(Vector3 dir)
	{
		GameObject original = Resources.Load("SW2_Effect/HotWing_Bullet") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position - gunfire.transform.forward * 0.5f, Quaternion.identity) as GameObject;
		gameObject.layer = PhysicsLayer.PROJECTILE;
		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = 0.2f;
		sphereCollider.isTrigger = true;
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.useGravity = false;
		FlyGrenadeShotScript flyGrenadeShotScript = gameObject.AddComponent<FlyGrenadeShotScript>();
		flyGrenadeShotScript.dir = dir;
		flyGrenadeShotScript.life = 3f;
		flyGrenadeShotScript.flySpeed = 10f;
		flyGrenadeShotScript.explodeRadius = bombRange;
		flyGrenadeShotScript.damage = (int)damage;
		flyGrenadeShotScript.searchRange = range;
		flyGrenadeShotScript.isLocal = true;
		flyGrenadeShotScript.grenadeId = (byte)ID;
		ID = (ID + 1) % 100;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			FlyGrenadeCreateRequest request = new FlyGrenadeCreateRequest(Lobby.GetInstance().GetChannelID(), flyGrenadeShotScript.grenadeId, gunfire.position - gunfire.transform.forward * 0.5f, dir);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected void PlayGunAnimation(string name, WrapMode mode)
	{
		gun.GetComponent<Animation>()[name].time = 0f;
		gun.GetComponent<Animation>()[name].wrapMode = mode;
		gun.GetComponent<Animation>()[name].speed = 1f;
		gun.GetComponent<Animation>().Play(name);
	}
}
