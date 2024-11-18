using UnityEngine;

public class Dragon : EnemyBoss
{
	protected struct FlameChecker
	{
		public float startTime;

		public float endTime;

		public float startAngle;

		public float endAngle;

		public float distanceSqr;
	}

	protected const float FLY_HEIGHT = 5f;

	public static EnemyState START_FLY_STATE = new StartFlyState();

	public static EnemyState START_RUSH_STATE = new StartFlyRushState();

	public static EnemyState FLY_IDLE_STATE = new FlyIdleState();

	public static EnemyState FLY_STATE = new FlyState();

	public static EnemyState FLY_DIVE_STATE = new FlyDiveState();

	public static EnemyState FLY_RUSH_STATE = new FlyRushState();

	public static EnemyState FLY_SHOT_STATE = new FlyShotState();

	public static EnemyState PU_STATE = new PuState();

	public static EnemyState LANDING_STATE = new LandingState();

	public static EnemyState SHOT_STATE = new ShotState();

	public static EnemyState FLAME_STATE = new FlameState();

	public static EnemyState RAGE_STATE = new DragonRageState();

	public static EnemyState DRAGON_GOTHIT_STATE = new DragonGotHitState();

	public static EnemyState FLY_FLAME_STATE = new FlyFlameState();

	public static EnemyState NORMAL_ATTACK_STATE = new DragonNormalAttackState();

	public static EnemyState FLY_RUSH_END_STATE = new FlyRushEndState();

	protected FlameChecker[] flameCheckers = new FlameChecker[5];

	protected GameObject mouthFire;

	protected Collider headCollider;

	protected Collider flyMouthFireCollider;

	protected Timer windTimer = new Timer();

	protected float lastFlyTime;

	protected float lastFlyIdleTime;

	protected float lastGroundIdleTime;

	protected float lastLandingTime;

	protected Timer dragonBigFlyAudioTimer = new Timer();

	protected Timer dragonSmallFlyAudioTimer = new Timer();

	protected bool needLandingToRage;

	protected float startRushDistance;

	protected float stopRushDistance;

	protected float stopDiveDistance;

	protected float shotExplodeRadius;

	protected float flyShotExplodeRadius;

	protected float flySpeed;

	protected float diveSpeed;

	protected float rushSpeed;

	protected float puSpeed;

	protected float attackSpeed;

	protected float maxAttackSpeed;

	protected float puDeceleration;

	protected float shotSpeed;

	protected float flyShotSpeed;

	protected int puDamage;

	protected int flameDamage;

	protected int flyShotDamage;

	protected int shotExplodeDamage;

	protected int flyShotExplodeDamage;

	protected float touchDamageDistance;

	protected float attackDamageDistance;

	protected float flameDamageDistance;

	protected float rushDamageDistance;

	protected float puDamageDistance;

	protected float attackKnockSpeed;

	protected float rushKnockSpeed;

	protected float puKnockSpeed;

	protected float[] maxFlyTime = new float[2];

	protected float[] maxGroundTime = new float[2];

	protected float[] maxFlyIdleTime = new float[2];

	protected float[] maxGroundIdleTime = new float[2];

	protected int[] maxShotTimes = new int[2];

	protected int[] maxFlyShotTimes = new int[2];

	protected int[] probability1 = new int[2];

	protected int[] probability2 = new int[2];

	protected int[] probability3 = new int[2];

	protected int[] probability4 = new int[2];

	protected int[] probability5 = new int[2];

	protected int[] probability6 = new int[2];

	protected string puAnimation;

	public Vector3 DiveTargetPos { get; set; }

	public Vector3 RushTargetPos { get; set; }

	public bool NeedLandToRage
	{
		get
		{
			return needLandingToRage;
		}
		set
		{
			needLandingToRage = value;
		}
	}

	public float StartRushDistance
	{
		get
		{
			return startRushDistance;
		}
	}

	public float MaxFlyIdleTime
	{
		get
		{
			return maxFlyIdleTime[(int)mindState];
		}
	}

