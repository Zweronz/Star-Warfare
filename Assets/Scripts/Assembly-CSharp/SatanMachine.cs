using System.Collections.Generic;
using UnityEngine;

public class SatanMachine : EnemyBoss
{
	public static EnemyState SATANMACHINE_CATCHING_STATE = new SatanMachineCatchingState();

	public static EnemyState IDLE_A_STATE = new SatanMachineIdleA();

	public static EnemyState IDLE_B_STATE = new SatanMachineIdleB();

	public static EnemyState SHOT_GIFT_BOMB_STATE = new SatanMachineGiftBombState();

	public static EnemyState SHOT_LASER_STATE = new SatanMachineLaserState();

	public static EnemyState STOMP_STATE = new SatanMachineStompState();

	public static EnemyState LAUNCH_MISSILE_A_STATE = new SatanMachineMissileAState();

	public static EnemyState LAUNCH_MISSILE_B_STATE = new SatanMachineMissileBState();

	public static EnemyState START_RAGE_STATE = new SatanMachineRageState();

	public static EnemyState THROW_BALL_STATE = new SatanMachineThrowBallState();

	public static EnemyState WHIRLING_ATTACK_STATE = new SatanMachineWhirlingAttackState();

	public static EnemyState END_RAGE_STATE = new SatanMachineRageEndState();

	protected float lastIdleTime;

	protected float lastStompTime;

	protected float lastMissileTime;

	protected float lastLaserTime;

	protected float lastGiftBombTime;

	protected float[] maxIdleTime = new float[2];

	protected float stompInterval;

	protected float missileInterval;

	protected float laserInterval;

	protected float giftBombInterval;

	protected float startStompDistance;

	protected float startLaserDistance;

	protected int stompDamage;

	protected int whirlDamage;

	protected float stompKnockSpeed;

	protected float whirlKnockSpeed;

	protected int giftBombTotal;

	private int launchMissileEffectIndex;

	protected int launchMissileTimes;

	protected int launchMissileMaxTimes;

	protected int[] launchMissileTarget;

	protected int rageAttackTimes;

	protected int rageAttackMaxTimes;

	protected bool[] rageTimes;

	protected int rageMaxTime = 4;

	protected Vector3 laserTarget;

	protected GameObject[] launchMissileEffect;

	protected GameObject[] jokeBalls;

	public Collider whirlingAttackCollider;

	protected Transform rightHandFrontTrans;

	protected Transform leftHandFrontTrans;

	protected Transform rightHandBackTrans;

	protected Transform leftHandBackTrans;

	protected Transform mouseTrans;

	protected Transform bagTrans;

	protected Transform bodyCenterTrans;

	protected new bool attacked = true;

	protected float[] maxRunTime = new float[2];

	private Timer mPlayerHitTimer = new Timer();

	public bool IsDoShotLaser { get; set; }

	public bool IsDoStomp { get; set; }

	public bool[] IsDoLaunchMissile { get; set; }

	public bool IsDoGiftBomb { get; set; }

	public bool IsDoWhirlingAttackBegin { get; set; }

	public bool IsDoWhirlingAttackProcess { get; set; }

	public bool IsDoWhirlingAttackEnd { get; set; }

	public bool IsDoThrowBall { get; set; }

	public bool IsDoPlayReloadSound { get; set; }

	public float MaxIdleTime
	{
		get
		{
			return maxIdleTime[(int)mindState];
		}
	}

	public SatanMachine()
	{
		EnemyBoss.INIT_STATE = new SatanMachineInitState();
		hasShadow = false;
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		lastIdleTime = Time.time;
	}

