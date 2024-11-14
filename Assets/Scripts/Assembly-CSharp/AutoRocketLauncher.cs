using UnityEngine;

public class AutoRocketLauncher : Weapon
{
	protected const float shootLastingTime = 0.5f;

	protected float rocketFlySpeed = 16f;

	protected static int sbulletCount;

	protected AlphaAnimationScript aas;

	protected Transform gunfire02;

	protected Transform gunfire03;

	protected int shootCount;

	protected float lastAutoShootTime;

	public override int BulletCount
	{
		get
		{
			return sbulletCount;
		}
		set
		{
			sbulletCount = value;
		}
	}

	public AutoRocketLauncher()
	{
		maxCapacity = 9999;
		base.IsSelectedForBattle = false;
	}

	public override WeaponType GetWeaponType()
	{
		return WeaponType.AutoRocketLauncher;
	}

	public override string GetAnimationSuffix()
	{
		return "_BLACKSTARS";
	}

	public override string GetAnimationSuffixAlter()
	{
		return "_bazinga";
	}

	public override bool IsTypeOfLoopShootingWeapon()
	{
		return false;
	}

	public override void GunOn()
	{
		base.GunOn();
		SetWeaponShootSpeed(2.4f);
	}

	public override void Init(Player player)
	{
		base.Init(player);
		gunfire = gun.transform.Find("Point01");
		gunfire02 = gun.transform.Find("Point02");
		gunfire03 = gun.transform.Find("Point03");
		aas = gun.GetComponent<AlphaAnimationScript>();
		base.FireSoundName = "Audio/rpg/rpg-31_FiringSound";
		base.ExplodeSoundName = "Audio/rpg/rpg-31_boom";
	}

	public override void Loop(float deltaTime)
	{
		if (shootCount == 1)
		{
			if (Time.time - lastAutoShootTime > 0.2f)
			{
				Shoot(2);
				shootCount = 2;
				lastAutoShootTime = Time.time;
			}
		}
		else if (shootCount == 2 && Time.time - lastAutoShootTime > 0.2f)
		{
			Shoot(3);
			shootCount = 0;
			lastAutoShootTime = Time.time;
		}
	}

	public void Shoot(int shootIndex)
	{
		AudioManager.GetInstance().PlaySound(base.FireSoundName);
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
		GameObject original = Resources.Load("Effect/BlackStar/DAN") as GameObject;
		Transform transform = gunfire;
		switch (shootIndex)
		{
		case 2:
			transform = gunfire02;
			break;
		case 3:
			transform = gunfire03;
			break;
		}
		GameObject gameObject = Object.Instantiate(original, transform.position, Quaternion.LookRotation(normalized2)) as GameObject;
		ProjectileScript component = gameObject.GetComponent<ProjectileScript>();
		component.dir = normalized2;
		component.flySpeed = rocketFlySpeed;
		component.explodeRadius = bombRange;
		component.hitForce = hitForce;
		component.life = 8f;
		component.damage = (int)damage;
		component.GunType = WeaponType.AutoRocketLauncher;
		component.targetPos = aimTarget;
		component.bagIndex = (byte)GameApp.GetInstance().GetUserState().GetWeaponBagIndex(this);
		component.weapon = this;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerFireRocketRequest request = new PlayerFireRocketRequest(14, gunfire.position, normalized2);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override void DamageBoothByArmor()
	{
		base.DamageBoothByArmor();
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.RPG_BOOTH);
	}

	public override float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return base.GetDamageAfterBoothByArmor(skill) * (1f + skill.GetSkill(SkillsType.RPG_BOOTH));
	}

	public override void Attack(float deltaTime)
	{
		lastAttackTime = Time.time;
		SetLastAttackTimeShared();
		shootCount = 1;
		lastAutoShootTime = Time.time;
		GameApp.GetInstance().GetUserState().UseEnegy(enegyConsume);
		if (aas != null)
		{
			aas.StartAnimation();
		}
		Shoot(1);
	}
}