	public float MaxFlyTime
	{
		get
		{
			return maxFlyTime[(int)mindState];
		}
	}

	public float MaxGroundIdleTime
	{
		get
		{
			return maxGroundIdleTime[(int)mindState];
		}
	}

	public float MaxGroundTime
	{
		get
		{
			return maxGroundTime[(int)mindState];
		}
	}

	public int MaxShotTimes
	{
		get
		{
			return maxShotTimes[(int)mindState];
		}
	}

	public int MaxFlyShotTimes
	{
		get
		{
			return maxFlyShotTimes[(int)mindState];
		}
	}

	public int Probability1
	{
		get
		{
			return probability1[(int)mindState];
		}
	}

	public int Probability2
	{
		get
		{
			return probability2[(int)mindState];
		}
	}

	public int Probability3
	{
		get
		{
			return probability3[(int)mindState];
		}
	}

	public int Probability4
	{
		get
		{
			return probability4[(int)mindState];
		}
	}

	public int Probability5
	{
		get
		{
			return probability5[(int)mindState];
		}
	}

	public int Probability6
	{
		get
		{
			return probability6[(int)mindState];
		}
	}

	public string PuAnimation
	{
		get
		{
			return puAnimation;
		}
	}

	public Dragon()
	{
		EnemyBoss.INIT_STATE = new DragonInitState();
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		lastFlyTime = Time.time;
		lastFlyIdleTime = Time.time;
		lastGroundIdleTime = Time.time;
		lastLandingTime = Time.time;
	}

	public void SetFlyIdleTimeNow()
	{
		lastFlyIdleTime = Time.time;
	}

	public float GetFlyIdleDuration()
	{
		return Time.time - lastFlyIdleTime;
	}

	public void SetGroundIdleTimeNow()
	{
		lastGroundIdleTime = Time.time;
	}

	public float GetGroundIdleDuration()
	{
		return Time.time - lastGroundIdleTime;
	}

	public void SetFlyTimeNow()
	{
		lastFlyTime = Time.time;
	}

	public float GetFlyTimeDuration()
	{
		return Time.time - lastFlyTime;
	}

	public void SetLandingTimeNow()
	{
		lastLandingTime = Time.time;
	}

	public float GetLandingTimeDuration()
	{
		return Time.time - lastLandingTime;
	}