	public override void OnDead()
	{
		enemyObject.layer = PhysicsLayer.DEADBODY;
		deadRemoveBodyTimer = new Timer();
		deadRemoveBodyTimer.SetTimer(3000f, false);
		PlayAnimation(AnimationString.SATANMACHINE_DEAD, WrapMode.ClampForever);
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_dead01") as GameObject;
		GameObject gameObject = Object.Instantiate(original, bodyCenterTrans.position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = bodyCenterTrans;
		StopSoundOnHit();
		PlaySound("Audio/enemy/SatanMachine/siwang");
		EnableGravity(true);
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
		bipObject = enemyObject;
		bodyCollider = enemyObject.transform.Find(BoneName.SatanMachineBody).gameObject.GetComponent<Collider>();
		whirlingAttackCollider = enemyObject.transform.Find(BoneName.SatanMachineWhirlingAttack).gameObject.GetComponent<Collider>();
		rightHandFrontTrans = enemyObject.transform.Find(BoneName.SatanMachineRightHandFront).gameObject.transform;
		rightHandBackTrans = enemyObject.transform.Find(BoneName.SatanMachineRightHandBack).gameObject.transform;
		leftHandFrontTrans = enemyObject.transform.Find(BoneName.SatanMachineLeftHandFront).gameObject.transform;
		leftHandBackTrans = enemyObject.transform.Find(BoneName.SatanMachineLeftHandBack).gameObject.transform;
		mouseTrans = enemyObject.transform.Find(BoneName.SatanMachineMouse).gameObject.transform;
		bagTrans = enemyObject.transform.Find(BoneName.SatanMachineBag).gameObject.transform;
		bodyCenterTrans = enemyObject.transform.Find(BoneName.SatanMachineBodyCenter).gameObject.transform;
		IsDoLaunchMissile = new bool[6];
		for (int i = 0; i < IsDoLaunchMissile.Length; i++)
		{
			IsDoLaunchMissile[i] = false;
		}
		rageTimes = new bool[rageMaxTime];
		for (int j = 0; j < rageTimes.Length; j++)
		{
			rageTimes[j] = false;
		}
	}

	protected override void loadParameters()
	{
		rageAttackMaxTimes = 2;
		giftBombTotal = 8;
		launchMissileMaxTimes = 6;
		launchMissileTarget = new int[launchMissileMaxTimes * 2];
		launchMissileEffect = new GameObject[launchMissileMaxTimes * 2];
		jokeBalls = new GameObject[giftBombTotal];
		mPlayerHitTimer.SetTimer(1f, true);
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_fire01") as GameObject;
		for (int i = 0; i < launchMissileEffect.Length; i++)
		{
			launchMissileEffect[i] = Object.Instantiate(original) as GameObject;
			launchMissileEffect[i].gameObject.SetActive(false);
		}
		original = Resources.Load("Effect/SatanMachine/SatanMachineBall") as GameObject;
		for (int j = 0; j < jokeBalls.Length; j++)
		{
			jokeBalls[j] = Object.Instantiate(original) as GameObject;
			jokeBalls[j].gameObject.SetActive(false);
		}
		startStompDistance = 15f;
		startLaserDistance = 20f;
		laserInterval = 30f;
		giftBombInterval = 60f;
		stompInterval = 15f;
		missileInterval = 10f;
		hp = 1200000;
		maxIdleTime[0] = 0.2f;
		maxCatchingTime[0] = 2f;
		maxRunTime[0] = 9f;
		maxIdleTime[1] = 0.1f;
		maxCatchingTime[1] = 2f;
		maxRunTime[1] = 11.5f;
		runSpeed = maxRunTime[0];
		turnSpeed = 0.1f;
		whirlDamage = 10000;
		stompDamage = 10000;
		touchDamage = 1000;
		whirlKnockSpeed = 0.3f;
		stompKnockSpeed = 0.04f;
		touchKnockSpeed = 0.15f;
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

	public void SetIdleTimeNow()
	{
		lastIdleTime = Time.time;
	}

	public float GetIdleTimeDuration()
	{
		return Time.time - lastIdleTime;
	}

	public override void OnIdle()
	{
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		byte b = 0;
		if (GetIdleTimeDuration() > MaxIdleTime)
		{
			if (attacked)
			{
				FindTarget();
				attacked = false;
				int num = Random.Range(0, 100);
				if (num < 50)
				{
					b = 96;
					SetState(IDLE_A_STATE);
				}
				else
				{
					b = 97;
					SetState(IDLE_B_STATE);
					IsDoPlayReloadSound = false;
				}
			}
			else if (IsCanRage())
			{
				attacked = true;
				ReadyToStartRage();
				b = 94;
			}
			else if (IsCanShotGiftBomb())
			{
				attacked = true;
				ReadyToShotGiftBomb();
				b = 92;
			}
			else if (IsCanStomp())
			{
				attacked = true;
				ReadyToStomp();
				b = 88;
			}
			else
			{
				int num2 = Random.Range(0, 100);
				if (num2 < 25 && IsCanLaunchMissile())
				{
					attacked = true;
					num2 = Random.Range(0, 100);
					if (num2 < 50)
					{
						ReadyToLaunchMissileA();
						b = 89;
					}
					else
					{
						ReadyToLaunchMissileB();
						b = 90;
					}
				}
				else if (num2 >= 25 && num2 < 50 && IsCanShotLaser())
				{
					attacked = true;
					ReadyToShotLaser();
					b = 91;
				}
				else
				{
					SearchNearestPlayer();
					b = 87;
				}
			}
		}
		int userID = GetTargetPlayer().GetUserID();
		if (b != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, userID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected bool IsCanRage()
	{
		int num = hp * (rageMaxTime + 1) / maxHp;
		if (num < rageMaxTime && !rageTimes[num])
		{
			rageTimes[num] = true;
			if (state == START_RAGE_STATE || state == WHIRLING_ATTACK_STATE || state == THROW_BALL_STATE || state == END_RAGE_STATE)
			{
				return false;
			}
			FindTarget();
			return true;
		}
		return false;
	}

	public void ReadyToStartRage()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_change") as GameObject;
		GameObject gameObject = Object.Instantiate(original, bodyCenterTrans.position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = bodyCenterTrans;
		PlaySound("Audio/enemy/SatanMachine/xiaochouxiaosheng");
		SetState(START_RAGE_STATE);
		rageAttackTimes = 0;
		runSpeed = maxRunTime[1];
	}

	public void ReadyToEndRage()
	{
		Debug.Log("Rage End");
		SetState(END_RAGE_STATE);
		runSpeed = maxRunTime[0];
	}

	public bool IsRageEnd()
	{
		return rageAttackTimes >= rageAttackMaxTimes;
	}

	public void ReadyToWhirlingAttack()
	{
		SetState(WHIRLING_ATTACK_STATE);
		IsDoWhirlingAttackBegin = false;
		IsDoWhirlingAttackProcess = false;
		IsDoWhirlingAttackEnd = false;
	}

	public void ReadyToThrowBall()
	{
		LookAtTarget();
		SetState(THROW_BALL_STATE);
		rageAttackTimes++;
		IsDoThrowBall = false;
	}

	public void ThrowBall()
	{
		PlaySound("Audio/enemy/SatanMachine/kongqipaofashe");
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		Vector3 position = bagTrans.position;
		List<Vector3> list = new List<Vector3>();
		List<short> list2 = new List<short>();
		for (int i = 0; i < giftBombTotal; i++)
		{
			float num = 0f;
			float num2 = 0f;
			if (Random.Range(1, 3) == 1)
			{
				num = Random.Range(6, 12) * ((Random.Range(1, 3) != 1) ? 1 : (-1));
				num2 = Random.Range(-12, 12);
			}
			else
			{
				num = Random.Range(-12, 12);
				num2 = Random.Range(6, 12) * ((Random.Range(1, 3) != 1) ? 1 : (-1));
			}
			Vector3 item = new Vector3(enemyTransform.forward.x * num, 4f, enemyTransform.forward.z * num2);
			list.Add(item);
			list2.Add(0);
		}
		ThrowBall(position, list, list2);
		SatanMachineGiftBombRequest request = new SatanMachineGiftBombRequest(base.EnemyID, 94, position, list, list2);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void ThrowBall(Vector3 launchPos, List<Vector3> forceList, List<short> typeList)
	{
		Debug.Log("*******************************");
		for (int i = 0; i < forceList.Count; i++)
		{
			Debug.Log("Throw : " + i);
			GameObject[] array = jokeBalls;
			foreach (GameObject gameObject in array)
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
					gameObject.transform.position = launchPos;
					gameObject.transform.rotation = Quaternion.identity;
					gameObject.GetComponent<Rigidbody>().AddForce(forceList[i], ForceMode.Impulse);
					SatanMachineBall componentInChildren = gameObject.GetComponentInChildren<SatanMachineBall>();
					componentInChildren.targetPlayer = player;
					componentInChildren.enemy = this;
					break;
				}
			}
		}
	}

	protected bool IsCanStomp()
	{
		if (Time.time - lastStompTime < stompInterval)
		{
			return false;
		}
		float nearestDistanceToPlayer = GetNearestDistanceToPlayer();
		if (nearestDistanceToPlayer < startStompDistance)
		{
			ChangeTargetPlayer(GetNearestPlayer());
			return true;
		}
		return false;
	}

	public void ReadyToStomp()
	{
		LookAtTarget();
		SetState(STOMP_STATE);
		IsDoStomp = false;
	}

	public void ResetStomp()
	{
		lastStompTime = Time.time;
	}

	public void CheckStomp()
	{
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_punch") as GameObject;
		Object.Instantiate(original, GetPosition(), Quaternion.identity);
		PlaySound("Audio/enemy/SatanMachine/jixiebichuidigongji");
		Vector3 position = new Vector3(GetPosition().x, 0f, GetPosition().z) + Vector3.up;
		Collider[] array = Physics.OverlapSphere(position, startStompDistance, 1 << PhysicsLayer.PLAYER);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.gameObject.layer == PhysicsLayer.PLAYER)
			{
				player.OnHit(stompDamage);
				CheckKnocked(stompKnockSpeed);
			}
		}
	}

	protected bool IsCanLaunchMissile()
	{
		if (Time.time - lastMissileTime < missileInterval)
		{
			return false;
		}
		return true;
	}

	protected void ReadyToLaunchMissile()
	{
		LookAtTarget();
		for (int i = 0; i < IsDoLaunchMissile.Length; i++)
		{
			IsDoLaunchMissile[i] = false;
		}
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		int num = launchMissileTarget.Length / (1 + remotePlayers.Count);
		for (int j = 0; j < launchMissileTarget.Length - num; j++)
		{
			launchMissileTarget[j] = remotePlayers[j / num].GetUserID();
		}
		for (int k = launchMissileTarget.Length - num; k < launchMissileTarget.Length; k++)
		{
			launchMissileTarget[k] = player.GetUserID();
		}
	}

	public void ReadyToLaunchMissileA()
	{
		SetState(LAUNCH_MISSILE_A_STATE);
		ReadyToLaunchMissile();
	}

	public void ReadyToLaunchMissileB()
	{
		SetState(LAUNCH_MISSILE_B_STATE);
		ReadyToLaunchMissile();
	}

	public void ResetMissile()
	{
		launchMissileTimes = 0;
		lastMissileTime = Time.time;
	}

	public void LaunchMissile()
	{
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && launchMissileTimes < launchMissileMaxTimes)
		{
			LaunchMissile(0, launchMissileTarget[launchMissileTimes * 2]);
			LaunchMissile(1, launchMissileTarget[launchMissileTimes * 2 + 1]);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 93, 0, launchMissileTarget[launchMissileTimes * 2]);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				request = new EnemyStateRequest(base.EnemyID, 93, 1, launchMissileTarget[launchMissileTimes * 2 + 1]);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			launchMissileTimes++;
			PlaySound("Audio/rpg/rpg-21_FiringSound");
		}
	}

	public void LaunchMissile(int missileIndex, int targetId)
	{
		Vector3 position = ((missileIndex % 2 != 0) ? leftHandFrontTrans.position : rightHandFrontTrans.position);
		Vector3 normalized = ((missileIndex % 2 != 0) ? (leftHandFrontTrans.position - leftHandBackTrans.position) : (rightHandFrontTrans.position - rightHandBackTrans.position)).normalized;
		if (launchMissileEffectIndex < 2)
		{
			for (int i = 0; i < launchMissileEffect.Length; i++)
			{
				if (!launchMissileEffect[i].gameObject.activeSelf)
				{
					launchMissileEffect[i].gameObject.SetActive(true);
					launchMissileEffect[i].gameObject.transform.position = position;
					launchMissileEffect[i].gameObject.transform.rotation = Quaternion.identity;
					break;
				}
			}
		}
		launchMissileEffectIndex = ((launchMissileEffectIndex + 1 < 4) ? (launchMissileEffectIndex + 1) : 0);
		GameObject original = Resources.Load("Effect/SatanMachine/SatanMachineMissile") as GameObject;
		GameObject gameObject = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
		TrackingMissileScript component = gameObject.GetComponent<TrackingMissileScript>();
		if (player.GetUserID() == targetId)
		{
			component.trackingPlayer = player;
		}
		else
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(targetId);
			if (remotePlayerByUserID != null)
			{
				component.trackingPlayer = remotePlayerByUserID;
			}
			else
			{
				component.trackingPlayer = player;
			}
		}
		component.initDir = normalized;
		component.localPlayer = player;
	}

	protected bool IsCanShotGiftBomb()
	{
		if (Time.time - lastGiftBombTime < giftBombInterval)
		{
			return false;
		}
		return true;
	}

	public void ReadyToShotGiftBomb()
	{
		LookAtTarget();
		SetState(SHOT_GIFT_BOMB_STATE);
		IsDoGiftBomb = false;
	}

	public void ShotGiftBomb()
	{
		PlaySound("Audio/enemy/SatanMachine/kongqipaofashe");
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		Vector3 position = bagTrans.position;
		List<Vector3> list = new List<Vector3>();
		List<short> list2 = new List<short>();
		for (int i = 0; i < giftBombTotal; i++)
		{
			float num = 0f;
			float num2 = 0f;
			if (Random.Range(1, 3) == 1)
			{
				num = Random.Range(6, 12) * ((Random.Range(1, 3) != 1) ? 1 : (-1));
				num2 = Random.Range(-12, 12);
			}
			else
			{
				num = Random.Range(-12, 12);
				num2 = Random.Range(6, 12) * ((Random.Range(1, 3) != 1) ? 1 : (-1));
			}
			Vector3 item = new Vector3(enemyTransform.forward.x * num, 4f, enemyTransform.forward.z * num2);
			list.Add(item);
			list2.Add((short)Random.Range(0, 4));
		}
		ShotGiftBomb(position, list, list2);
		SatanMachineGiftBombRequest request = new SatanMachineGiftBombRequest(base.EnemyID, 89, position, list, list2);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}

	public void ShotGiftBomb(Vector3 launchPos, List<Vector3> forceList, List<short> typeList)
	{
		for (int i = 0; i < forceList.Count; i++)
		{
			GameObject original = Resources.Load("Effect/SatanMachine/SatanMachineGiftBomb" + Random.Range(1, 5)) as GameObject;
			GameObject gameObject = Object.Instantiate(original, launchPos, Quaternion.identity) as GameObject;
			gameObject.GetComponent<Rigidbody>().AddForce(forceList[i], ForceMode.Impulse);
			SatanMachineGift componentInChildren = gameObject.GetComponentInChildren<SatanMachineGift>();
			componentInChildren.targetPlayer = player;
			componentInChildren.enemy = this;
		}
	}

	public void ResetGiftBomb()
	{
		lastGiftBombTime = Time.time;
	}

	protected bool IsCanShotLaser()
	{
		if (Time.time - lastLaserTime < laserInterval)
		{
			return false;
		}
		float nearestDistanceToPlayer = GetNearestDistanceToPlayer();
		if (nearestDistanceToPlayer < startLaserDistance)
		{
			ChangeTargetPlayer(GetNearestPlayer());
			return true;
		}
		return false;
	}

	public void ReadyToShotLaser()
	{
		PlaySound("Audio/enemy/SatanMachine/kongqipaofashe");
		PlaySound("Audio/enemy/SatanMachine/laser");
		SetState(SHOT_LASER_STATE);
		IsDoShotLaser = false;
		LookAtTarget();
		laserTarget = GetTargetPlayer().GetTransform().position;
		GameObject original = Resources.Load("Effect/SatanMachine/xmas_beam01") as GameObject;
		GameObject gameObject = Object.Instantiate(original, mouseTrans.position, Quaternion.identity) as GameObject;
		gameObject.transform.parent = mouseTrans;
	}

	public void ShotLaser()
	{
		Vector3 position = mouseTrans.position;
		Quaternion rotation = enemyTransform.rotation;
		GameObject original = Resources.Load("Effect/SatanMachine/SatanMachineLaser") as GameObject;
		GameObject gameObject = Object.Instantiate(original, position, rotation) as GameObject;
		SatanMachineLaserScript componentInChildren = gameObject.GetComponentInChildren<SatanMachineLaserScript>();
		componentInChildren.SetTargetPlayer(GetTargetPlayer(), laserTarget);
		componentInChildren.mEnemy = this;
	}

	public void ResetLaser()
	{
		lastLaserTime = Time.time;
	}

	public void SearchNearestPlayer()
	{
		SetState(SATANMACHINE_CATCHING_STATE);
	}

	public bool CheckWhirlingAttackHit()
	{
		bool flag = false;
		if (state != Enemy.DEAD_STATE && player.State != Player.KNOCKED_STATE)
		{
			flag = CheckHit(whirlingAttackCollider, whirlDamage, whirlKnockSpeed);
		}
		if (!flag)
		{
			flag = CheckHitRemotePlayer(whirlingAttackCollider);
		}
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && flag)
		{
			int farthestPlayer = GetFarthestPlayer();
			ChangeTargetPlayer(farthestPlayer);
			EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 98, GetTransform().position, farthestPlayer);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
		return flag;
	}

	private bool CheckHit(Collider collider, int damage, float knockSpeed)
	{
		if (collider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(damage);
			CheckKnocked(knockSpeed);
			return true;
		}
		return false;
	}

	private bool CheckHitRemotePlayer(Collider collider)
	{
		List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (collider.bounds.Intersects(item.GetCollider().bounds))
			{
				return true;
			}
		}
		return false;
	}

	protected new void CheckKnocked(float speed)
	{
		Vector3 from = new Vector3(player.GetTransform().forward.x, 0f, player.GetTransform().forward.z);
		Vector3 to = enemyTransform.position - player.GetTransform().position;
		to.y = 0f;
		float f = Vector3.Angle(from, to);
		if (Mathf.Abs(f) > 90f)
		{
			speed = 0f - speed;
		}
		player.OnKnocked(speed);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			PlayerOnKnockedRequest request = new PlayerOnKnockedRequest(speed);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public override EnemyState EnterSpecialState(float deltaTime)
	{
		if (GetCatchingration() > base.MaxCatchingTime)
		{
			SetCatchingTimeNow();
			return Enemy.IDLE_STATE;
		}
		return state;
	}

	public override void TouchPlayer()
	{
		if (state != Enemy.DEAD_STATE && player.State != Player.KNOCKED_STATE && mPlayerHitTimer.Ready())
		{
			mPlayerHitTimer.Do();
			CheckHit(bodyCollider, touchDamage, touchKnockSpeed);
		}
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.GRAVEBORN_STATE && state != Enemy.DEAD_STATE)
		{
			beWokeUp = true;
			hp -= dp.damage;
			if (base.IsElite)
			{
				criticalAttacked = false;
			}
			else
			{
				criticalAttacked = dp.criticalAttack;
			}
		}
	}

	public override void OnHitResponse()
	{
		beWokeUp = true;
	}
}
