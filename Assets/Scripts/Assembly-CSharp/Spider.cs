using UnityEngine;

public class Spider : EnemyBoss
{
	public static EnemyState START_JUMP_STATE = new SpiderStartJumpState();

	public static EnemyState JUMP_STATE = new SpiderJumpState();

	public static EnemyState LANDING_STATE = new SpiderLandingState();

	public static EnemyState RUSH_STATE = new SpiderRushingState();

	public static EnemyState SHOT_STATE = new SpiderShotState();

	public static EnemyState CONTINUOUS_SHOT_STATE = new SpiderContinuousShotState();

	public static EnemyState DOUBLE_ATTACK_STATE = new SpiderDoubleAttackState();

	public static EnemyState NORMAL_ATTACK_STATE = new SpiderNormalAttackState();

	public static EnemyState RAGE_STATE = new SpiderRageState();

	public static EnemyState SPIDER_GOTHIT_STATE = new SpiderGotHitState();

	public static EnemyState START_RUSH_STATE = new SpiderStartRushState();

	protected Collider leftArmCollider;

	protected Collider rightArmCollider;

	protected Transform leftHandTransform;

	protected Transform rightHandTransform;

	protected AlphaAnimationScript eyeAnimationScript;

	public Vector3 jumpSpeed = default(Vector3);

	protected float lastIdleTime;

	protected float lastJumpIdleTime;

	protected Timer spiderRunAudioTimer = new Timer();

	protected Timer spiderRightArmAudioTimer = new Timer();

	protected Timer spiderLeftArmAudioTimer = new Timer();

	protected Timer spiderLeftArmGroundTimer = new Timer();

	protected Timer spiderRightArmGroundTimer = new Timer();

	protected bool canJump;

	protected bool hasLanded;

	protected bool isContinuousShot;

	protected Timer slowShotTimer = new Timer();

	protected Timer fastShotTimer = new Timer();

	protected Timer slimeTimer = new Timer();

	protected float stopRushDistance;

	protected float startRushDistance;

	protected float[] maxIdleTime = new float[2];

	protected float rushSpeed;

	protected float spitSpeed;

	protected new float attackDetectionAngle;

	protected float shotHorizontalRange;

	protected float shotVerticalRange;

	protected float minScale;

	protected float maxScale;

	protected float normalScale;

	protected int spitDamage;

	protected int slimeDamage;

	protected float touchDamageDistance;

	protected float attackDamageDistance;

	protected float rushDamageDistance;

	protected float shotSlowInterval;

	protected float shotFastInterval;

	protected float poisonZoneInterval;

	protected float poisonZoneTime;

	protected int jumpBigDamage;

	protected int jumpMidDamage;

	protected float bigDamageDistance;

	protected float midDamageDistance;

	protected float jumpVerticalSpeed;

	protected float jumpGravity;

	protected float[] maxJumpIdleTime = new float[2];

	protected float landingGravity;

	protected float attackKnockSpeed;

	protected float rushKnockSpeed;

	protected float jumpBigKnockSpeed;

	protected float jumpMidKnockSpeed;

	protected int[] maxRushTimes = new int[2];

	protected int[] maxRushTimesOnePlayer = new int[2];

	protected int[] probability1 = new int[2];

	protected int[] probability2 = new int[2];

	protected int[] probability3 = new int[2];

	protected int[] probability4 = new int[2];

	protected int[] probability5 = new int[2];

	public float StartLandTime { get; set; }

	public bool CanJump
	{
		get
		{
			return canJump;
		}
		set
		{
			canJump = value;
		}
	}

	public bool HasLanded
	{
		get
		{
			return hasLanded;
		}
		set
		{
			hasLanded = value;
		}
	}

	public float StartRushDistance
	{
		get
		{
			return startRushDistance;
		}
	}

	public float MaxIdleTime
	{
		get
		{
			return maxIdleTime[(int)mindState];
		}
	}

	public float MaxJumpIdleTime
	{
		get
		{
			return maxJumpIdleTime[(int)mindState];
		}
	}

