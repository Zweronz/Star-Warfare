using UnityEngine;

public abstract class Weapon
{
	protected bool attacked;

	protected float lastAttackTime;

	protected GameObject hitParticles;

	protected GameObject projectile;

	protected Camera cameraComponent;

	protected Transform cameraTransform;

	protected ThirdPersonStandardCameraScript gameCamera;

	protected Transform gunfire;

	protected GameObject gun;

	protected GameObject gunfireObj;

	protected Transform weaponBoneTrans;

	protected AudioSource shootAudio;

	protected GameConfig gConfig;

	protected GameWorld gameWorld;

	protected Player player;

	protected Vector3 aimTarget;

	protected int gunID = 1;

	protected bool isCDing;

	protected float hitForce;

	protected float range;

	protected float bombRange;

	protected int maxCapacity;

	protected int maxGunLoad;

	protected int capacity;

	protected int bulletCount;

	protected float maxDeflection;

	protected Vector2 deflection;

	protected float damage;

	protected float damageInit;

	protected float splashDamage;

	protected float splashDamageInit;

	protected float splashDuration;

	protected float bombRangeInit;

	protected float attackFrenquency;

	protected float attackFrenquencyInit;

	protected float accuracy;

	protected int enegyConsume = 5;

	protected float speedDrag;

	protected Vector3 lastHitPosition;

	protected int price;

	protected int mithril;

	protected byte unlockLevel;

	protected byte displayOrder;

	protected byte level;

	protected Vector3 center;

	protected Timer playSoundTimer = new Timer();

	protected bool waitForAttackAnimationStop = true;

	public Renderer[] gunRenderers;

	public int DamageLevel { get; set; }

	public int FrequencyLevel { get; set; }

	public int AccuracyLevel { get; set; }

	public WeaponConfig WConf { get; set; }

	public bool IsSelectedForBattle { get; set; }

	public WeaponAdjuster Adjuster { get; set; }

	public string FireSoundName { get; set; }

	public string ExplodeSoundName { get; set; }

	public int AimID { get; set; }

	public int GunID
	{
		get
		{
			return gunID;
		}
		set
		{
			gunID = value;
		}
	}

	public byte DisplayOrder
	{
		get
		{
			return displayOrder;
		}
		set
		{
			displayOrder = value;
		}
	}

	public string Info
	{
		get
		{
			return Name;
		}
	}

	public string Name { get; set; }

	public int Price
	{
		get
		{
			return price;
		}
	}

	public int Mithril
	{
		get
		{
			return mithril;
		}
	}

	public byte Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public float Accuracy
	{
		get
		{
			return accuracy;
		}
		set
		{
			accuracy = value;
		}
	}

	public GameObject Gun
	{
		get
		{
			return gun;
		}
		set
		{
			gun = value;
		}
	}

	public Vector3 Center
	{
		get
		{
			return center;
		}
		set
		{
			center = value;
		}
	}

	public int MaxGunLoad
	{
		get
		{
			return maxGunLoad;
		}
	}

	public float Damage
	{
		get
		{
			return damage;
		}
		set
		{
			damage = value;
		}
	}

	public float AttackFrequency
	{
		get
		{
			return attackFrenquency;
		}
		set
		{
			attackFrenquency = value;
		}
	}

	public int EnegyConsume
	{
		get
		{
			return enegyConsume;
		}
		set
		{
			enegyConsume = value;
		}
	}

	public virtual int BulletCount
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int Capacity
	{
		get
		{
			return capacity;
		}
	}

	public Vector2 Deflection
	{
		get
		{
			return deflection;
		}
	}

	public byte UnlockLevel
	{
		get
		{
			return unlockLevel;
		}
	}

	public Weapon()
	{
	}

	public abstract void Attack(float deltaTime);

	public virtual void StopFire()
	{
	}

	public abstract WeaponType GetWeaponType();

	public bool GetWaitForAttackAnimationStop()
	{
		return waitForAttackAnimationStop;
	}

	public virtual bool IsTypeOfLoopShootingWeapon()
	{
		return true;
	}

	public void EnableGunObject(bool bEnable)
	{
		if (!bEnable)
		{
			gun.SetActiveRecursively(false);
		}
		else
		{
			gun.SetActiveRecursively(true);
		}
	}

	public abstract string GetAnimationSuffix();

	public abstract string GetAnimationSuffixAlter();

	public virtual void DoLogic()
	{
	}

