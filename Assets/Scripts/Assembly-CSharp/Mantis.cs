using UnityEngine;

public class Mantis : EnemyBoss
{
	protected const float FLY_HEIGHT = 0.5f;

	public static EnemyState START_FLY_STATE = new MantisStartFlyState();

	public static EnemyState FLY_IDLE_STATE = new MantisFlyIdleState();

	public static EnemyState FLY_STATE = new MantisFlyState();

	public static EnemyState FLY_DIVESTART_STATE = new MantisFlyStartDiveState();

	public static EnemyState FLY_DIVE_STATE = new MantisFlyDiveState();

	public static EnemyState FLY_DIVEEND_STATE = new MantisFlyDiveEndState();

	public static EnemyState FLY_ATTACK_STATE = new MantisFlyAttackState();

	public static EnemyState FLY_BOOMERANG_STATE = new MantisFlyBoomerangState();

	public static EnemyState FLY_LASER_STATE = new MantisFlyLaserState();

	public static EnemyState FLY_SHOT_STATE = new MantisFlyShotState();

	public static EnemyState LANDING_STATE = new MantisLandingState();

	public static EnemyState CRITICAL_ATTACK_STATE = new MantisCriticalAttackState();

	public static EnemyState NORMAL_ATTACK_STATE = new MantisNormalAttackState();

	public static EnemyState MANTIS_GOTHIT_STATE = new MantisGotHitState();

	public static EnemyState RAGE_STATE = new MantisRageState();

	protected GameObject mantisLaserObj;

	protected GameObject mantisLaserFireObj;

	protected Collider leftArmCollider;

	protected Collider rightArmCollider;

	protected float lastFlyTime;

	protected float lastFlyIdleTime;

	protected float lastGroundIdleTime;

	protected float lastLandingTime;

	protected Timer mantisflyAudioTimer = new Timer();

	protected bool needLandingToRage;

	protected float flyAttackRange;

	protected float startDiveDistance;

	protected float stopDiveDistance;

	protected float[] maxFlyTime = new float[2];

	protected float[] maxGroundTime = new float[2];

	protected float[] maxFlyIdleTime = new float[2];

	protected float[] maxGroundIdleTime = new float[2];

	protected float flySpeed;

	protected float diveSpeed;

	protected float hookSpeed;

	protected float hookAngle;

	protected new float attackDetectionAngle;

	protected int boomerangDamage;

	protected int laserDamage;

	protected float touchDamageDistance;

	protected float attackDamageDistance;

	protected float flyAttackDamageDistance;

	protected float rushDamageDistance;

	protected float attackKnockSpeed;

	protected float diveKnockSpeed;

	protected float boomerangHorizontalAngleL;

	protected float boomerangVerticalAngleL;

	protected float boomerangRisingSpeedL;

	protected float boomerangRisingTimeL;

	protected float boomerangAttackSpeedL;

	protected float boomerangAngularVelocityL;

	protected float boomerangHorizontalAngleR;

	protected float boomerangVerticalAngleR;

	protected float boomerangRisingSpeedR;

	protected float boomerangRisingTimeR;

	protected float boomerangAttackSpeedR;

	protected float boomerangAngularVelocityR;

	protected int[] maxShotTimes = new int[2];

	protected int[] probability1 = new int[2];

	protected int[] probability2 = new int[2];

	protected int[] probability3 = new int[2];

	protected int[] probability4 = new int[2];

	protected int[] probability5 = new int[2];

	public Vector3 DiveTargetPos { get; set; }

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

	public float StartDiveDistance
	{
		get
		{
			return startDiveDistance;
		}
	}

	public float FlyAttackRange
	{
		get
		{
			return flyAttackRange;
		}
	}

	public float MaxFlyTime
	{
		get
		{
			return maxFlyTime[(int)mindState];
		}
	}

	public float MaxGroundTime
	{
		get
		{
			return maxGroundTime[(int)mindState];
		}
	}

	public float MaxFlyIdleTime
	{
		get
		{
			return maxFlyIdleTime[(int)mindState];
		}
	}