	public override void OnDead()
	{
		enemyObject.layer = PhysicsLayer.DEADBODY;
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(3000f, false);
		GameObject original = Resources.Load("Effect/Dragon/dragon_dead_blood") as GameObject;
		Object.Instantiate(original, bipObject.transform.position, Quaternion.identity);
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		StopSoundOnHit();
		PlaySoundSingle("Audio/enemy/Dragon/long_housheng_2");
		EnableGravity(true);
		DisableAllEffect();
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(5f, false);
		GameApp.GetInstance().GetUserState().Achievement.KillBoss(2);
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			AddCashAndExpToPlayer();
		}
	}

	public override bool RemoveDeadBodyTimer()
	{
		if (deadRemoveBodyTimer.Ready())
		{
			return true;
		}
		return false;
	}

	protected override void loadParameters()
	{
		areaCenter = new Vector3(-10f, 0f, -2.5f);
		areaRadius = 17f;
		maxOverRushDistance = 10f;
		hp = 360000;
		hpPercentagePerHit = 0.15f;
		hitTimesForRage = 4;
		stopRushDistance = 6f;
		stopDiveDistance = 0.5f;
		attackRange = 10f;
		startRushDistance = 27f;
		groupAttackDistance = 14f;
		shotExplodeRadius = 5f;
		flyShotExplodeRadius = 4f;
		maxFlyIdleTime[0] = 0.9f;
		maxFlyTime[0] = 20f;
		maxGroundIdleTime[0] = 0.9f;
		maxGroundTime[0] = 12f;
		maxCatchingTime[0] = 5f;
		maxFlyIdleTime[1] = 0.36f;
		maxFlyTime[1] = 20f;
		maxGroundIdleTime[1] = 0.36f;
		maxGroundTime[1] = 12f;
		maxCatchingTime[1] = 4.4f;
		touchDamage = 700;
		attackDamage = 3500;
		rushDamage = 4200;
		puDamage = 5200;
		flameDamage = 3500;
		shotDamage = 700;
		shotExplodeDamage = 2100;
		flyShotDamage = 1400;
		flyShotExplodeDamage = 3200;
		runSpeed = 12f;
		flySpeed = 14f;
		diveSpeed = 32f;
		rushSpeed = 32f;
		turnSpeed = 0.06f;
		upSpeed = 2f;
		downSpeed = 3f;
		shotSpeed = 22f;
		flyShotSpeed = 30f;
		maxAttackSpeed = 20f;
		touchDamageDistance = 5f;
		attackDamageDistance = 6f;
		flameDamageDistance = 18f;
		rushDamageDistance = 8.2f;
		puDamageDistance = 6f;
		touchKnockSpeed = 0.08f;
		attackKnockSpeed = 0.16f;
		rushKnockSpeed = 0.22f;
		puKnockSpeed = 0.2f;
		maxShotTimes[0] = 1;
		maxFlyShotTimes[0] = 1;
		maxShotTimes[1] = 2;
		maxFlyShotTimes[1] = 3;
		probability1[0] = 40;
		probability1[1] = 30;
		probability2[0] = 15;
		probability3[0] = 45;
		probability4[0] = 65;
		probability2[1] = 15;
		probability3[1] = 45;
		probability4[1] = 60;
		probability5[0] = 70;
		probability5[1] = 50;
		probability6[0] = 70;
		probability6[1] = 50;
		flameCheckers[0].startTime = 0.45f;
		flameCheckers[0].endTime = 0.57f;
		flameCheckers[0].startAngle = -45f;
		flameCheckers[0].endAngle = -10f;
		flameCheckers[0].distanceSqr = 100f;
		flameCheckers[1].startTime = 0.57f;
		flameCheckers[1].endTime = 0.73f;
		flameCheckers[1].startAngle = -40f;
		flameCheckers[1].endAngle = -5f;
		flameCheckers[1].distanceSqr = 196f;
		flameCheckers[2].startTime = 0.73f;
		flameCheckers[2].endTime = 0.82f;
		flameCheckers[2].startAngle = -50f;
		flameCheckers[2].endAngle = 5f;
		flameCheckers[2].distanceSqr = 324f;
		flameCheckers[3].startTime = 0.82f;
		flameCheckers[3].endTime = 0.89f;
		flameCheckers[3].startAngle = -20f;
		flameCheckers[3].endAngle = 60f;
		flameCheckers[3].distanceSqr = 324f;
		flameCheckers[4].startTime = 0.89f;
		flameCheckers[4].endTime = 0.93f;
		flameCheckers[4].startAngle = 0f;
		flameCheckers[4].endAngle = 65f;
		flameCheckers[4].distanceSqr = 324f;
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		animation = enemyObject.transform.Find("Dragon").GetComponent<Animation>();
		animation.wrapMode = WrapMode.Loop;
		mouthFire = enemyObject.transform.Find(BoneName.DragonMouth).Find("dragonball_fire0").gameObject;
		bipObject = enemyObject.transform.Find(BoneName.DragonBip02).gameObject;
		bodyCollider = bipObject.GetComponent<Collider>();
		flyBodyCollider = enemyObject.transform.Find(BoneName.Dragon).gameObject.GetComponent<Collider>();
		headCollider = enemyObject.transform.Find(BoneName.DragonHead).gameObject.GetComponent<Collider>();
		flyMouthFireCollider = enemyObject.transform.Find(BoneName.DragonFire).gameObject.GetComponent<Collider>();
		trails = new GameObject[1];
		GameObject original = Resources.Load("Effect/Dragon/dragon_trail") as GameObject;
		Vector3 position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
		trails[0] = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		leftArmTrail = enemyObject.transform.Find(BoneName.DragonLeftTeeth).Find("Trail").gameObject;
		rightArmTrail = enemyObject.transform.Find(BoneName.DragonRightTeeth).Find("Trail").gameObject;
		dragonBigFlyAudioTimer.SetTimer(1f, false);
		dragonSmallFlyAudioTimer.SetTimer(0.4f, false);
		walkAudioTimer.SetTimer(0.6f, false);
		touchtimer.SetTimer(2f, false);
		firetimer.SetTimer(1f, false);
		windTimer.SetTimer(1f, false);
		walkAudioName = "Audio/enemy/Dragon/long_walk";
		puAnimation = AnimationString.ENEMY_PU;
		DisableAllEffect();
	}

	public override void SetState(EnemyState newState)
	{
		base.SetState(newState);
		if (newState == Enemy.IDLE_STATE || newState == FLY_IDLE_STATE)
		{
			DisableAllEffect();
		}
	}

	public override bool NearGround()
	{
		if (enemyTransform.position.y - (float)Global.FLOORHEIGHT < 0.1f)
		{
			return true;
		}
		return false;
	}

	public void Fly()
	{
		LookAtTarget();
		CharacterController characterController = enemyObject.GetComponent<Collider>() as CharacterController;
		if (characterController != null)
		{
			Vector3 vector = enemyTransform.forward * flySpeed;
			characterController.Move(vector * Time.deltaTime);
		}
	}

	public void OnDive()
	{
		maxTurnRadian *= 5f;
		RushTargetPos = GetTargetPlayer().GetTransform().position;
		dir = RushTargetPos - enemyTransform.position;
		dir.y = 0f;
		dir.Normalize();
		Vector3 vector = RushTargetPos - areaCenter;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (magnitude < areaRadius)
		{
			float a = (areaRadius - magnitude) * 0.5f;
			a = Mathf.Min(a, maxOverRushDistance);
			RushTargetPos += a * dir;
		}
		RushTargetPos = new Vector3(RushTargetPos.x, GetTargetPlayer().GetTransform().position.y, RushTargetPos.z);
		DiveTargetPos = enemyTransform.position + 0.5f * (RushTargetPos - enemyTransform.position);
		DiveTargetPos = new Vector3(DiveTargetPos.x, 0f, DiveTargetPos.z);
		SetDir((DiveTargetPos - enemyTransform.position).normalized);
		EnableTrailEffect(true);
	}

	public void Dive()
	{
		enemyTransform.Translate(dir * diveSpeed * Time.deltaTime, Space.World);
		PlaySoundSingle("Audio/enemy/Dragon/long_fly_dive");
	}

	public void OnRush()
	{
		maxTurnRadian *= 5f;
		RushTargetPos = GetTargetPlayer().GetTransform().position;
		dir = RushTargetPos - enemyTransform.position;
		dir.y = 0f;
		dir.Normalize();
		Vector3 vector = RushTargetPos - areaCenter;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (magnitude < areaRadius)
		{
			float a = (areaRadius - magnitude) * 0.5f;
			a = Mathf.Min(a, maxOverRushDistance);
			RushTargetPos += a * dir;
		}
		RushTargetPos = new Vector3(RushTargetPos.x, GetTargetPlayer().GetTransform().position.y, RushTargetPos.z);
		SetDir((RushTargetPos - enemyTransform.position).normalized);
		EnableTrailEffect(true);
	}

	public void Rush()
	{
		enemyTransform.Translate(dir * rushSpeed * Time.deltaTime, Space.World);
		PlaySoundSingle("Audio/enemy/Dragon/long_fly_dive");
	}

	public bool IsEasyToCollideWall()
	{
		Vector3 vector = enemyTransform.position - areaCenter;
		vector.y = 0f;
		float sqrMagnitude = vector.sqrMagnitude;
		Vector3 vector2 = targetToLookAt - areaCenter;
		vector2.y = 0f;
		float sqrMagnitude2 = vector2.sqrMagnitude;
		return sqrMagnitude > 330f && sqrMagnitude2 > 330f;
	}

	public void Pu()
	{
		if (AnimationPlayed(PuAnimation, 0.41f))
		{
			puSpeed -= puDeceleration;
			if (puSpeed < 0f)
			{
				puSpeed = 0f;
			}
			PlaySoundSingle("Audio/enemy/Dragon/long_luodi");
		}
		else if (dragonSmallFlyAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Dragon/long_feixing");
			dragonSmallFlyAudioTimer.Do();
		}
		if (!AnimationPlayed(PuAnimation, 0.6f))
		{
			EnableLeftArmTrail(true);
			EnableRightArmTrail(true);
		}
		else
		{
			EnableLeftArmTrail(false);
			EnableRightArmTrail(false);
		}
		enemyTransform.Translate(enemyTransform.forward * puSpeed * Time.deltaTime, Space.World);
	}

	public void Land()
	{
		if (AnimationPlayed(AnimationString.ENEMY_LANDING, 0.48f))
		{
			PlaySoundSingle("Audio/enemy/Dragon/long_luodi");
		}
		else if (dragonSmallFlyAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Dragon/long_feixing");
			dragonSmallFlyAudioTimer.Do();
		}
	}

	public void DisableAllEffect()
	{
		EnableTrailEffect(false);
		EnableMouthFire(false);
		EnableLeftArmTrail(false);
		EnableRightArmTrail(false);
	}

	public void EnableMouthFire(bool bEnable)
	{
		if (bEnable)
		{
			mouthFire.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = true;
			mouthFire.transform.GetChild(1).GetComponent<ParticleEmitter>().emit = true;
		}
		else
		{
			mouthFire.transform.GetChild(0).GetComponent<ParticleEmitter>().emit = false;
			mouthFire.transform.GetChild(1).GetComponent<ParticleEmitter>().emit = false;
		}
	}

	public void StartFlyShot()
	{
		base.CanShot = true;
		animation[AnimationString.ENEMY_FLY_ATTACK].time = 0f;
	}

	public void FlyShot()
	{
		GameObject original = Resources.Load("Effect/Dragonball") as GameObject;
		Vector3 position = enemyTransform.Find(BoneName.DragonMouth).position;
		GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		Vector3 vector = new Vector3(targetToLookAt.x, (float)Global.FLOORHEIGHT + 0.1f, targetToLookAt.z);
		EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
		component.speed = (vector - position).normalized * flyShotSpeed;
		component.attackDamage = flyShotDamage;
		component.areaDamage = flyShotExplodeDamage;
		component.damageType = DamageType.Sputtering;
		component.trType = TrajectoryType.Straight;
		component.enemyType = enemyType;
		component.explodeEffect = "Effect/RPG_bom";
		component.explodeRadius = flyShotExplodeRadius;
		PlaySoundSingle("Audio/enemy/Dragon/long_fly_attack");
	}

	public void StartShot()
	{
		base.CanShot = true;
		animation[AnimationString.ENEMY_ATTACK].time = 0f;
	}

	public void Shot()
	{
		GameObject original = Resources.Load("Effect/Dragonball") as GameObject;
		Vector3 position = enemyTransform.Find(BoneName.DragonMouth).position;
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
			gameObject.transform.rotation = Quaternion.AngleAxis(45f - 22.5f * (float)i, enemyTransform.up) * enemyTransform.rotation;
			EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
			component.speed = gameObject.transform.forward * shotSpeed;
			component.attackDamage = shotDamage;
			component.areaDamage = shotExplodeDamage;
			component.damageType = DamageType.Sputtering;
			component.trType = TrajectoryType.Straight;
			component.enemyType = enemyType;
			component.explodeEffect = "Effect/RPG_bom";
			component.explodeRadius = shotExplodeRadius;
		}
		PlaySoundSingle("Audio/enemy/Dragon/long_tuxi_1");
	}

	private bool isPlayerInFlame()
	{
		float num = animation[AnimationString.ENEMY_ATTACK02].time / animation[AnimationString.ENEMY_ATTACK02].clip.length;
		float playerHorizontalAngle = GetPlayerHorizontalAngle();
		float horizontalSqrDistanceFromPlayer = GetHorizontalSqrDistanceFromPlayer();
		for (int i = 0; i < 5; i++)
		{
			if (num > flameCheckers[i].startTime && num < flameCheckers[i].endTime && playerHorizontalAngle > flameCheckers[i].startAngle && playerHorizontalAngle < flameCheckers[i].endAngle && horizontalSqrDistanceFromPlayer < flameCheckers[i].distanceSqr)
			{
				return true;
			}
		}
		return false;
	}

	public void OnRage()
	{
		float num = animation[AnimationString.ENEMY_RAGE].time / animation[AnimationString.ENEMY_RAGE].clip.length;
		if ((double)num > 0.24 && (double)num < 0.74)
		{
			EnableMouthFire(true);
			PlaySoundSingle("Audio/enemy/Dragon/long_housheng_1");
		}
		else
		{
			EnableMouthFire(false);
		}
	}

	public void CheckFlame()
	{
		float num = animation[AnimationString.ENEMY_ATTACK02].time / animation[AnimationString.ENEMY_ATTACK02].clip.length;
		if (num < 0.4f || num > 0.86f)
		{
			EnableMouthFire(false);
			return;
		}
		EnableMouthFire(true);
		if (firetimer.Ready() && isPlayerInFlame())
		{
			player.OnHit(flameDamage);
			firetimer.Do();
		}
		PlaySoundSingle("Audio/enemy/Dragon/long_tuxi_2");
	}

	public void FlyCheckFlame()
	{
		float num = animation[AnimationString.ENEMY_FLY_ATTACK01].time / animation[AnimationString.ENEMY_FLY_ATTACK01].clip.length;
		if (num < 0.3f || num > 0.85f)
		{
			EnableMouthFire(false);
			LookAtTarget();
			return;
		}
		EnableMouthFire(true);
		if (firetimer.Ready() && flyMouthFireCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(flameDamage);
			firetimer.Do();
		}
		PlaySoundSingle("Audio/enemy/Dragon/long_tuxi_1");
	}

	public override void CheckHit()
	{
		if (firetimer.Ready() && headCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(attackDamage);
			CheckKnocked(attackKnockSpeed);
			firetimer.Do();
		}
	}

	public void CheckPuHit()
	{
		if (firetimer.Ready() && headCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(puDamage);
			CheckKnocked(puKnockSpeed);
			firetimer.Do();
		}
	}

	public void CheckRushHit()
	{
		if (firetimer.Ready() && headCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(rushDamage);
			CheckKnocked(rushKnockSpeed);
			firetimer.Do();
		}
	}

	public override bool CanTurn()
	{
		return state != Enemy.DEAD_STATE && state != Enemy.IDLE_STATE;
	}

	public override bool NeedMoveDown()
	{
		return enemyTransform.position.y - (float)Global.FLOORHEIGHT > 0.1f;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		trails[0].transform.position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z) + enemyTransform.forward * 10f;
		if (windTimer.Ready() && (state == FLY_IDLE_STATE || state == FLY_RUSH_END_STATE || state == FLY_SHOT_STATE || state == FLY_FLAME_STATE || state == FLY_STATE || state == START_FLY_STATE || state == START_RUSH_STATE || state == LANDING_STATE))
		{
			GameObject original = Resources.Load("Effect/Dragon/dragon_wind") as GameObject;
			Vector3 position = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
			GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
			windTimer.Do();
		}
		if (dragonBigFlyAudioTimer.Ready() && (state == FLY_IDLE_STATE || state == FLY_SHOT_STATE || state == FLY_FLAME_STATE || state == FLY_STATE || state == START_RUSH_STATE))
		{
			PlaySound("Audio/enemy/Dragon/long_fly_sprint2");
			dragonBigFlyAudioTimer.Do();
		}
	}

	public void OnFlame()
	{
		PlayAnimation(AnimationString.ENEMY_ATTACK02, WrapMode.ClampForever);
	}

	public void FlyOnFlame()
	{
		PlayAnimation(AnimationString.ENEMY_FLY_ATTACK01, WrapMode.ClampForever);
	}

	public void StartNormalAttack()
	{
		Vector3 vector = target.position - enemyTransform.position;
		vector.y = 0f;
		attackSpeed = vector.magnitude * 3f;
		attackSpeed = Mathf.Min(attackSpeed, maxAttackSpeed);
	}

	public void NormalAttack()
	{
		if (AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.36f) && !AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.47f))
		{
			enemyTransform.Translate(enemyTransform.forward * attackSpeed * Time.deltaTime, Space.World);
			PlaySoundSingle("Audio/enemy/Dragon/long_luodi");
		}
	}

	public void StartPu()
	{
		Vector3 vector = target.position - enemyTransform.position;
		vector.y = 0f;
		puSpeed = vector.magnitude;
		puDeceleration = puSpeed / 30f;
		if (GetTargetHorizontalAngle() > 0f)
		{
			puAnimation = AnimationString.ENEMY_PU;
		}
		else
		{
			puAnimation = AnimationString.ENEMY_PU01;
		}
	}

	public void StartFly()
	{
		if (dragonBigFlyAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Dragon/long_fly_sprint2");
			dragonBigFlyAudioTimer.Do();
		}
		FlyUp();
	}

	public void FlyUp()
	{
		if (enemyTransform.position.y < 5f)
		{
			enemyTransform.Translate(Vector3.up * upSpeed * 5f * Time.deltaTime);
		}
		else
		{
			enemyTransform.position = new Vector3(enemyTransform.position.x, 5f, enemyTransform.position.z);
		}
	}

	public byte ChangeStateInFlyFar()
	{
		int num = Random.Range(0, 100);
		byte b = 0;
		if (num < Probability1)
		{
			SetState(FLY_STATE);
			SetCatchingTimeNow();
			b = 47;
		}
		else
		{
			SetState(FLY_SHOT_STATE);
			b = 48;
			StartFlyShot();
		}
		return b;
	}

	public byte ChangeStateInFlyNear()
	{
		byte b = 0;
		int nearestPlayer = GetNearestPlayer();
		ChangeTargetPlayer(nearestPlayer);
		float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
		if (nearestDistanceToTargetPlayer < groupAttackDistance)
		{
			SetState(FLY_SHOT_STATE);
			b = 48;
			StartFlyShot();
		}
		else
		{
			int num = Random.Range(0, 100);
			if (num < Probability6)
			{
				SetState(FLY_FLAME_STATE);
				b = 55;
			}
			else
			{
				SetState(FLY_SHOT_STATE);
				b = 48;
				StartFlyShot();
			}
		}
		return b;
	}

	public override void OnIdle()
	{
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		byte b = 0;
		int targetID = GetTargetPlayer().GetUserID();
		if (GetLandingTimeDuration() < MaxGroundTime)
		{
			if (GetGroundIdleDuration() > MaxGroundIdleTime)
			{
				float averagehorizontalDistance = GetAveragehorizontalDistance();
				if (averagehorizontalDistance < base.AttackRange)
				{
					targetID = GetNearestPlayer();
					ChangeTargetPlayer(targetID);
					float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
					if (nearestDistanceToTargetPlayer < groupAttackDistance)
					{
						SetState(FLAME_STATE);
						b = 54;
					}
					else
					{
						int num = Random.Range(0, 100);
						if (num < Probability5)
						{
							if (IsEasyToCollideWall())
							{
								return;
							}
							SetState(NORMAL_ATTACK_STATE);
							b = 56;
							StartNormalAttack();
						}
						else
						{
							SetState(FLAME_STATE);
							b = 54;
						}
					}
				}
				else if (averagehorizontalDistance < startRushDistance)
				{
					int num2 = Random.Range(0, 100);
					if (num2 < Probability2)
					{
						SetState(Enemy.CATCHING_STATE);
						SetCatchingTimeNow();
						b = 46;
						targetID = GetNearestPlayer();
					}
					else if (num2 < Probability3)
					{
						if (IsEasyToCollideWall())
						{
							return;
						}
						SetState(PU_STATE);
						b = 53;
						targetID = GetRandomPlayer();
						ChangeTargetPlayer(targetID);
						StartPu();
					}
					else if (num2 < Probability4)
					{
						SetState(FLAME_STATE);
						b = 54;
						targetID = GetRandomPlayer();
					}
					else
					{
						SetState(SHOT_STATE);
						b = 51;
						targetID = GetRandomPlayer();
						StartShot();
					}
				}
				else
				{
					SetState(START_RUSH_STATE);
					b = 50;
					targetID = GetFarthestPlayer();
					EnableGravity(false);
				}
			}
		}
		else
		{
			SetState(START_FLY_STATE);
			SetFlyTimeNow();
			EnableGravity(false);
			b = 45;
			targetID = GetFarthestPlayer();
		}
		if (b != 0)
		{
			ChangeTargetPlayer(targetID);
			LookAtTarget();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, targetID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public bool CloseToDiveTarget()
	{
		Vector3 vector = DiveTargetPos - enemyTransform.position;
		vector.y = 0f;
		if (vector.sqrMagnitude < stopDiveDistance * stopDiveDistance)
		{
			return true;
		}
		return false;
	}

	public bool CloseToRushTarget()
	{
		Vector3 vector = RushTargetPos - enemyTransform.position;
		vector.y = 0f;
		if (vector.sqrMagnitude < stopRushDistance * stopRushDistance)
		{
			return true;
		}
		return false;
	}

	public override bool isFlying()
	{
		return GetLandingTimeDuration() > GetFlyTimeDuration();
	}

	protected override void StopSoundOnHit()
	{
		StopSound("long_fly_dive");
		StopSound("long_housheng_1");
		StopSound("long_tuxi_1");
		StopSound("long_tuxi_2");
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.DEAD_STATE)
		{
			hp -= dp.damage;
			Vector3 hitPoint = Vector3.zero;
			if (GetHitPoint(out hitPoint))
			{
				GameObject original = Resources.Load("Effect/Dragon/dragon_blood") as GameObject;
				Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if ((float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(DRAGON_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Dragon/long_attacked");
				GameObject original2 = Resources.Load("Effect/Dragon/dragon_dead_blood") as GameObject;
				Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
			}
		}
	}

	public override void OnHitResponse()
	{
		if (state != Enemy.DEAD_STATE)
		{
			Vector3 hitPoint = Vector3.zero;
			if (GetHitResponsePoint(out hitPoint))
			{
				GameObject original = Resources.Load("Effect/Dragon/dragon_blood") as GameObject;
				Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if ((float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(DRAGON_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Dragon/long_attacked");
				GameObject original2 = Resources.Load("Effect/Dragon/dragon_dead_blood") as GameObject;
				Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
			}
		}
	}

	public override EnemyState EnterSpecialState(float deltaTime)
	{
		if (GetCatchingration() > base.MaxCatchingTime)
		{
			return Enemy.IDLE_STATE;
		}
		EnableMouthFire(false);
		EnemyState result = null;
		byte b = 0;
		if (GetHorizontalSqrDistanceFromTarget() < base.AttackRange * base.AttackRange)
		{
			float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
			if (nearestDistanceToTargetPlayer < groupAttackDistance)
			{
				SetState(FLAME_STATE);
				b = 54;
			}
			else
			{
				int num = Random.Range(0, 100);
				if (num < Probability5 && !IsEasyToCollideWall())
				{
					SetState(NORMAL_ATTACK_STATE);
					b = 56;
					StartNormalAttack();
				}
				else
				{
					SetState(FLAME_STATE);
					b = 54;
				}
			}
			LookAtTarget();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		return result;
	}
}
