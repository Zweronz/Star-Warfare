using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Earthworm : EnemyBoss
{
	protected const float MIN_DISTANCE_TO_WALL = 10f;

	public static EnemyState RUSH_STATE = new EarthwormRushingState();

	public static EnemyState SHOT_STATE = new EarthwormShotState();

	public static EnemyState CONTINUOUS_SHOT_STATE = new EarthwormContinuousShotState();

	public static EnemyState NORMAL_ATTACK_STATE = new EarthwormNormalAttackState();

	public static EnemyState RAGE_STATE = new EarthwormRageState();

	public static EnemyState EARTHWORM_GOTHIT_STATE = new EarthwormGotHitState();

	public static EnemyState START_RUSH_STATE = new EarthwormStartRushState();

	public static EnemyState RUSH_END_STATE = new EarthwormRushEndState();

	public static EnemyState DRILLIN_STATE = new EarthwormDrillinState();

	public static EnemyState DRILLOUT_STATE = new EarthwormDrilloutState();

	public static EnemyState DRILL_IDLE_STATE = new EarthwormDrillIdleState();

	public static EnemyState START_SUPER_RUSH_STATE = new EarthwormStartSuperRushState();

	public static EnemyState SUPER_RUSH_STATE = new EarthwormSuperRushingState();

	public static EnemyState SUPER_RUSH_END_STATE = new EarthwormSuperRushEndState();

	protected Collider[] touchColliders;

	protected Collider[] tailColliders;

	protected float lastIdleTime;

	protected float lastDrillIdleTime;

	protected float lastSuperRushTime;

	protected float lastStartRushTime;

	protected Timer earthwormRunAudioTimer = new Timer();

	protected Timer earthwormTailSlashAudioTimer = new Timer();

	protected Timer earthwormTailGroundAudioTimer = new Timer();

	protected Timer earthwormTailGroundAudioTimer2 = new Timer();

	protected Timer earthwormTurnAroundTimer = new Timer();

	protected Timer drillInTimer = new Timer();

	protected Timer drillIdleTimer = new Timer();

	protected Timer drillOutTimer = new Timer();

	protected Timer drillDownTimer = new Timer();

	protected Timer hitWallEffectTimer = new Timer();

	protected Timer rageAudioTimer = new Timer();

	protected bool isContinuousShot;

	protected Vector3 wallHitPosition = default(Vector3);

	protected Vector3 wallDirection = default(Vector3);

	protected float outWallDistance;

	protected float inWallDistance;

	protected float centerAreaMinX;

	protected float centerAreaMaxX;

	protected float centerAreaMinZ;

	protected float centerAreaMaxZ;

	protected Timer continuousShotTimer = new Timer();

	protected Timer rageContinuousShotTimer = new Timer();

	protected Timer slimeTimer = new Timer();

	protected float stopRushDistance;

	protected float startDrillDistance;

	protected float hitWallEffectDistance;

	protected float superRushTimeOut;

	protected float[] maxIdleTime = new float[2];

	protected float[] maxSuperRushInterval = new float[2];

	protected float rushSpeed;

	protected float superRushSpeed;

	protected float spitSpeed;

	protected float slowEffect;

	protected float shotHorizontalRange;

	protected float shotVerticalRange;

	protected float minScale;

	protected float maxScale;

	protected float normalScale;

	protected float rageShotHorizontalRange;

	protected float rageShotVerticalRange;

	protected float rageMinScale;

	protected float rageMaxScale;

	protected int spitDamage;

	protected int slimeDamage;

	protected int superRushDamage;

	protected float continuousShotInterval;

	protected float rageContinuousShotInterval;

	protected float poisonZoneInterval;

	protected float poisonZoneTime;

	protected float tailAttackRadius;

	protected int drillBigDamage;

	protected int drillMidDamage;

	protected float bigDamageDistance;

	protected float midDamageDistance;

	protected float[] maxDrillIdleTime = new float[2];

	protected float attackKnockSpeed;

	protected float rushKnockSpeed;

	protected float superRushKnockSpeed;

	protected float drillBigKnockSpeed;

	protected float drillMidKnockSpeed;

	protected int[] maxRushTimes = new int[2];

	protected int[] maxRushTimesOnePlayer = new int[2];

	protected int[] probability1 = new int[2];

	protected int[] probability2 = new int[2];

	protected int[] probability3 = new int[2];

	protected int[] probability4 = new int[2];

	protected int[] probability5 = new int[2];

	public float StartDrillDistance
	{
		get
		{
			return startDrillDistance;
		}
	}

	public float MaxIdleTime
	{
		get
		{
			return maxIdleTime[(int)mindState];
		}
	}

	public float MaxDrillIdleTime
	{
		get
		{
			return maxDrillIdleTime[(int)mindState];
		}
	}

	public float MaxSuperRushInterval
	{
		get
		{
			return maxSuperRushInterval[(int)mindState];
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

	public Earthworm()
	{
		EnemyBoss.INIT_STATE = new EarthwormInitState();
		hasShadow = false;
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		lastIdleTime = Time.time;
		lastDrillIdleTime = Time.time;
		lastSuperRushTime = Time.time;
		lastStartRushTime = Time.time;
	}

	public void SetIdleTimeNow()
	{
		lastIdleTime = Time.time;
	}

	public float GetIdleTimeDuration()
	{
		return Time.time - lastIdleTime;
	}

	public void SetDrillIdleTimeNow()
	{
		lastDrillIdleTime = Time.time;
	}

	public float GetDrillIdleTimeDuration()
	{
		return Time.time - lastDrillIdleTime;
	}

	public void SetSuperRushTimeNow()
	{
		lastSuperRushTime = Time.time;
	}

	public float GetSuperRushTimeDuration()
	{
		return Time.time - lastSuperRushTime;
	}

	public void SetStartRushTimeNow()
	{
		lastStartRushTime = Time.time;
	}

	public float GetStartRushTimeDuration()
	{
		return Time.time - lastStartRushTime;
	}

	public override void OnDead()
	{
		enemyObject.layer = PhysicsLayer.DEADBODY;
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(3000f, false);
		GameObject original = Resources.Load("Effect/update_effect/efffect_worm_blood_001") as GameObject;
		UnityEngine.Object.Instantiate(original, bipObject.transform.position, Quaternion.identity);
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		StopSoundOnHit();
		PlaySoundSingle("Audio/enemy/Earthworm/dead");
		EnableGravity(true);
		DisableAllEffect();
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(5f, false);
		GameApp.GetInstance().GetUserState().Achievement.KillBoss(1);
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
		bipObject = enemyObject.transform.Find(BoneName.EarthwormBip01).gameObject;
		touchColliders = new Collider[3];
		touchColliders[0] = enemyObject.transform.Find(BoneName.EarthwormNeck).gameObject.GetComponent<Collider>();
		touchColliders[1] = enemyObject.transform.Find(BoneName.EarthwormSpine1).gameObject.GetComponent<Collider>();
		touchColliders[2] = enemyObject.transform.Find(BoneName.EarthwormRightThigh).gameObject.GetComponent<Collider>();
		tailColliders = new Collider[6];
		tailColliders[0] = enemyObject.transform.Find(BoneName.EarthwormTail2).gameObject.GetComponent<Collider>();
		tailColliders[1] = enemyObject.transform.Find(BoneName.EarthwormTail4).gameObject.GetComponent<Collider>();
		tailColliders[2] = enemyObject.transform.Find(BoneName.EarthwormTail6).gameObject.GetComponent<Collider>();
		tailColliders[3] = enemyObject.transform.Find(BoneName.EarthwormTail8).gameObject.GetComponent<Collider>();
		tailColliders[4] = enemyObject.transform.Find(BoneName.EarthwormTail10).gameObject.GetComponent<Collider>();
		tailColliders[5] = enemyObject.transform.Find(BoneName.EarthwormTail12).gameObject.GetComponent<Collider>();
		bodyCollider = touchColliders[0];
		trails = new GameObject[1];
		GameObject original = Resources.Load("Effect/update_effect/efffect_worm_move_001") as GameObject;
		Vector3 position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z);
		trails[0] = UnityEngine.Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		earthwormRunAudioTimer.SetTimer(0.1f, false);
		earthwormTailSlashAudioTimer.SetTimer(3f, false);
		earthwormTurnAroundTimer.SetTimer(2f, false);
		earthwormTailGroundAudioTimer.SetTimer(3f, true);
		earthwormTailGroundAudioTimer2.SetTimer(5f, true);
		walkAudioTimer.SetTimer(0.4f, false);
		touchtimer.SetTimer(2f, false);
		firetimer.SetTimer(2f, false);
		criticalTimer.SetTimer(2f, false);
		continuousShotTimer.SetTimer(continuousShotInterval, false);
		rageContinuousShotTimer.SetTimer(rageContinuousShotInterval, true);
		slimeTimer.SetTimer(poisonZoneInterval, false);
		drillInTimer.SetTimer(8f, false);
		drillIdleTimer.SetTimer(8f, false);
		drillOutTimer.SetTimer(8f, false);
		drillDownTimer.SetTimer(8f, false);
		hitWallEffectTimer.SetTimer(3f, true);
		rageAudioTimer.SetTimer(6f, true);
		walkAudioName = "Audio/enemy/Earthworm/run";
		DisableAllEffect();
		StartRage();
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
		centerAreaMinX = -16f;
		centerAreaMaxX = 20f;
		centerAreaMinZ = -9.5f;
		centerAreaMaxZ = 14.5f;
		outWallDistance = 60f;
		inWallDistance = 80f;
		maxOverRushDistance = 10f;
		superRushTimeOut = 5f;
		hp = 440000;
		hpPercentagePerHit = 0.15f;
		hitTimesForRage = 3;
		stopRushDistance = 4f;
		attackRange = 14f;
		startDrillDistance = 32f;
		hitWallEffectDistance = 15f;
		maxIdleTime[0] = 0.2f;
		maxCatchingTime[0] = 5f;
		maxSuperRushInterval[0] = 50f;
		maxIdleTime[1] = 0.1f;
		maxCatchingTime[1] = 5f;
		maxSuperRushInterval[1] = 24f;
		runSpeed = 16f;
		rushSpeed = 42f;
		superRushSpeed = 50f;
		spitSpeed = 24f;
		turnSpeed = 0.05f;
		downSpeed = 1f;
		shotHorizontalRange = 36f;
		shotVerticalRange = 16f;
		minScale = 1.1f;
		maxScale = 1.5f;
		normalScale = 2.2f;
		continuousShotInterval = 0.16f;
		poisonZoneInterval = 0.6f;
		poisonZoneTime = 20f;
		slowEffect = 0.3f;
		rageShotHorizontalRange = 50f;
		rageShotVerticalRange = 20f;
		rageMinScale = 1.1f;
		rageMaxScale = 1.5f;
		rageContinuousShotInterval = 0.12f;
		touchDamage = 1200;
		attackDamage = 6400;
		rushDamage = 5600;
		superRushDamage = 7200;
		shotDamage = 5560;
		slimeDamage = 660;
		tailAttackRadius = 7f;
		drillBigDamage = 7200;
		drillMidDamage = 3600;
		bigDamageDistance = 10f;
		midDamageDistance = 20f;
		maxDrillIdleTime[0] = 4.4f;
		maxDrillIdleTime[1] = 3f;
		touchKnockSpeed = 0.08f;
		attackKnockSpeed = 0.16f;
		rushKnockSpeed = 0.24f;
		superRushKnockSpeed = 0.36f;
		drillBigKnockSpeed = 0.24f;
		drillMidKnockSpeed = 0.08f;
		maxRushTimes[0] = 3;
		maxRushTimes[1] = 3;
		maxRushTimesOnePlayer[0] = 3;
		maxRushTimesOnePlayer[1] = 3;
		probability1[0] = 35;
		probability2[0] = 50;
		probability3[0] = 85;
		probability4[0] = 100;
		probability1[1] = 25;
		probability2[1] = 55;
		probability3[1] = 70;
		probability4[1] = 85;
		probability5[0] = 0;
		probability5[1] = 20;
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

	public bool SuperRushTimeOut()
	{
		return GetStartRushTimeDuration() > superRushTimeOut;
	}

	public void OnRush()
	{
		maxTurnRadian *= 5f;
		Vector3 vector = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
		Vector3 vector2 = new Vector3(targetToLookAt.x, 1f, targetToLookAt.z);
		dir = vector2 - vector;
		dir.Normalize();
		ray = new Ray(vector2, dir);
		if (Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
		{
			wallHitPosition = rayhit.point;
		}
		float magnitude = (vector2 - wallHitPosition).magnitude;
		if (magnitude < 10f - stopRushDistance + maxOverRushDistance)
		{
			Vector3 vector3 = wallHitPosition - dir * (10f - stopRushDistance);
			targetToLookAt = new Vector3(vector3.x, targetToLookAt.y, vector3.z);
		}
		else
		{
			targetToLookAt += dir * maxOverRushDistance;
		}
		EnableTrailEffect(true);
	}

	public void OnSuperRush()
	{
		maxTurnRadian *= 5f;
		Vector3 vector = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
		Vector3 vector2 = new Vector3(targetToLookAt.x, 1f, targetToLookAt.z);
		dir = vector2 - vector;
		dir.Normalize();
		ray = new Ray(vector2, dir);
		if (Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
		{
			wallHitPosition = rayhit.point;
			wallDirection = -rayhit.collider.gameObject.transform.right;
		}
		Vector3 vector3 = wallHitPosition + dir * (outWallDistance + stopRushDistance);
		targetToLookAt = new Vector3(vector3.x, targetToLookAt.y, vector3.z);
		EnableTrailEffect(true);
	}

	public void DisableAllEffect()
	{
		EnableTrailEffect(false);
	}

	public void Rush()
	{
		enemyTransform.Translate(dir * rushSpeed * Time.deltaTime, Space.World);
		if (earthwormRunAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Earthworm/attack_dash_idle");
			earthwormRunAudioTimer.Do();
		}
	}

	public void SuperRush()
	{
		enemyTransform.Translate(dir * superRushSpeed * Time.deltaTime, Space.World);
		if (earthwormRunAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Earthworm/attack_dash_fastidle2");
			earthwormRunAudioTimer.Do();
		}
		if (GetAttackCount() < MaxRushTimes)
		{
			Vector3 a = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
			if (hitWallEffectTimer.Ready() && Vector3.Distance(a, wallHitPosition) < hitWallEffectDistance)
			{
				hitWallEffectTimer.Do();
				GameObject original = Resources.Load("Effect/update_effect/efffect_worm_appare_003") as GameObject;
				Vector3 vector = new Vector3((wallHitPosition + 0.01f * wallDirection).x, 2f, (wallHitPosition + 0.01f * wallDirection).z);
				GameObject gameObject = UnityEngine.Object.Instantiate(original, vector, Quaternion.identity) as GameObject;
				gameObject.transform.LookAt(vector + 10f * wallDirection);
			}
		}
	}

	public void Shot()
	{
		Shot(0f, 0f, normalScale);
		PlaySoundSingle("Audio/enemy/Earthworm/attack_spit1");
	}

	public void Shot(float angleH, float angleV, float scale)
	{
		GameObject original = Resources.Load("Effect/update_effect/efffect_worm_Poison_fly_001") as GameObject;
		Vector3 position = enemyTransform.Find(BoneName.EarthwormMouth).position;
		GameObject gameObject = UnityEngine.Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		Vector3 vector = targetToLookAt - position;
		vector.y = 0f;
		Vector3 normalized = vector.normalized;
		float magnitude = vector.magnitude;
		float num = magnitude / spitSpeed;
		float num2 = position.y - 0.5f;
		float num3 = num2 / num + 0.5f * Physics.gravity.y * num;
		Vector3 vector2 = num3 * Vector3.down;
		Vector3 vector3 = vector2 + spitSpeed * normalized;
		gameObject.transform.LookAt(position + vector3 * 10f);
		gameObject.transform.RotateAround(position, gameObject.transform.up, angleH);
		gameObject.transform.RotateAround(position, gameObject.transform.right, angleV);
		EarthwormSpitScript component = gameObject.GetComponent<EarthwormSpitScript>();
		component.speed = gameObject.transform.forward * vector3.magnitude;
		component.spitDamage = shotDamage;
		component.slimeDamage = slimeDamage;
		component.slimeDisappearTime = poisonZoneTime;
		component.maxSlimeScale = scale;
		component.slimeTimer = slimeTimer;
		component.slowEffect = slowEffect;
	}

	public void DoContinuousShot()
	{
		if (isContinuousShot)
		{
			float num = animation[AnimationString.EARTHWORM_CONTINUOUS_SHOT].time / animation[AnimationString.EARTHWORM_CONTINUOUS_SHOT].clip.length;
			if (continuousShotTimer.Ready())
			{
				float angleH = (float)Random.Range(-100, 100) * shotHorizontalRange / 100f;
				float angleV = (float)Random.Range(-100, 100) * shotVerticalRange / 100f;
				float scale = (float)Random.Range(0, 100) * (maxScale - minScale) / 100f + minScale;
				Shot(angleH, angleV, scale);
				continuousShotTimer.Do();
			}
			PlaySoundSingle("Audio/enemy/Earthworm/attack_spit2");
		}
	}

	public void DoRageContinuousShot()
	{
		if (isContinuousShot && rageContinuousShotTimer.Ready())
		{
			float angleH = (float)Random.Range(-100, 100) * rageShotHorizontalRange / 100f;
			float angleV = (float)Random.Range(-100, 100) * rageShotVerticalRange / 100f;
			float scale = (float)Random.Range(0, 100) * (rageMaxScale - rageMinScale) / 100f + rageMinScale;
			Shot(angleH, angleV, scale);
			rageContinuousShotTimer.Do();
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

	public void LookAtTargetInNormalAttack()
	{
		targetToLookAt = new Vector3(2f * enemyTransform.position.x - target.transform.position.x, enemyTransform.position.y, 2f * enemyTransform.position.z - target.transform.position.z);
		Vector3 to = new Vector3(enemyTransform.forward.x, 0f, enemyTransform.forward.z);
		Vector3 from = targetToLookAt - enemyTransform.position;
		from.y = 0f;
		float f = Vector3.Angle(from, to);
		float num = Mathf.Abs(f) * ((float)Math.PI / 180f);
		maxTurnRadian = num * 0.025f;
	}

	public void NormalAttack()
	{
		float num = animation[AnimationString.EARTHWORM_NORMAL_ATTACK].time / animation[AnimationString.EARTHWORM_NORMAL_ATTACK].clip.length;
		if (num > 0.2f && num < 0.8f)
		{
			if (earthwormTailSlashAudioTimer.Ready())
			{
				PlaySound("Audio/enemy/Earthworm/attack_tail1");
				earthwormTailSlashAudioTimer.Do();
			}
			if (num > 0.51f && earthwormTailGroundAudioTimer.Ready())
			{
				PlaySound("Audio/enemy/Earthworm/attack_tail2");
				earthwormTailGroundAudioTimer.Do();
				GameObject original = Resources.Load("Effect/update_effect/efffect_worm_attack_001") as GameObject;
				Transform transform = enemyObject.transform.Find(BoneName.EarthwormTail12);
				Vector3 vector = transform.position - 2f * transform.right;
				Vector3 position = new Vector3(vector.x, 0.02f, vector.z);
				Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
				UnityEngine.Object.Instantiate(original, position, rotation);
			}
		}
	}

	public void CheckNormalAttackHit()
	{
		if (!firetimer.Ready())
		{
			return;
		}
		float num = animation[AnimationString.EARTHWORM_NORMAL_ATTACK].time / animation[AnimationString.EARTHWORM_NORMAL_ATTACK].clip.length;
		if (num > 0.51f && num < 0.61f)
		{
			Transform transform = enemyObject.transform.Find(BoneName.EarthwormTail12);
			Vector3 vector = transform.position - 2f * transform.right;
			Vector3 vector2 = new Vector3(vector.x, 0.02f, vector.z);
			float sqrMagnitude = (player.GetTransform().position - vector2).sqrMagnitude;
			if (sqrMagnitude < tailAttackRadius * tailAttackRadius)
			{
				player.OnHit(attackDamage);
				CheckKnocked(attackKnockSpeed);
				firetimer.Do();
			}
		}
	}

	private void CheckRageHit()
	{
		if (!firetimer.Ready())
		{
			return;
		}
		float num = animation[AnimationString.ENEMY_RAGE].time / animation[AnimationString.ENEMY_RAGE].clip.length;
		if ((num > 0.32f && num < 0.42f) || (num > 0.65f && num < 0.75f))
		{
			Transform transform = enemyObject.transform.Find(BoneName.EarthwormTail12);
			Vector3 vector = transform.position - 2f * transform.right;
			Vector3 vector2 = new Vector3(vector.x, 0.02f, vector.z);
			float sqrMagnitude = (player.GetTransform().position - vector2).sqrMagnitude;
			if (sqrMagnitude < tailAttackRadius * tailAttackRadius)
			{
				player.OnHit(attackDamage);
				CheckKnocked(attackKnockSpeed);
				firetimer.Do();
			}
		}
	}

	public override void CheckHit()
	{
		if (!firetimer.Ready())
		{
			return;
		}
		bool flag = false;
		Collider[] array = tailColliders;
		foreach (Collider collider in array)
		{
			if (collider.bounds.Intersects(player.GetCollider().bounds))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			player.OnHit(attackDamage);
			CheckKnocked(attackKnockSpeed);
			firetimer.Do();
		}
	}

	public override void TouchPlayer()
	{
		if (state == Enemy.DEAD_STATE || !touchtimer.Ready())
		{
			return;
		}
		bool flag = false;
		Collider[] array = touchColliders;
		foreach (Collider collider in array)
		{
			if (collider.bounds.Intersects(player.GetCollider().bounds))
			{
				flag = true;
				break;
			}
		}
		if (!flag && state != NORMAL_ATTACK_STATE)
		{
			Collider[] array2 = tailColliders;
			foreach (Collider collider2 in array2)
			{
				if (collider2.bounds.Intersects(player.GetCollider().bounds))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			if (state == RUSH_STATE)
			{
				player.OnHit(rushDamage);
				CheckKnocked(rushKnockSpeed);
			}
			else if (state == SUPER_RUSH_STATE)
			{
				player.OnHit(superRushDamage);
				CheckKnocked(superRushKnockSpeed);
			}
			else if (state == DRILLOUT_STATE)
			{
				player.OnHit(drillBigDamage);
				CheckKnocked(drillBigKnockSpeed);
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
		return state != Enemy.DEAD_STATE && state != Enemy.IDLE_STATE && state != DRILLIN_STATE && state != DRILL_IDLE_STATE && state != DRILLOUT_STATE;
	}

	public override bool NeedMoveDown()
	{
		return enemyTransform.position.y - (float)Global.FLOORHEIGHT > 0.1f;
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		trails[0].transform.position = new Vector3(enemyTransform.position.x, 0.5f, enemyTransform.position.z) + enemyTransform.forward * 4f;
	}

	public void StartRage()
	{
		targetToLookAt = enemyTransform.position + 20f * enemyTransform.forward;
	}

	public void StartRush()
	{
		animation[AnimationString.EARTHWORM_START_RUSH].time = 0f;
		PlaySoundSingle("Audio/enemy/Earthworm/attack_dash_start");
	}

	public void CreateEnterBlackhole()
	{
		hitWallEffectTimer.SetTimer(3f, true);
		Vector3 vector = new Vector3(enemyTransform.position.x, 1f, enemyTransform.position.z);
		Vector3 vector2 = new Vector3(target.position.x, 1f, target.position.z);
		Vector3 direction = vector2 - vector;
		float magnitude = direction.magnitude;
		direction.Normalize();
		ray = new Ray(vector, direction);
		if (Physics.Raycast(ray, out rayhit, magnitude, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
		{
			wallHitPosition = rayhit.point;
			wallDirection = -rayhit.collider.gameObject.transform.right;
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_appare_003") as GameObject;
			Vector3 vector3 = new Vector3((wallHitPosition + 0.01f * wallDirection).x, 2f, (wallHitPosition + 0.01f * wallDirection).z);
			GameObject gameObject = UnityEngine.Object.Instantiate(original, vector3, Quaternion.identity) as GameObject;
			gameObject.transform.LookAt(vector3 + 10f * wallDirection);
		}
	}

	public void StartSuperRush()
	{
		if (GetAttackCount() > 0)
		{
			animation[AnimationString.EARTHWORM_START_RUSH].time = 1f;
		}
		else
		{
			animation[AnimationString.EARTHWORM_START_RUSH].time = 0f;
		}
		PlaySoundSingle("Audio/enemy/Earthworm/attack_dash_start");
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && GetAttackCount() > 0)
		{
			Vector3 vector = new Vector3(Random.Range(centerAreaMinX, centerAreaMaxX), 1f, Random.Range(centerAreaMinZ, centerAreaMaxZ));
			Vector3 vector2 = new Vector3(target.position.x, 1f, target.position.z);
			Vector3 vector3 = vector - vector2;
			vector3.Normalize();
			ray = new Ray(vector, vector3);
			if (Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
			{
				wallHitPosition = rayhit.point;
				wallDirection = -rayhit.collider.gameObject.transform.right;
			}
			Vector3 vector4 = wallHitPosition + vector3 * inWallDistance;
			enemyTransform.position = new Vector3(vector4.x, enemyTransform.position.y, vector4.z);
		}
	}

	public void Rage()
	{
		float num = animation[AnimationString.ENEMY_RAGE].time / animation[AnimationString.ENEMY_RAGE].clip.length;
		if (rageAudioTimer.Ready() && num > 0.25f)
		{
			rageAudioTimer.Do();
			PlaySoundSingle("Audio/enemy/Earthworm/rage2");
		}
		DoRageContinuousShot();
		if (base.CanShot && num > 0.27f)
		{
			StartContinuousShot();
			base.CanShot = false;
		}
		if (!base.CanShot && num > 0.73f)
		{
			StopContinuousShot();
		}
		if (num > 0.32f && earthwormTailGroundAudioTimer2.Ready())
		{
			PlaySound("Audio/enemy/Earthworm/attack_tail2");
			earthwormTailGroundAudioTimer2.Do();
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_attack_001") as GameObject;
			Transform transform = enemyObject.transform.Find(BoneName.EarthwormTail12);
			Vector3 vector = transform.position - 2f * transform.right;
			Vector3 position = new Vector3(vector.x, 0.02f, vector.z);
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original, position, rotation);
		}
		if (num > 0.65f && earthwormTailGroundAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Earthworm/attack_tail2");
			earthwormTailGroundAudioTimer.Do();
			GameObject original2 = Resources.Load("Effect/update_effect/efffect_worm_attack_001") as GameObject;
			Transform transform2 = enemyObject.transform.Find(BoneName.EarthwormTail12);
			Vector3 vector2 = transform2.position - 2f * transform2.right;
			Vector3 position2 = new Vector3(vector2.x, 0.02f, vector2.z);
			Quaternion rotation2 = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original2, position2, rotation2);
		}
		if (num > 0.2f)
		{
			CheckHit();
		}
		CheckRageHit();
	}

	public void CheckDrillout()
	{
		if (!touchtimer.Ready())
		{
			return;
		}
		float num = animation[AnimationString.EARTHWORM_DRILLOUT].time / animation[AnimationString.EARTHWORM_DRILLOUT].clip.length;
		if (num > 0.42f && num < 0.5f)
		{
			float sqrMagnitude = (player.GetTransform().position - enemyTransform.position).sqrMagnitude;
			if (sqrMagnitude < bigDamageDistance * bigDamageDistance)
			{
				player.OnHit(drillBigDamage);
				CheckKnocked(drillBigKnockSpeed);
				touchtimer.Do();
			}
		}
		else if (num > 0.5f && num < 0.6f)
		{
			float sqrMagnitude2 = (player.GetTransform().position - enemyTransform.position).sqrMagnitude;
			if (sqrMagnitude2 < midDamageDistance * midDamageDistance)
			{
				player.OnHit(drillMidDamage);
				CheckKnocked(drillMidKnockSpeed);
				touchtimer.Do();
			}
		}
	}

	public void Drillin()
	{
		if (drillInTimer.Ready() && AnimationPlayed(AnimationString.EARTHWORM_DRILLIN, 0.2f))
		{
			drillInTimer.Do();
			PlaySoundSingle("Audio/enemy/Earthworm/ground_in");
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_disappare_001") as GameObject;
			Vector3 position = new Vector3(enemyTransform.position.x, 0.02f, enemyTransform.position.z) + 4.5f * enemyTransform.forward;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original, position, rotation);
		}
	}

	public void DrillIdle()
	{
		if (drillIdleTimer.Ready() && GetDrillIdleTimeDuration() > MaxDrillIdleTime - 3f)
		{
			drillIdleTimer.Do();
			PlaySoundSingle("Audio/enemy/Earthworm/ready_to_break");
			Vector3 vector = new Vector3(Random.Range(centerAreaMinX, centerAreaMaxX), 1f, Random.Range(centerAreaMinZ, centerAreaMaxZ));
			Vector3 vector2 = new Vector3(target.position.x, 1f, target.position.z);
			Vector3 vector3 = vector2 - vector;
			vector3.Normalize();
			ray = new Ray(vector, vector3);
			if (Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL)))
			{
				wallHitPosition = rayhit.point;
			}
			float sqrMagnitude = (vector2 - wallHitPosition).sqrMagnitude;
			if (sqrMagnitude < 100f)
			{
				enemyTransform.position = new Vector3(wallHitPosition.x, enemyTransform.position.y, wallHitPosition.z) - vector3 * 10f;
			}
			else
			{
				enemyTransform.position = new Vector3(target.position.x, enemyTransform.position.y, target.position.z);
			}
			enemyTransform.LookAt(new Vector3(wallHitPosition.x, enemyTransform.position.y, wallHitPosition.z));
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_appare_001") as GameObject;
			Vector3 position = new Vector3(enemyTransform.position.x, 0.01f, enemyTransform.position.z) - 12f * enemyTransform.forward;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original, position, rotation);
		}
	}

	public void DrillOut()
	{
		float num = animation[AnimationString.EARTHWORM_DRILLOUT].time / animation[AnimationString.EARTHWORM_DRILLOUT].clip.length;
		if (drillOutTimer.Ready() && num > 0.02f)
		{
			drillOutTimer.Do();
			PlaySoundSingle("Audio/enemy/Earthworm/ground_out1");
			GameObject original = Resources.Load("Effect/update_effect/efffect_worm_appare_002") as GameObject;
			Vector3 position = new Vector3(enemyTransform.position.x, 0.02f, enemyTransform.position.z) - 12f * enemyTransform.forward;
			Quaternion rotation = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original, position, rotation);
		}
		if (drillDownTimer.Ready() && num > 0.33f)
		{
			drillDownTimer.Do();
			PlaySoundSingle("Audio/enemy/Earthworm/ground_out2");
			GameObject original2 = Resources.Load("Effect/update_effect/efffect_worm_attack_003") as GameObject;
			Vector3 position2 = new Vector3(enemyTransform.position.x, 0.02f, enemyTransform.position.z);
			Quaternion rotation2 = Quaternion.EulerAngles(0f, Random.Range(0f, 360f), 0f);
			UnityEngine.Object.Instantiate(original2, position2, rotation2);
		}
		if (num > 0.5f)
		{
			animation[AnimationString.EARTHWORM_DRILLOUT].speed = 1.5f;
		}
	}

	public byte ChangeStateFar()
	{
		int num = Random.Range(0, 100);
		byte b = 0;
		if (num < Probability1)
		{
			SetState(Enemy.CATCHING_STATE);
			SetCatchingTimeNow();
			return 80;
		}
		if (num < Probability2)
		{
			SetState(CONTINUOUS_SHOT_STATE);
			return 84;
		}
		if (num < Probability3)
		{
			SetState(SHOT_STATE);
			return 83;
		}
		if (num < Probability4)
		{
			SetState(DRILLIN_STATE);
			return 85;
		}
		SetState(START_RUSH_STATE);
		return 82;
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
			if (GetSuperRushTimeDuration() > MaxSuperRushInterval && Random.Range(0, 100) < Probability5)
			{
				SetSuperRushTimeNow();
				SetState(START_SUPER_RUSH_STATE);
				b = 86;
				targetID = GetRandomPlayer();
			}
			else
			{
				float averagehorizontalDistance = GetAveragehorizontalDistance();
				if (averagehorizontalDistance < base.AttackRange)
				{
					SetState(NORMAL_ATTACK_STATE);
					b = 81;
					targetID = GetNearestPlayer();
				}
				else if (averagehorizontalDistance < startDrillDistance)
				{
					b = ChangeStateFar();
					switch (b)
					{
					case 80:
						targetID = GetNearestPlayer();
						break;
					case 82:
						targetID = GetFarthestPlayer();
						break;
					default:
						targetID = GetRandomPlayer();
						break;
					}
				}
				else
				{
					SetState(START_RUSH_STATE);
					b = 82;
					targetID = GetFarthestPlayer();
				}
			}
		}
		if (b != 0)
		{
			ChangeTargetPlayer(targetID);
		}
		switch (b)
		{
		case 82:
			StartRush();
			break;
		case 86:
			StartSuperRush();
			break;
		}
		switch (b)
		{
		case 81:
			LookAtTargetInNormalAttack();
			break;
		default:
			LookAtTarget();
			break;
		case 0:
			return;
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, targetID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void StopSoundOnHit()
	{
		StopSound("rage2");
		StopSound("attack_spit2");
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.DEAD_STATE)
		{
			hp -= dp.damage;
			Vector3 hitPoint = Vector3.zero;
			if (GetHitPoint(out hitPoint))
			{
				GameObject original = Resources.Load("Effect/update_effect/efffect_worm_blood_002") as GameObject;
				UnityEngine.Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if (state != DRILLIN_STATE && state != DRILL_IDLE_STATE && state != DRILLOUT_STATE && (float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(EARTHWORM_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Earthworm/hit");
				GameObject original2 = Resources.Load("Effect/update_effect/efffect_worm_blood_001") as GameObject;
				UnityEngine.Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
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
				GameObject original = Resources.Load("Effect/update_effect/efffect_worm_blood_002") as GameObject;
				UnityEngine.Object.Instantiate(original, hitPoint, Quaternion.identity);
			}
			if (state != DRILLIN_STATE && state != DRILL_IDLE_STATE && state != DRILLOUT_STATE && (float)(maxHp - hp) > (float)(currentHitTime * maxHp) * hpPercentagePerHit)
			{
				currentHitTime++;
				base.CanShot = true;
				SetState(EARTHWORM_GOTHIT_STATE);
				DisableAllEffect();
				StopSoundOnHit();
				PlaySoundSingle("Audio/enemy/Earthworm/hit");
				GameObject original2 = Resources.Load("Effect/update_effect/efffect_worm_blood_001") as GameObject;
				UnityEngine.Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
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
			SetState(NORMAL_ATTACK_STATE);
			b = 81;
			LookAtTargetInNormalAttack();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		return result;
	}

	protected override bool GetHitPoint(out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		Camera mainCamera = Camera.main;
		Transform transform = mainCamera.transform;
		ThirdPersonStandardCameraScript component = Camera.main.GetComponent<ThirdPersonStandardCameraScript>();
		Vector3 vector = mainCamera.ScreenToWorldPoint(new Vector3(component.ReticlePosition.x, (float)Screen.height - component.ReticlePosition.y, 0.1f));
		Vector3 vector2 = vector - transform.position;
		vector2.Normalize();
		Ray ray = new Ray(transform.position, vector2);
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point + vector2 * 1.4f + Vector3.down * 1.8f;
			return true;
		}
		ray = new Ray(player.GetTransform().position + Vector3.up * 0.5f, bipObject.transform.position + Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f) + Vector3.forward * Random.Range(-1f, 1f) - player.GetTransform().position - Vector3.up * 0.5f);
		hitInfo = default(RaycastHit);
		if (Physics.Raycast(ray, out hitInfo, 100f, 1 << PhysicsLayer.ENEMY))
		{
			hitPoint = hitInfo.point;
			return true;
		}
		return false;
	}
}