	public float MaxGroundIdleTime
	{
		get
		{
			return maxGroundIdleTime[(int)mindState];
		}
	}

	public int MaxShotTimes
	{
		get
		{
			return maxShotTimes[(int)mindState];
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

	public Mantis()
	{
		EnemyBoss.INIT_STATE = new MantisInitState();
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
		GameObject original = Resources.Load("Effect/Mantis/mantis_dead_blood") as GameObject;
		Object.Instantiate(original, bipObject.transform.position, Quaternion.identity);
		StopSoundOnHit();
		PlaySoundSingle("Audio/enemy/Mantis/tanglang_dead");
		if (isFlying())
		{
			PlayAnimation(AnimationString.MANTIS_FLY_DEAD, WrapMode.ClampForever, 0.5f);
			if (AnimationPlayed(AnimationString.MANTIS_FLY_DEAD, 1f))
			{
				EnableGravity(true);
			}
		}
		else
		{
			PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever, 0.5f);
			EnableGravity(true);
		}
		DisableAllEffect();
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(5f, false);
		GameApp.GetInstance().GetUserState().Achievement.KillBoss(0);
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
		areaCenter = new Vector3(17.5f, 0f, 2f);
		areaRadius = 19f;
		maxOverRushDistance = 10f;
		hp = 36000;
		hpPercentagePerHit = 0.15f;
		hitTimesForRage = 4;
		stopDiveDistance = 6f;
		attackRange = 8f;
		flyAttackRange = 6f;
		startDiveDistance = 26f;
		groupAttackDistance = 12f;
		maxFlyIdleTime[0] = 1f;
		maxGroundIdleTime[0] = 1f;
		maxFlyTime[0] = 18f;
		maxGroundTime[0] = 8f;
		maxCatchingTime[0] = 5f;
		maxFlyIdleTime[1] = 0.4f;
		maxGroundIdleTime[1] = 0.4f;
		maxFlyTime[1] = 25f;
		maxGroundTime[1] = 10f;
		maxCatchingTime[1] = 5f;
		runSpeed = 12f;
		flySpeed = 14f;
		diveSpeed = 28f;
		hookSpeed = 22f;
		turnSpeed = 0.06f;
		downSpeed = 1f;
		hookAngle = 22f;
		attackDetectionAngle = 24f;
		touchDamage = 200;
		attackDamage = 1000;
		rushDamage = 1400;
		shotDamage = 800;
		boomerangDamage = 800;
		laserDamage = 1111;
		touchDamageDistance = 4.5f;
		attackDamageDistance = 11.5f;
		flyAttackDamageDistance = 10.5f;
		rushDamageDistance = 9.5f;
		touchKnockSpeed = 0.08f;
		attackKnockSpeed = 0.16f;
		diveKnockSpeed = 0.22f;
		boomerangHorizontalAngleL = 55f;
		boomerangVerticalAngleL = 25f;
		boomerangRisingSpeedL = 9f;
		boomerangRisingTimeL = 1.2f;
		boomerangAttackSpeedL = 24f;
		boomerangAngularVelocityL = 7f;
		boomerangHorizontalAngleR = 40f;
		boomerangVerticalAngleR = 30f;
		boomerangRisingSpeedR = 11f;
		boomerangRisingTimeR = 1f;
		boomerangAttackSpeedR = 30f;
		boomerangAngularVelocityR = 7f;
		maxShotTimes[0] = 1;
		maxShotTimes[1] = 3;
		probability1[0] = 30;
		probability2[0] = 70;
		probability3[0] = 80;
		probability1[1] = 20;
		probability2[1] = 60;
		probability3[1] = 70;
		probability4[0] = 70;
		probability4[1] = 50;
		probability5[0] = 80;
		probability5[1] = 75;
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		Transform mouthTransform = enemyTransform.Find(BoneName.MantisMouth);
		GameObject original = Resources.Load("Effect/Mantis/sfx_laser") as GameObject;
		mantisLaserObj = Object.Instantiate(original, Vector3.zero, Quaternion.identity) as GameObject;
		MantisLaserScript component = mantisLaserObj.GetComponent<MantisLaserScript>();
		component.mouthTransform = mouthTransform;
		GameObject original2 = Resources.Load("Effect/Mantis/sfx_laserFire") as GameObject;
		mantisLaserFireObj = Object.Instantiate(original2, Vector3.zero, Quaternion.identity) as GameObject;
		MantisLaserFireScript component2 = mantisLaserFireObj.GetComponent<MantisLaserFireScript>();
		component2.mouthTransform = mouthTransform;
		bipObject = enemyObject.transform.Find(BoneName.MantisBip01).gameObject;
		bodyCollider = bipObject.GetComponent<Collider>();
		flyBodyCollider = enemyObject.transform.Find(BoneName.MantisPelvis).gameObject.GetComponent<Collider>();
		leftArmCollider = enemyObject.transform.Find(BoneName.MantisLeftArm).gameObject.GetComponent<Collider>();
		rightArmCollider = enemyObject.transform.Find(BoneName.MantisRightArm).gameObject.GetComponent<Collider>();
		trails = new GameObject[1];
		GameObject original3 = Resources.Load("Effect/Dragon/dragon_trail") as GameObject;
		Vector3 position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
		trails[0] = Object.Instantiate(original3, position, Quaternion.identity) as GameObject;
		leftArmTrail = enemyObject.transform.Find(BoneName.MantisLeftArm).Find("Trail").gameObject;
		rightArmTrail = enemyObject.transform.Find(BoneName.MantisRightArm).Find("Trail").gameObject;
		mantisflyAudioTimer.SetTimer(0.3f, false);
		walkAudioTimer.SetTimer(0.4f, false);
		touchtimer.SetTimer(2f, false);
		firetimer.SetTimer(2f, false);
		criticalTimer.SetTimer(2f, false);
		walkAudioName = "Audio/enemy/Mantis/tanglang_run";
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
		if (enemyTransform.position.y - (float)Global.FLOORHEIGHT < -1.5f)
		{
			enemyTransform.position = new Vector3(enemyTransform.position.x, Global.FLOORHEIGHT, enemyTransform.position.z);
			return true;
		}
		return false;
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

	public override bool isFlying()
	{
		return GetLandingTimeDuration() > GetFlyTimeDuration();
	}

	public void FlySound()
	{
		PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fly_idle");
		FlyUp();
	}

	public virtual void FlyIdle()
	{
		FlySound();
		if ((!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle()) || isAllPlayerDead())
		{
			return;
		}
		byte b = 0;
		int targetID = GetTargetPlayer().GetUserID();
		if (GetFlyTimeDuration() < MaxFlyTime)
		{
			if (GetFlyIdleDuration() > MaxFlyIdleTime)
			{
				float averagehorizontalDistance = GetAveragehorizontalDistance();
				if (averagehorizontalDistance < FlyAttackRange)
				{
					b = ChangeStateInFlyNear();
					targetID = GetNearestPlayer();
				}
				else if (averagehorizontalDistance < StartDiveDistance)
				{
					b = ChangeStateInFlyFar();
					targetID = ((b != 15) ? GetRandomPlayer() : GetNearestPlayer());
				}
				else
				{
					int num = Random.Range(0, 100);
					if (num < Probability3)
					{
						SetState(FLY_BOOMERANG_STATE);
						b = 19;
						targetID = GetRandomPlayer();
						StartFlyBoomerang();
					}
					else
					{
						SetState(FLY_DIVESTART_STATE);
						b = 21;
						targetID = GetFarthestPlayer();
					}
				}
			}
		}
		else
		{
			SetState(LANDING_STATE);
			b = 18;
			targetID = GetNearestPlayer();
		}
		if (b != 0)
		{
			ChangeTargetPlayer(targetID);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, targetID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void OnDive()
	{
		maxTurnRadian *= 5f;
		DiveTargetPos = targetToLookAt;
		dir = DiveTargetPos - enemyTransform.position;
		dir.y = 0f;
		dir.Normalize();
		Vector3 vector = DiveTargetPos - areaCenter;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (magnitude < areaRadius)
		{
			float a = (areaRadius - magnitude) * 0.5f;
			a = Mathf.Min(a, maxOverRushDistance);
			DiveTargetPos += a * dir;
		}
		DiveTargetPos = new Vector3(DiveTargetPos.x, target.position.y, DiveTargetPos.z);
		dir = (DiveTargetPos - enemyTransform.position).normalized;
		EnableTrailEffect(true);
	}

	public void Dive()
	{
		enemyTransform.Translate(dir * diveSpeed * Time.deltaTime, Space.World);
	}

	public void StartFlyShot()
	{
		base.CanShot = true;
		animation[AnimationString.MANTIS_SHOT].time = 0f;
	}

	public void StartFlyBoomerang()
	{
		base.CanShot = true;
		animation[AnimationString.MANTIS_BOOMERANG].time = 0f;
	}

	public void FlyShot()
	{
		GameObject original = Resources.Load("Effect/Mantis/sfx_blade") as GameObject;
		Vector3 position = enemyTransform.Find(BoneName.MantisLeftArm).position;
		position.y -= 1f;
		GameObject[] array = new GameObject[3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
			array[i].transform.LookAt(new Vector3(targetToLookAt.x, 0f, targetToLookAt.z));
			array[i].transform.RotateAround(array[i].transform.position, Vector3.up, (float)(1 - i) * hookAngle);
			MantisHookScript component = array[i].GetComponent<MantisHookScript>();
			Vector3 forward = array[i].transform.forward;
			forward.y *= 2f;
			forward.Normalize();
			component.speed = forward * hookSpeed;
			component.attackDamage = shotDamage;
		}
		PlaySoundSingle("Audio/enemy/Mantis/tanglang_fly_attack01");
	}

	public void FlyBoomerang()
	{
		GameObject original = Resources.Load("Effect/Mantis/sfx_boomerang") as GameObject;
		Vector3 position = enemyTransform.Find(BoneName.MantisLeftArm).position;
		GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		gameObject.transform.LookAt(targetToLookAt);
		gameObject.transform.RotateAround(gameObject.transform.position, gameObject.transform.right, 0f - boomerangVerticalAngleL);
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, boomerangHorizontalAngleL);
		RotateScript component = gameObject.GetComponent<RotateScript>();
		component.rotationSpeed.y = -4000f;
		MantisBoomerangScript component2 = gameObject.GetComponent<MantisBoomerangScript>();
		Vector3 forward = gameObject.transform.forward;
		forward.Normalize();
		component2.risingSpeed = forward * boomerangRisingSpeedL;
		component2.attackDamage = boomerangDamage;
		component2.target = target;
		component2.attackSpeedValue = boomerangAttackSpeedL;
		component2.angularVelocity = boomerangAngularVelocityL;
		component2.risingTime = boomerangRisingTimeL;
		Vector3 position2 = enemyTransform.Find(BoneName.MantisRightArm).position;
		GameObject gameObject2 = Object.Instantiate(original, position2, Quaternion.identity) as GameObject;
		gameObject2.transform.LookAt(target.position);
		gameObject2.transform.RotateAround(gameObject2.transform.position, gameObject2.transform.right, 0f - boomerangVerticalAngleR);
		gameObject2.transform.RotateAround(gameObject2.transform.position, Vector3.up, 0f - boomerangHorizontalAngleR);
		RotateScript component3 = gameObject2.GetComponent<RotateScript>();
		component3.rotationSpeed.y = 4000f;
		MantisBoomerangScript component4 = gameObject2.GetComponent<MantisBoomerangScript>();
		Vector3 forward2 = gameObject2.transform.forward;
		forward2.Normalize();
		component4.risingSpeed = forward2 * boomerangRisingSpeedR;
		component4.attackDamage = boomerangDamage;
		component4.target = target;
		component4.attackSpeedValue = boomerangAttackSpeedR;
		component4.angularVelocity = boomerangAngularVelocityR;
		component4.risingTime = boomerangRisingTimeR;
		PlaySoundSingle("Audio/enemy/Mantis/tanglang_fly_attack02");
	}

	public void FlyLaserStart()
	{
		Transform transform = enemyTransform.Find(BoneName.MantisMouth);
		Vector3 worldPosition = new Vector3(targetToLookAt.x, 0.5f, targetToLookAt.z);
		mantisLaserObj.transform.position = transform.position;
		mantisLaserObj.transform.Translate(0.1f * Vector3.down);
		mantisLaserObj.transform.LookAt(worldPosition);
		mantisLaserObj.transform.RotateAround(transform.transform.position, Vector3.up, 40f);
		mantisLaserObj.SetActiveRecursively(true);
		MantisLaserScript component = mantisLaserObj.GetComponent<MantisLaserScript>();
		component.attackDamage = laserDamage;
		component.speed = -60f;
	}

	public void FlyLaserStop()
	{
		mantisLaserObj.SetActiveRecursively(false);
	}

	public void LaserFireStart()
	{
		base.CanShot = true;
		mantisLaserFireObj.SetActiveRecursively(true);
		PlaySound("Audio/enemy/Mantis/tanglang_jiguang");
		animation[AnimationString.MANTIS_LASER].time = 0f;
		animation[AnimationString.MANTIS_LASER_180].time = 0f;
	}

	public void LaserFireStop()
	{
		mantisLaserFireObj.SetActiveRecursively(false);
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
		PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fly_walk");
	}

	public void Land()
	{
		EnableGravity(true);
		if (AnimationPlayed(AnimationString.ENEMY_LANDING, 0.4f))
		{
			PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_luodi");
		}
		else
		{
			PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fly_idle");
		}
	}

	public void DisableAllEffect()
	{
		FlyLaserStop();
		LaserFireStop();
		EnableLeftArmTrail(false);
		EnableRightArmTrail(false);
		EnableTrailEffect(false);
	}

	public void NormalAttack()
	{
		float num = animation[AnimationString.ENEMY_ATTACK].time / animation[AnimationString.ENEMY_ATTACK].clip.length;
		if (num > 0.43f && num < 0.71f)
		{
			EnableRightArmTrail(true);
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attack_01");
		}
		else
		{
			EnableRightArmTrail(false);
		}
	}

	public void CriticalAttack()
	{
		float num = animation[AnimationString.MANTIS_CRITICAL_ATTACK].time / animation[AnimationString.MANTIS_CRITICAL_ATTACK].clip.length;
		if (num > 0.25f && num < 0.43f)
		{
			EnableRightArmTrail(true);
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attack_01");
		}
		else
		{
			EnableRightArmTrail(false);
		}
		if (num > 0.57f && num < 0.74f)
		{
			EnableLeftArmTrail(true);
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attack_01");
		}
		else
		{
			EnableLeftArmTrail(false);
		}
	}

	public void FlyAttack()
	{
		float num = animation[AnimationString.MANTIS_SHOT].time / animation[AnimationString.MANTIS_SHOT].clip.length;
		if (num > 0.3f && num < 0.6f)
		{
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attack_01");
		}
		if (num > 0.46f && num < 0.7f)
		{
			EnableLeftArmTrail(true);
			EnableRightArmTrail(true);
		}
		else
		{
			EnableLeftArmTrail(false);
			EnableRightArmTrail(false);
		}
	}

	public override void CheckHit()
	{
		if (firetimer.Ready() && (leftArmCollider.bounds.Intersects(player.GetCollider().bounds) || rightArmCollider.bounds.Intersects(player.GetCollider().bounds)))
		{
			player.OnHit(attackDamage);
			firetimer.Do();
			if (state != FLY_ATTACK_STATE)
			{
				CheckKnocked(attackKnockSpeed);
			}
		}
	}

	public void CheckDiveHit()
	{
		if (firetimer.Ready() && (leftArmCollider.bounds.Intersects(player.GetCollider().bounds) || rightArmCollider.bounds.Intersects(player.GetCollider().bounds)))
		{
			player.OnHit(rushDamage);
			CheckKnocked(diveKnockSpeed);
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
	}

	public void StartFly()
	{
		if (mantisflyAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Mantis/feixingtanglang_qifei");
			mantisflyAudioTimer.Do();
		}
		FlyUp();
	}

	public void FlyUp()
	{
		if (enemyTransform.position.y < 0.5f)
		{
			enemyTransform.Translate(Vector3.up * 2f * 0.5f * Time.deltaTime);
		}
		else
		{
			enemyTransform.position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
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
			b = 15;
		}
		else if (num < Probability2)
		{
			SetState(FLY_LASER_STATE);
			b = 20;
			LaserFireStart();
		}
		else
		{
			SetState(FLY_SHOT_STATE);
			b = 16;
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
			SetState(FLY_LASER_STATE);
			LaserFireStart();
			return 20;
		}
		int num = Random.Range(0, 100);
		if (num < Probability5)
		{
			SetState(FLY_ATTACK_STATE);
			return 17;
		}
		SetState(FLY_LASER_STATE);
		LaserFireStart();
		return 20;
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
						SetState(CRITICAL_ATTACK_STATE);
						b = 12;
					}
					else
					{
						int num = Random.Range(0, 100);
						if (num < Probability4)
						{
							SetState(NORMAL_ATTACK_STATE);
							b = 11;
						}
						else
						{
							SetState(CRITICAL_ATTACK_STATE);
							b = 12;
						}
					}
				}
				else if (averagehorizontalDistance < startDiveDistance)
				{
					SetState(Enemy.CATCHING_STATE);
					SetCatchingTimeNow();
					b = 10;
					targetID = GetNearestPlayer();
				}
				else
				{
					SetState(START_FLY_STATE);
					b = 13;
					targetID = GetFarthestPlayer();
				}
			}
		}
		else
		{
			SetState(START_FLY_STATE);
			b = 13;
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

	protected override void StopSoundOnHit()
	{
		StopSound("tanglang_jiguang");
		StopSound("feixingtanglang_fuchong2");
		StopSound("feixingtanglang_gongjiqian");
	}

	protected virtual void ChangeStateAfterHit()
	{
	}

	public virtual void OnRage()
	{
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.DEAD_STATE)
		{
			hp -= dp.damage;
			Vector3 hitPoint = Vector3.zero;
			if (GetHitPoint(out hitPoint))
			{
				GameObject original = Resources.Load("Effect/Mantis/mantis_blood") as GameObject;
				Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if ((float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(MANTIS_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Mantis/tanglang_attacked");
				GameObject original2 = Resources.Load("Effect/Mantis/mantis_dead_blood") as GameObject;
				Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
				ChangeStateAfterHit();
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
				GameObject original = Resources.Load("Effect/Mantis/mantis_blood") as GameObject;
				Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if ((float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(MANTIS_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Mantis/tanglang_attacked");
				GameObject original2 = Resources.Load("Effect/Mantis/mantis_dead_blood") as GameObject;
				Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
				ChangeStateAfterHit();
			}
		}
	}

	public override EnemyState EnterSpecialState(float deltaTime)
	{
		if (GetCatchingration() > base.MaxCatchingTime)
		{
			return Enemy.IDLE_STATE;
		}
		EnemyState result = null;
		byte b = 0;
		if (GetHorizontalSqrDistanceFromTarget() < base.AttackRange * base.AttackRange)
		{
			float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
			if (nearestDistanceToTargetPlayer < groupAttackDistance)
			{
				result = CRITICAL_ATTACK_STATE;
				b = 12;
			}
			else
			{
				int num = Random.Range(0, 100);
				if (num < Probability4)
				{
					SetState(NORMAL_ATTACK_STATE);
					b = 11;
				}
				else
				{
					SetState(CRITICAL_ATTACK_STATE);
					b = 12;
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
