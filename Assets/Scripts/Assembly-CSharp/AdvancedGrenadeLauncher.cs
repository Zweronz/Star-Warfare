using UnityEngine;

public class AdvancedGrenadeLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 20f;

	protected static int sbulletCount;

	protected float poisonZoneInterval = 5f;

	protected float poisonZoneTime = 4f;

	protected Timer poisonZoneTimer;

	protected float trackRange = 8f;

	protected float trackSpeed = 10f;

	protected int trackChance = 30;

	protected byte trackID;

	protected int poisonZoneDamage
	{
		get
		{
			int num = (int)((float)(20 * level) * 1.2f);
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				num /= 20;
			}
			return num;
		}
	}

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

	public AdvancedGrenadeLauncher()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
		poisonZoneTimer = new Timer();
		poisonZoneTimer.SetTimer(poisonZoneInterval, false);
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AdvancedGrenadeLauncher;
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
		SetWeaponShootSpeed(1f);
	}

	public override void Init(Player player)
	{
		base.Init(player);
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
		PlayGunAnimation("fire", WrapMode.ClampForever);
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
		if (poisonZoneTimer.Ready())
		{
			poisonZoneTimer.Do();
			CreateTrackingPoisonGrenade(normalized2);
			return;
		}
		int num = Random.Range(0, 100);
		if (num < 30)
		{
			CreateTrackingGrenade(normalized2);
		}
		else
		{
			CreateNormalGrenade(normalized2);
		}
	}

	private void CreateNormalGrenade(Vector3 dir)
	{
		GameObject original = Resources.Load("Effect/GrenadeShot") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(dir)) as GameObject;
		GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
		AdvancedGrenadeShotScript componentInChildren = gameObject.GetComponentInChildren<AdvancedGrenadeShotScript>();
		component.dir = dir;
		component.life = 8f;
		componentInChildren.dir = dir;
		componentInChildren.flySpeed = rocketFlySpeed;
		componentInChildren.explodeRadius = bombRange;
		componentInChildren.hitForce = hitForce;
		componentInChildren.life = 8f;
		componentInChildren.damage = (int)damage;
		componentInChildren.GunType = WeaponType.AdvancedGrenadeLauncher;
		componentInChildren.targetPos = aimTarget;
		componentInChildren.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(24, gunfire.position, dir);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void CreateTrackingGrenade(Vector3 dir)
	{
		GameObject original = Resources.Load("Effect/TrackingGrenadeShot") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(dir)) as GameObject;
		GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
		TrackingGrenadeShotScript componentInChildren = gameObject.GetComponentInChildren<TrackingGrenadeShotScript>();
		component.dir = dir;
		component.life = 8f;
		componentInChildren.userID = Lobby.GetInstance().GetChannelID();
		componentInChildren.trackID = trackID;
		trackID = (byte)((trackID + 1) % 100);
		componentInChildren.slimeDamage = poisonZoneDamage;
		componentInChildren.slimeDisappearTime = poisonZoneTime;
		componentInChildren.trackSpeed = trackSpeed;
		componentInChildren.trackRadius = trackRange;
		componentInChildren.dir = dir;
		componentInChildren.flySpeed = rocketFlySpeed;
		componentInChildren.explodeRadius = bombRange;
		componentInChildren.hitForce = hitForce;
		componentInChildren.life = 8f;
		componentInChildren.damage = (int)damage;
		componentInChildren.GunType = WeaponType.AdvancedGrenadeLauncher;
		componentInChildren.targetPos = aimTarget;
		componentInChildren.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(25, gunfire.position, dir);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void CreateTrackingPoisonGrenade(Vector3 dir)
	{
		GameObject original = Resources.Load("Effect/TrackingGrenadeShot") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(dir)) as GameObject;
		GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
		TrackingGrenadeShotScript componentInChildren = gameObject.GetComponentInChildren<TrackingGrenadeShotScript>();
		component.dir = dir;
		component.life = 8f;
		componentInChildren.userID = Lobby.GetInstance().GetChannelID();
		componentInChildren.trackID = trackID;
		trackID = (byte)((trackID + 1) % 100);
		componentInChildren.slime = true;
		componentInChildren.slimeDamage = poisonZoneDamage;
		componentInChildren.slimeDisappearTime = poisonZoneTime;
		componentInChildren.trackSpeed = trackSpeed;
		componentInChildren.trackRadius = trackRange;
		componentInChildren.dir = dir;
		componentInChildren.flySpeed = rocketFlySpeed;
		componentInChildren.explodeRadius = bombRange;
		componentInChildren.hitForce = hitForce;
		componentInChildren.life = 8f;
		componentInChildren.damage = (int)damage;
		componentInChildren.GunType = WeaponType.AdvancedGrenadeLauncher;
		componentInChildren.targetPos = aimTarget;
		componentInChildren.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(25, gunfire.position, dir);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void Loop(float deltaTime)
	{
		if (player == null)
		{
			return;
		}
		if (player.State != Player.ATTACK_STATE)
		{
			PlayGunAnimation("idle", WrapMode.Loop);
		}
		if (gunRenderers != null && gunRenderers.Length > 0)
		{
			if (poisonZoneTimer.Ready())
			{
				gunRenderers[gunRenderers.Length - 1].enabled = true;
			}
			else
			{
				gunRenderers[gunRenderers.Length - 1].enabled = false;
			}
		}
	}

	protected void PlayGunAnimation(string name, WrapMode mode)
	{
		if (!gun.animation.IsPlaying(name))
		{
			gun.animation[name].wrapMode = mode;
			gun.animation[name].speed = 1f;
			gun.animation.Play(name);
		}
	}
}
