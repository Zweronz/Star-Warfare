using UnityEngine;

public class GrenadeLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 20f;

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

	public GrenadeLauncher()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.GrenadeLauncher;
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
		GameObject original = Resources.Load("Effect/GrenadeShot") as GameObject;
		GameObject gameObject = Object.Instantiate(original, gunfire.position, Quaternion.LookRotation(normalized2)) as GameObject;
		GrenadePhysicsScript component = gameObject.GetComponent<GrenadePhysicsScript>();
		AdvancedGrenadeShotScript component2 = gameObject.transform.GetChild(0).GetComponent<AdvancedGrenadeShotScript>();
		component.dir = normalized2;
		component.life = 8f;
		component2.slime = false;
		component2.dir = normalized2;
		component2.flySpeed = rocketFlySpeed;
		component2.explodeRadius = bombRange;
		component2.hitForce = hitForce;
		component2.life = 8f;
		component2.damage = (int)damage;
		component2.GunType = WeaponType.GrenadeLauncher;
		component2.targetPos = aimTarget;
		component2.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(4, gunfire.position, normalized2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}
}