	public virtual void LoadConfig(byte ownedState)
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[13];
		if (unitDataTable == null)
		{
			return;
		}
		Name = unitDataTable.GetData(gunID, 0, string.Empty, false);
		damage = unitDataTable.GetData(gunID, 1, 0, false);
		attackFrenquency = (float)(short)unitDataTable.GetData(gunID, 2, 0, false) / 100f;
		range = unitDataTable.GetData(gunID, 3, 0, false);
		bombRange = (float)(sbyte)unitDataTable.GetData(gunID, 4, 0, false) / 10f;
		splashDamage = unitDataTable.GetData(gunID, 5, 0, false);
		splashDuration = (float)(sbyte)unitDataTable.GetData(gunID, 6, 0, false) / 10f;
		speedDrag = (float)(sbyte)unitDataTable.GetData(gunID, 7, 0, false) / 10f;
		enegyConsume = (short)unitDataTable.GetData(gunID, 8, 0, false);
		price = unitDataTable.GetData(gunID, 9, 0, false);
		mithril = unitDataTable.GetData(gunID, 10, 0, false);
		unlockLevel = (byte)unitDataTable.GetData(gunID, 13, 0, false);
		AimID = (byte)unitDataTable.GetData(gunID, 14, 0, false);
		displayOrder = (byte)unitDataTable.GetData(gunID, 15, 0, false);
		damageInit = damage;
		attackFrenquencyInit = attackFrenquency;
		bombRangeInit = bombRange;
		splashDamageInit = splashDamage;
		Adjuster = WeaponDirection.adjusters[gunID];
		if (ownedState != 15)
		{
			for (int i = 0; i < ownedState; i++)
			{
				Upgrade();
			}
		}
	}

	public void CreateGun()
	{
		string path = string.Format("Weapon/gun{0:D2}", gunID);
		Object original = Resources.Load(path);
		gun = (GameObject)Object.Instantiate(original, player.GetTransform().position, player.GetTransform().rotation);
		WeaponResourceConfig.RotateGun(gun, gunID);
		gunRenderers = WeaponResourceConfig.GetWeaponRenderers(gun, gunID);
	}

	public void CreateGunForUI()
	{
		string path = string.Format("NoMipMapRes/Weapon/gun{0:D2}", gunID);
		Object original = Resources.Load(path);
		gun = Object.Instantiate(original) as GameObject;
		WeaponResourceConfig.RotateGunInUI(gun, gunID);
		gunRenderers = WeaponResourceConfig.GetWeaponRenderers(gun, gunID);
	}

	public float GetLastAttackTime()
	{
		return lastAttackTime;
	}

	public void SetLastAttackTime(float time)
	{
		lastAttackTime = time;
	}

	public float GetRange()
	{
		return range;
	}

	public float GetDamageInit()
	{
		return damageInit;
	}

	public float GetSplashDamageInit()
	{
		return splashDamageInit;
	}

	public float GetSplashDuration()
	{
		return splashDuration;
	}

	public float GetBombRangeInit()
	{
		return bombRangeInit;
	}

	public float GetAttackFrenquencyInit()
	{
		return attackFrenquencyInit;
	}

	public float GetSpeedDrag()
	{
		return speedDrag;
	}

	public int GetGunID()
	{
		return gunID;
	}

	public void Upgrade()
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().weaponUpgrade;
		damage += (float)weaponUpgrade.GetDamage(level) * damageInit / 100f;
		level++;
		DamageLevel++;
	}

	public float GetNextLevelDamage()
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().weaponUpgrade;
		return damage + damageInit * (float)weaponUpgrade.GetDamage(level);
	}

	public bool IsMaxLevelDamage()
	{
		return (float)DamageLevel >= WConf.damageConf.maxLevel;
	}

	public bool IsMaxLevelCD()
	{
		return (float)FrequencyLevel >= WConf.attackRateConf.maxLevel;
	}

	public bool IsMaxLevelAccuracy()
	{
		return (float)AccuracyLevel >= WConf.accuracyConf.maxLevel;
	}

	public int GetDamageUpgradePrice()
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().GetWeaponUpgrade();
		int num = level;
		float num2 = price;
		float num3 = (float)weaponUpgrade.GetPrice(num) / 100f;
		float num4 = num2;
		num4 *= num3;
		return (int)num4 / 100 * 100;
	}

	public int GetFrequencyUpgradePrice()
	{
		int frequencyLevel = FrequencyLevel;
		float basePrice = WConf.attackRateConf.basePrice;
		float upPriceFactor = WConf.attackRateConf.upPriceFactor;
		float num = basePrice;
		for (int i = 0; i < frequencyLevel; i++)
		{
			num *= 1f + upPriceFactor;
		}
		return (int)num / 100 * 100;
	}

	public int GetAccuracyUpgradePrice()
	{
		int accuracyLevel = AccuracyLevel;
		float basePrice = WConf.accuracyConf.basePrice;
		float upPriceFactor = WConf.accuracyConf.upPriceFactor;
		float num = basePrice;
		for (int i = 0; i < accuracyLevel; i++)
		{
			num *= 1f + upPriceFactor;
		}
		return (int)num / 100 * 100;
	}

	public float GetNextLevelFrequency()
	{
		return attackFrenquency - attackFrenquency * WConf.attackRateConf.upFactor;
	}

	public float GetNextLevelAccuracy()
	{
		return accuracy + accuracy * WConf.accuracyConf.upFactor;
	}

	public virtual void Init(Player player)
	{
		gameWorld = GameApp.GetInstance().GetGameWorld();
		gameCamera = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
		cameraComponent = gameCamera.camera;
		cameraTransform = gameCamera.CameraTransform;
		this.player = player;
		hitForce = 0f;
		weaponBoneTrans = player.GetTransform().Find(BoneName.Weapon);
		CreateGun();
		gun.transform.parent = weaponBoneTrans;
		shootAudio = gun.audio;
		if (shootAudio == null)
		{
		}
		gunfire = WeaponResourceConfig.GetWeaponGunFire(gun, gunID);
		GunOff();
	}

	public virtual void DamageBoothByArmor()
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().GetWeaponUpgrade();
		damage = damageInit;
		splashDamage = splashDamageInit;
		bombRange = bombRangeInit;
		attackFrenquency = attackFrenquencyInit;
		for (int i = 0; i < level; i++)
		{
			damage += (float)weaponUpgrade.GetDamage(i) * damageInit / 100f;
		}
		damage *= 1f + player.GetSkills().GetSkill(SkillsType.ATTACK_BOOTH) + gameWorld.TeamSkills.teamAttackBooth;
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			damage = VSMath.GetDamageInVS(damage);
			splashDamage = VSMath.GetDamageInVS(splashDamage) * 0.2f;
			if (GetWeaponType() != WeaponType.AutoBow && GetWeaponType() != WeaponType.LightBow && GetWeaponType() != WeaponType.TrackingGun && GetWeaponType() != WeaponType.TheArrow)
			{
				bombRange = VSMath.GetExplodeRadiusInVS(bombRangeInit);
			}
			else if (GetWeaponType() != WeaponType.FlyGrenadeLauncher)
			{
				bombRange = VSMath.GetFlyGrenadeRadiusInVS(bombRangeInit);
			}
			if (GetWeaponType() == WeaponType.AssaultRifle)
			{
				attackFrenquency = attackFrenquencyInit * VSMath.ASSAULT_RIFLE_FREQUENCY_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.AdvancedAssaultRifle)
			{
				attackFrenquency = attackFrenquencyInit * VSMath.ASSAULT_RIFLE_FREQUENCY_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.LaserRifle)
			{
				attackFrenquency = attackFrenquencyInit * VSMath.LASER_RIFLE_FREQUENCY_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.MachineGun || GetWeaponType() == WeaponType.AdvancedMachineGun)
			{
				damage *= VSMath.MACHINE_GUN_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.LightBow || GetWeaponType() == WeaponType.AutoBow || GetWeaponType() == WeaponType.TheArrow)
			{
				damage *= VSMath.BOW_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.GrenadeLauncher || GetWeaponType() == WeaponType.AdvancedGrenadeLauncher)
			{
				damage *= VSMath.GL_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.Sword || GetWeaponType() == WeaponType.AdvancedSword)
			{
				damage *= VSMath.SWORD_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.RocketLauncher || GetWeaponType() == WeaponType.AutoRocketLauncher)
			{
				damage *= VSMath.RPG_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.Sniper || GetWeaponType() == WeaponType.AdvancedSniper || GetWeaponType() == WeaponType.RelectionSniper)
			{
				damage *= VSMath.SNIPER_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.TrackingGun)
			{
				damage *= VSMath.TRACKINGGUN_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.PingPongLauncher)
			{
				damage *= VSMath.PINGPONGLAUNCHER_DAMAGE_BOOTH;
			}
			else if (GetWeaponType() == WeaponType.FlyGrenadeLauncher)
			{
				damage *= VSMath.FLY_GRENADE_DAMAGE_BOOTH;
			}
		}
		float skill = player.GetSkills().GetSkill(SkillsType.ATTACK_FRENQUENCY);
		if (skill != 0f)
		{
			attackFrenquency *= 1f + skill;
		}
	}

	public virtual float SimpleDamage()
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().GetWeaponUpgrade();
		float num = damageInit;
		if (level != 0 && level != 15)
		{
			for (int i = 0; i < level; i++)
			{
				num += (float)weaponUpgrade.GetDamage(i) * damageInit / 100f;
			}
		}
		return num;
	}

	public virtual float SimpleDamage(int gunLevel)
	{
		WeaponUpgrade weaponUpgrade = GameApp.GetInstance().GetUserState().GetWeaponUpgrade();
		float num = damageInit;
		if (gunLevel != 0 && gunLevel != 15)
		{
			for (int i = 0; i < gunLevel; i++)
			{
				num += (float)weaponUpgrade.GetDamage(i) * damageInit / 100f;
			}
		}
		return num;
	}

	public virtual float GetDamageAfterBoothByArmor(PlayerSkill skill)
	{
		return SimpleDamage() * (1f + skill.GetSkill(SkillsType.ATTACK_BOOTH));
	}

	public virtual void Loop(float deltaTime)
	{
	}

	public virtual void AutoDestructEffect()
	{
	}

	public virtual void AutoAim(float deltaTime)
	{
	}

	public virtual void PlaySound()
	{
	}

	public virtual void GetBullet()
	{
		capacity += maxGunLoad;
		capacity = Mathf.Clamp(capacity, 0, maxCapacity);
	}

	public void AddBullets(int num)
	{
		BulletCount += num;
		BulletCount = Mathf.Clamp(BulletCount, 0, 9999);
	}

	public virtual void MaxBullet()
	{
		BulletCount = maxGunLoad;
	}

	public virtual void GunOn()
	{
		if (gun != null)
		{
			Renderer[] array = gunRenderers;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = true;
			}
		}
	}

	public bool CheckBullets()
	{
		if (player.IsLocal())
		{
			int enegy = GameApp.GetInstance().GetUserState().Enegy;
			if (enegy < enegyConsume)
			{
				StopFire();
				return false;
			}
			return true;
		}
		return true;
	}

	public virtual bool HaveBullets()
	{
		if (player.IsLocal())
		{
			int enegy = GameApp.GetInstance().GetUserState().Enegy;
			if (enegy < enegyConsume)
			{
				PlayBlankShootSound();
				StopFire();
				return false;
			}
			return true;
		}
		return true;
	}

	public virtual void PlayBlankShootSound()
	{
		AudioManager.GetInstance().PlaySoundSingle("Audio/blank/blank_shot01");
	}

	public virtual void CreateTrajectory()
	{
		SetLastAttackTimeShared();
		lastAttackTime = Time.time;
	}

	public virtual bool CoolDown()
	{
		if (Time.time - lastAttackTime > attackFrenquency && CanAttackForAttackTimeShared())
		{
			return true;
		}
		return false;
	}

	public void SetWeaponShootSpeed(float speed)
	{
		if (player.GetGameObject().animation[AnimationString.FlyAttack + GetAnimationSuffix()] != null)
		{
			player.GetGameObject().animation[AnimationString.FlyAttack + GetAnimationSuffix()].speed *= speed / attackFrenquency;
		}
		if (player.GetGameObject().animation[AnimationString.Attack + GetAnimationSuffix()] != null)
		{
			player.GetGameObject().animation[AnimationString.Attack + GetAnimationSuffix()].speed *= speed / attackFrenquency;
		}
		if (player.GetGameObject().animation[AnimationString.RunAttack + GetAnimationSuffix()] != null)
		{
			player.GetGameObject().animation[AnimationString.RunAttack + GetAnimationSuffix()].speed *= speed / attackFrenquency;
		}
	}

	public virtual void GunOff()
	{
		if (gun != null)
		{
			Renderer[] array = gunRenderers;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = false;
			}
		}
		StopFire();
	}

	public void SetLastAttackTimeShared()
	{
		if (player != null)
		{
			player.LastAttackTimeShared = Time.time;
		}
	}

	public bool CanAttackForAttackTimeShared()
	{
		if (player == null)
		{
			return true;
		}
		if (Time.time - player.LastAttackTimeShared > attackFrenquency)
		{
			return true;
		}
		return false;
	}
}