	public int MaxRushTimes
	{
		get
		{
			if (gameWorld.GetPlayingPlayerCount() == 1)
			{
				return maxRushTimesOnePlayer[(int)mindState];
			}
			return maxRushTimes[(int)mindState];
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

	public Spider()
	{
		EnemyBoss.INIT_STATE = new SpiderInitState();
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		lastIdleTime = Time.time;
		lastJumpIdleTime = Time.time;
	}

	public void SetIdleTimeNow()
	{
		lastIdleTime = Time.time;
	}

	public float GetIdleTimeDuration()
	{
		return Time.time - lastIdleTime;
	}

	public void SetJumpIdleTimeNow()
	{
		lastJumpIdleTime = Time.time;
	}

	public float GetJumpIdleTimeDuration()
	{
		return Time.time - lastJumpIdleTime;
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
		PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_dead");
		EnableGravity(true);
		DisableAllEffect();
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(5f, false);
		GameApp.GetInstance().GetUserState().Achievement.KillBoss(1);
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

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		bipObject = enemyObject.transform.FindChild(BoneName.SpiderBip01).gameObject;
		bodyCollider = bipObject.collider;
		leftArmCollider = enemyObject.transform.FindChild(BoneName.SpiderLeftArm).gameObject.collider;
		rightArmCollider = enemyObject.transform.FindChild(BoneName.SpiderRightArm).gameObject.collider;
		trails = new GameObject[2];
		GameObject original = Resources.Load("Effect/Dragon/dragon_trail") as GameObject;
		Vector3 position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
		trails[0] = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		trails[1] = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		leftHandTransform = enemyObject.transform.FindChild(BoneName.SpiderLeftHand);
		rightHandTransform = enemyObject.transform.FindChild(BoneName.SpiderRightHand);
		leftArmTrail = leftHandTransform.FindChild("Trail").gameObject;
		rightArmTrail = rightHandTransform.FindChild("Trail").gameObject;
		spiderRunAudioTimer.SetTimer(0.1f, false);
		spiderRightArmAudioTimer.SetTimer(1f, false);
		spiderLeftArmAudioTimer.SetTimer(1f, false);
		spiderRightArmGroundTimer.SetTimer(1f, false);
		spiderLeftArmGroundTimer.SetTimer(1f, false);
		walkAudioTimer.SetTimer(0.4f, false);
		touchtimer.SetTimer(2f, false);
		firetimer.SetTimer(2f, false);
		criticalTimer.SetTimer(2f, false);
		slowShotTimer.SetTimer(shotSlowInterval, false);
		fastShotTimer.SetTimer(shotFastInterval, false);
		slimeTimer.SetTimer(poisonZoneInterval, false);
		walkAudioName = "Audio/enemy/Spider/juxingjiachong_walk";
		eyeAnimationScript = enemyObject.GetComponentInChildren<AlphaAnimationScript>();
		DisableAllEffect();
	}

	public override void SetState(EnemyState newState)
	{
		base.SetState(newState);
		if (newState == Enemy.IDLE_STATE)
		{
			DisableAllEffect();
		}
	}

	protected override void loadParameters()
	{
		areaCenter = new Vector3(-5.5f, 0f, 7f);
		areaRadius = 21f;
		maxOverRushDistance = 10f;
		hp = 108000;
		hpPercentagePerHit = 0.15f;
		hitTimesForRage = 4;
		stopRushDistance = 4f;
		attackRange = 6f;
		startRushDistance = 29f;
		groupAttackDistance = 12f;
		maxIdleTime[0] = 1f;
		maxCatchingTime[0] = 5f;
		maxIdleTime[1] = 0.64f;
		maxCatchingTime[1] = 5f;
		runSpeed = 12f;
		rushSpeed = 32f;
		spitSpeed = 20f;
		turnSpeed = 0.05f;
		downSpeed = 1f;
		attackDetectionAngle = 20f;
		shotHorizontalRange = 36f;
		shotVerticalRange = 10f;
		minScale = 0.5f;
		maxScale = 0.8f;
		normalScale = 1f;
		shotSlowInterval = 0.2f;
		shotFastInterval = 0.1f;
		poisonZoneInterval = 0.6f;
		poisonZoneTime = 12f;
		touchDamage = 400;
		attackDamage = 2200;
		rushDamage = 2800;
		shotDamage = 1760;
		slimeDamage = 440;
		touchDamageDistance = 5f;
		attackDamageDistance = 9f;
		rushDamageDistance = 5f;
		jumpBigDamage = 3600;
		jumpMidDamage = 880;
		bigDamageDistance = 5f;
		midDamageDistance = 9f;
		jumpVerticalSpeed = 20f;
		jumpGravity = 4f;
		maxJumpIdleTime[0] = 0.3f;
		maxJumpIdleTime[1] = 0.15f;
		landingGravity = 8f;
		touchKnockSpeed = 0.08f;
		attackKnockSpeed = 0.16f;
		rushKnockSpeed = 0.24f;
		jumpBigKnockSpeed = 0.24f;
		jumpMidKnockSpeed = 0.08f;
		maxRushTimes[0] = 1;
		maxRushTimes[1] = 3;
		maxRushTimesOnePlayer[0] = 1;
		maxRushTimesOnePlayer[1] = 2;
		probability1[0] = 28;
		probability2[0] = 46;
		probability3[0] = 76;
		probability4[0] = 100;
		probability1[1] = 20;
		probability2[1] = 50;
		probability3[1] = 68;
		probability4[1] = 90;
		probability5[0] = 70;
		probability5[1] = 50;
	}

	public override bool NearGround()
	{
		if (enemyTransform.position.y - (float)Global.FLOORHEIGHT < 0.5f)
		{
			enemyTransform.position = new Vector3(enemyTransform.position.x, Global.FLOORHEIGHT, enemyTransform.position.z);
			return true;
		}
		return false;
	}

	public bool CloseToRushTarget()
	{
		if (Vector3.Distance(enemyTransform.position, targetToLookAt) < stopRushDistance)
		{
			return true;
		}
		return false;
	}

	public void OnRush()
	{
		maxTurnRadian *= 5f;
		dir = targetToLookAt - enemyTransform.position;
		dir.y = 0f;
		dir.Normalize();
		Vector3 vector = targetToLookAt - areaCenter;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		if (magnitude < areaRadius)
		{
			float a = (areaRadius - magnitude) * 0.5f;
			a = Mathf.Min(a, maxOverRushDistance);
			targetToLookAt += a * dir;
		}
		EnableTrailEffect(true);
	}

	public void DisableAllEffect()
	{
		EnableTrailEffect(false);
		EnableLeftArmTrail(false);
		EnableRightArmTrail(false);
	}

	public void Rush()
	{
		enemyTransform.Translate(dir * rushSpeed * Time.deltaTime, Space.World);
		if (spiderRunAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Spider/juxingjiachong_walk");
			spiderRunAudioTimer.Do();
		}
	}

	public void Shot()
	{
		Shot(0f, 0f, normalScale);
		PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_rage");
	}

	public void Shot(float angleH, float angleV, float scale)
	{
		GameObject original = Resources.Load("Effect/Spider/sfx_spit") as GameObject;
		Vector3 position = enemyTransform.FindChild(BoneName.SpiderLTooth).position;
		Vector3 position2 = enemyTransform.FindChild(BoneName.SpiderRTooth).position;
		Vector3 vector = (position + position2) / 2f;
		GameObject gameObject = Object.Instantiate(original, vector, Quaternion.identity) as GameObject;
		Vector3 vector2 = targetToLookAt - vector;
		vector2.y = 0f;
		Vector3 normalized = vector2.normalized;
		float magnitude = vector2.magnitude;
		float num = magnitude / spitSpeed;
		float num2 = vector.y - 0.5f;
		float num3 = num2 / num + 0.5f * Physics.gravity.y * num;
		Vector3 vector3 = num3 * Vector3.down;
		Vector3 vector4 = vector3 + spitSpeed * normalized;
		gameObject.transform.LookAt(vector + vector4 * 10f);
		gameObject.transform.RotateAround(vector, gameObject.transform.up, angleH);
		gameObject.transform.RotateAround(vector, gameObject.transform.right, angleV);
		SpiderSpitScript component = gameObject.GetComponent<SpiderSpitScript>();
		component.speed = gameObject.transform.forward * vector4.magnitude;
		component.spitDamage = shotDamage;
		component.slimeDamage = slimeDamage;
		component.slimeDisappearTime = poisonZoneTime;
		component.maxSlimeScale = scale;
		component.slimeTimer = slimeTimer;
		if (null != eyeAnimationScript)
		{
			eyeAnimationScript.StartAnimation();
		}
	}

	public void DoContinuousShot()
	{
		if (isContinuousShot)
		{
			float num = animation[AnimationString.SPIDER_CONTINUOUS_SHOT].time / animation[AnimationString.SPIDER_CONTINUOUS_SHOT].clip.length;
			if ((fastShotTimer.Ready() && num < 0.57f) || (slowShotTimer.Ready() && num > 0.57f))
			{
				float angleH = (float)Random.Range(-100, 100) * shotHorizontalRange / 100f;
				float angleV = (float)Random.Range(-100, 100) * shotVerticalRange / 100f;
				float scale = (float)Random.Range(0, 100) * (maxScale - minScale) / 100f + minScale;
				Shot(angleH, angleV, scale);
				slowShotTimer.Do();
				fastShotTimer.Do();
			}
			PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_taunt");
		}
	}

	public void StartContinuousShot()
	{
		isContinuousShot = true;
	}

	public void StopContinuousShot()
	{
		isContinuousShot = false;
	}

	public void NormalAttack()
	{
		float num = animation[AnimationString.SPIDER_NORMAL_ATTACK].time / animation[AnimationString.SPIDER_NORMAL_ATTACK].clip.length;
		if (num > 0.43f && num < 0.71f)
		{
			if (spiderRightArmAudioTimer.Ready())
			{
				PlaySound("Audio/enemy/Spider/juxingjiachong_attack_04");
				spiderRightArmAudioTimer.Do();
			}
			if (num > 0.61f && spiderRightArmGroundTimer.Ready())
			{
				GameObject original = Resources.Load("Effect/Spider/sfx_rock_boom") as GameObject;
				Object.Instantiate(original, new Vector3(rightHandTransform.position.x, 0f, rightHandTransform.position.z), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
				GameObject original2 = Resources.Load("Effect/Spider/ground_crack") as GameObject;
				Object.Instantiate(original2, new Vector3(rightHandTransform.position.x, 0.1f, rightHandTransform.position.z), Quaternion.Euler(270f, Random.Range(0f, 360f), 0f));
				spiderRightArmGroundTimer.Do();
			}
		}
		if (num > 0.45f && num < 0.7f)
		{
			EnableRightArmTrail(true);
		}
		else
		{
			EnableRightArmTrail(false);
		}
	}

	public void DoubleAttack()
	{
		float num = animation[AnimationString.SPIDER_DOUBLE_ATTACK].time / animation[AnimationString.SPIDER_DOUBLE_ATTACK].clip.length;
		if (num > 0.2f && num < 0.4f)
		{
			if (spiderRightArmAudioTimer.Ready())
			{
				PlaySound("Audio/enemy/Spider/juxingjiachong_attack_04");
				spiderRightArmAudioTimer.Do();
			}
			if (num > 0.34f && spiderRightArmGroundTimer.Ready())
			{
				GameObject original = Resources.Load("Effect/Spider/sfx_rock_boom") as GameObject;
				Object.Instantiate(original, new Vector3(rightHandTransform.position.x, 0f, rightHandTransform.position.z), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
				GameObject original2 = Resources.Load("Effect/Spider/ground_crack") as GameObject;
				Object.Instantiate(original2, new Vector3(rightHandTransform.position.x, 0.1f, rightHandTransform.position.z), Quaternion.Euler(270f, Random.Range(0f, 360f), 0f));
				spiderRightArmGroundTimer.Do();
			}
		}
		if (num > 0.22f && num < 0.44f)
		{
			EnableRightArmTrail(true);
		}
		else
		{
			EnableRightArmTrail(false);
		}
		if (num > 0.6f && num < 0.8f)
		{
			if (spiderLeftArmAudioTimer.Ready())
			{
				PlaySound("Audio/enemy/Spider/juxingjiachong_attack_04");
				spiderLeftArmAudioTimer.Do();
			}
			if (num > 0.77f && spiderLeftArmGroundTimer.Ready())
			{
				GameObject original3 = Resources.Load("Effect/Spider/sfx_rock_boom") as GameObject;
				Object.Instantiate(original3, new Vector3(leftHandTransform.position.x, 0f, leftHandTransform.position.z), Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
				GameObject original4 = Resources.Load("Effect/Spider/ground_crack") as GameObject;
				Object.Instantiate(original4, new Vector3(leftHandTransform.position.x, 0.1f, leftHandTransform.position.z), Quaternion.Euler(270f, Random.Range(0f, 360f), 0f));
				spiderLeftArmGroundTimer.Do();
			}
		}
		if (num > 0.7f && num < 0.92f)
		{
			EnableLeftArmTrail(true);
		}
		else
		{
			EnableLeftArmTrail(false);
		}
	}

	public override void CheckHit()
	{
		if (firetimer.Ready() && (leftArmCollider.bounds.Intersects(player.GetCollider().bounds) || rightArmCollider.bounds.Intersects(player.GetCollider().bounds)))
		{
			player.OnHit(attackDamage);
			CheckKnocked(attackKnockSpeed);
			firetimer.Do();
		}
	}

	public override void TouchPlayer()
	{
		if (state != Enemy.DEAD_STATE && state != START_JUMP_STATE && state != LANDING_STATE && touchtimer.Ready() && bodyCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			if (state == RUSH_STATE)
			{
				player.OnHit(rushDamage);
				CheckKnocked(rushKnockSpeed);
			}
			else
			{
				player.OnHit(touchDamage);
				CheckKnocked(touchKnockSpeed);
			}
			touchtimer.Do();
		}
	}

	public override bool CanTurn()
	{
		return state != Enemy.DEAD_STATE && state != Enemy.IDLE_STATE && state != JUMP_STATE && state != LANDING_STATE && !CanJump;
	}

	public override bool NeedMoveDown()
	{
		return enemyTransform.position.y - (float)Global.FLOORHEIGHT > 0.1f;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		trails[0].transform.position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z) + enemyTransform.forward * 4f + enemyTransform.right * 2f;
		trails[1].transform.position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z) + enemyTransform.forward * 4f - enemyTransform.right * 2f;
	}

	public void StartRush()
	{
		animation[AnimationString.SPIDER_START_RUSH].time = 0f;
		PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_run");
	}

	public void StartJump()
	{
		float num = (0f - jumpVerticalSpeed) / Physics.gravity.y / jumpGravity;
		Vector3 vector = target.position - enemyTransform.position;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float num2 = magnitude / num;
		Vector3 vector2 = vector.normalized * num2;
		jumpSpeed = jumpVerticalSpeed * Vector3.up + vector2;
		EnableGravity(false);
	}

	public bool Jump(float deltaTime)
	{
		CharacterController characterController = enemyObject.collider as CharacterController;
		if (characterController != null)
		{
			characterController.Move(jumpSpeed * deltaTime);
		}
		jumpSpeed += Physics.gravity * jumpGravity * deltaTime;
		if (jumpSpeed.y < 0f)
		{
			return true;
		}
		return false;
	}

	public void StartLand()
	{
		jumpSpeed = Vector3.zero;
		float num = enemyTransform.position.y - (float)Global.FLOORHEIGHT - 0.1f;
		float num2 = Mathf.Sqrt(2f * num / Physics.gravity.y / landingGravity);
		StartLandTime = Time.time + num2 - 0.26f;
	}

	public void Land(float deltaTime)
	{
		if (!HasLanded)
		{
			CharacterController characterController = enemyObject.collider as CharacterController;
			if (characterController != null)
			{
				characterController.Move(jumpSpeed * deltaTime);
			}
			jumpSpeed += Physics.gravity * landingGravity * deltaTime;
			if (enemyTransform.position.y < (float)Global.FLOORHEIGHT + 0.1f)
			{
				enemyTransform.position = new Vector3(enemyTransform.position.x, (float)Global.FLOORHEIGHT + 0.1f, enemyTransform.position.z);
				OnLand();
				HasLanded = true;
			}
		}
	}

	public void OnLand()
	{
		GameObject original = Resources.Load("Effect/Spider/sfx_fragment") as GameObject;
		Vector3 position = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
		Object.Instantiate(original, position, Quaternion.identity);
		GameObject original2 = Resources.Load("Effect/Spider/sfx_smoke") as GameObject;
		Vector3 position2 = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
		Object.Instantiate(original2, position2, Quaternion.identity);
		float sqrMagnitude = (player.GetTransform().position - enemyTransform.position).sqrMagnitude;
		if (sqrMagnitude < bigDamageDistance * bigDamageDistance)
		{
			player.OnHit(jumpBigDamage);
			CheckKnocked(jumpBigKnockSpeed);
		}
		else if (sqrMagnitude < midDamageDistance * midDamageDistance)
		{
			player.OnHit(jumpMidDamage);
			CheckKnocked(jumpMidKnockSpeed);
		}
		PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_jump03");
	}

	public byte ChangeStateFar()
	{
		int num = Random.Range(0, 100);
		byte b = 0;
		if (num < Probability1)
		{
			SetState(Enemy.CATCHING_STATE);
			SetCatchingTimeNow();
			return 30;
		}
		if (num < Probability2)
		{
			SetState(CONTINUOUS_SHOT_STATE);
			return 36;
		}
		if (num < Probability3)
		{
			SetState(SHOT_STATE);
			return 35;
		}
		if (num < Probability4)
		{
			SetState(START_JUMP_STATE);
			return 33;
		}
		SetState(START_RUSH_STATE);
		return 34;
	}

	public byte ChangeStateNear()
	{
		byte b = 0;
		int nearestPlayer = GetNearestPlayer();
		ChangeTargetPlayer(nearestPlayer);
		float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
		if (nearestDistanceToTargetPlayer < groupAttackDistance)
		{
			SetState(DOUBLE_ATTACK_STATE);
			return 32;
		}
		int num = Random.Range(0, 100);
		if (num < Probability5)
		{
			SetState(NORMAL_ATTACK_STATE);
			return 31;
		}
		SetState(DOUBLE_ATTACK_STATE);
		return 32;
	}

	public override void OnIdle()
	{
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		byte b = 0;
		int targetID = GetTargetPlayer().GetUserID();
		if (GetIdleTimeDuration() > MaxIdleTime)
		{
			float averagehorizontalDistance = GetAveragehorizontalDistance();
			if (averagehorizontalDistance < base.AttackRange)
			{
				b = ChangeStateNear();
				targetID = GetNearestPlayer();
			}
			else if (averagehorizontalDistance < startRushDistance)
			{
				b = ChangeStateFar();
				targetID = ((b != 30) ? GetRandomPlayer() : GetNearestPlayer());
			}
			else
			{
				SetState(START_RUSH_STATE);
				b = 34;
				targetID = GetFarthestPlayer();
			}
		}
		if (b != 0)
		{
			ChangeTargetPlayer(targetID);
		}
		if (b == 34)
		{
			StartRush();
		}
		if (b == 33)
		{
			StartJump();
		}
		if (b != 0)
		{
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
		StopSound("juxingjiachong");
		StopSound("juxingjiachong_rage");
		StopSound("juxingjiachong_taunt");
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
			if (state != START_JUMP_STATE && state != JUMP_STATE && state != LANDING_STATE && (float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				CanJump = false;
				SetState(SPIDER_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_hurt");
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
			if (state != START_JUMP_STATE && state != JUMP_STATE && state != LANDING_STATE && (float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				CanJump = false;
				SetState(SPIDER_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_hurt");
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
		EnemyState result = null;
		byte b = 0;
		if (GetHorizontalSqrDistanceFromTarget() < base.AttackRange * base.AttackRange)
		{
			float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
			if (nearestDistanceToTargetPlayer < groupAttackDistance)
			{
				result = DOUBLE_ATTACK_STATE;
				b = 32;
			}
			else
			{
				int num = Random.Range(0, 100);
				if (num < Probability5)
				{
					SetState(NORMAL_ATTACK_STATE);
					b = 31;
				}
				else
				{
					SetState(DOUBLE_ATTACK_STATE);
					b = 32;
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
