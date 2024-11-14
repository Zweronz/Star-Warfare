using UnityEngine;

public class CoopMantis : Mantis
{
	public enum TargetPointType
	{
		LEFT_MID = 0,
		RIGHT_MID = 1,
		LEFT_TOP = 2,
		RIGHT_TOP = 3,
		LEFT_BOTTOM = 4,
		RIGHT_BOTTOM = 5,
		NUM = 6
	}

	protected const float FLY_HEIGHT_PLATFORM = 8.5f;

	public static EnemyState JOIN_START_FLY_STATE = new CoopMantisJoinStartFlyState();

	public static EnemyState COOP_START_FLY_STATE = new CoopMantisStartFlyState();

	public static EnemyState COOP_FLY_STATE = new CoopMantisFlyState();

	public static EnemyState COOP_FLY_IDLE_STATE = new CoopMantisFlyIdleState();

	public static EnemyState COOP_FLY_LASER_STATE = new CoopMantisFlyLaserState();

	public static EnemyState COOP_FLY_SHOT_STATE = new CoopMantisFlyShotState();

	public static EnemyState COOP_FLY_START_DIVE_STATE = new CoopMantisFlyStartDiveState();

	public static EnemyState COOP_FLY_DIVE_STATE = new CoopMantisFlyDiveState();

	public static EnemyState COOP_FLY_DIVE_END_STATE = new CoopMantisFlyDiveEndState();

	protected TargetPointType mCurrentPoint;

	protected TargetPointType mTargetPoint;

	protected CoopMantis mCoopMantis;

	protected bool mIsInDefent;

	protected Collider mHitCheckCollider;

	protected GameObject defentObj;

	protected FadeInAlphaAnimationScript[] defentScript = new FadeInAlphaAnimationScript[3];

	protected DefentOneByOneScript[] defentOnebyOneScript = new DefentOneByOneScript[3];

	protected Timer showDefentTimer = new Timer();

	protected Vector3[] mTargetPoints;

	protected float mLastCoopFlyTime;

	protected float mLastCoopIdleTime;

	protected float mLastDefentTime;

	protected float mCoopDiveDistance;

	protected float mMaxDoubleSingleTime;

	protected float mMaxCoopIdleTime;

	protected float mMaxDefentTime;

	protected float mMaxCoopFlyTime;

	protected int mMaxCoopAttackTimes;

	protected int mMaxCoopDiveTimes;

	protected float mCoopHookSpeed;

	public bool CanDive { get; set; }

	public int CoopAttackCount { get; set; }

	public int CoopDiveCount { get; set; }

	public bool NeedChangeToCoop { get; set; }

	public bool NeedRageForCoopDead { get; set; }

	public string InitAniamtion { get; set; }

	public float InitIdleTime { get; set; }

	public float SeePlayerIdleTime { get; set; }

	public TargetPointType CoopTargetPoint
	{
		get
		{
			return mTargetPoint;
		}
	}

	public float MaxCoopFlyTime
	{
		get
		{
			return mMaxCoopFlyTime;
		}
	}

	public virtual void SetDoubleSingleTimeNow()
	{
	}

	public virtual float GetDoubleSingleTimeDuration()
	{
		return 0f;
	}

	public void SetCoopIdleTimeNow()
	{
		mLastCoopIdleTime = Time.time;
	}

	public float GetCoopIdleDuration()
	{
		return Time.time - mLastCoopIdleTime;
	}

	public void SetCoopFlyTimeNow()
	{
		mLastCoopFlyTime = Time.time;
	}

	public float GetCoopFlyDuration()
	{
		return Time.time - mLastCoopFlyTime;
	}

	public void SetDefentTimeNow()
	{
		mLastDefentTime = Time.time;
	}

	public float GetDefentDuration()
	{
		return Time.time - mLastDefentTime;
	}

	public void SetCoopMantis(CoopMantis enemy)
	{
		mCoopMantis = enemy;
	}

	public CoopMantis GetCoopMantis()
	{
		return mCoopMantis;
	}

	public virtual void AddCashAndExp()
	{
	}

	public void SetTargetPoint(TargetPointType type)
	{
		mTargetPoint = type;
		LookAtPoint(new Vector3(mTargetPoints[(int)mTargetPoint].x, enemyTransform.position.y, mTargetPoints[(int)mTargetPoint].z));
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		mLastCoopIdleTime = Time.time;
		mLastDefentTime = Time.time;
		mLastCoopFlyTime = Time.time;
	}

	public void InitCoopAttackCount()
	{
		CoopAttackCount = mMaxCoopAttackTimes;
	}

	public void DecreaseCoopAttackCount()
	{
		CoopAttackCount--;
	}

	public void ClearCoopAttackCount()
	{
		CoopAttackCount = 0;
	}

	public void InitCoopDiveCount()
	{
		CoopDiveCount = mMaxCoopDiveTimes;
	}

	public void DecreaseCoopDiveCount()
	{
		CoopDiveCount--;
	}

	public void ClearCoopDiveCount()
	{
		CoopDiveCount = 0;
	}

	public void SetCoopDiveAttackTargetPoint()
	{
		switch (mCurrentPoint)
		{
		case TargetPointType.LEFT_TOP:
			SetTargetPoint(TargetPointType.RIGHT_TOP);
			break;
		case TargetPointType.RIGHT_TOP:
			SetTargetPoint(TargetPointType.RIGHT_BOTTOM);
			break;
		case TargetPointType.RIGHT_BOTTOM:
			SetTargetPoint(TargetPointType.LEFT_BOTTOM);
			break;
		case TargetPointType.LEFT_BOTTOM:
			SetTargetPoint(TargetPointType.LEFT_TOP);
			break;
		}
	}

	public void SetCoopAttackTargetPoint(bool isLeftToTop)
	{
		switch (mCurrentPoint)
		{
		case TargetPointType.LEFT_TOP:
			SetTargetPoint(TargetPointType.RIGHT_MID);
			break;
		case TargetPointType.LEFT_BOTTOM:
			SetTargetPoint(TargetPointType.RIGHT_MID);
			break;
		case TargetPointType.RIGHT_TOP:
			SetTargetPoint(TargetPointType.LEFT_MID);
			break;
		case TargetPointType.RIGHT_BOTTOM:
			SetTargetPoint(TargetPointType.LEFT_MID);
			break;
		case TargetPointType.LEFT_MID:
			if (isLeftToTop)
			{
				SetTargetPoint(TargetPointType.LEFT_TOP);
			}
			else
			{
				SetTargetPoint(TargetPointType.LEFT_BOTTOM);
			}
			break;
		case TargetPointType.RIGHT_MID:
			if (isLeftToTop)
			{
				SetTargetPoint(TargetPointType.RIGHT_BOTTOM);
			}
			else
			{
				SetTargetPoint(TargetPointType.RIGHT_TOP);
			}
			break;
		}
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		mHitCheckCollider = enemyObject.transform.FindChild(BoneName.MantisHitSphere).gameObject.collider;
		mTargetPoints = new Vector3[6];
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.BOSS_ASSIST_TARGET);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			switch (gameObject.name)
			{
			case "LeftMid":
				mTargetPoints[0] = gameObject.transform.position;
				break;
			case "RightMid":
				mTargetPoints[1] = gameObject.transform.position;
				break;
			case "LeftTop":
				mTargetPoints[2] = gameObject.transform.position;
				break;
			case "RightTop":
				mTargetPoints[3] = gameObject.transform.position;
				break;
			case "LeftBottom":
				mTargetPoints[4] = gameObject.transform.position;
				break;
			case "RightBottom":
				mTargetPoints[5] = gameObject.transform.position;
				break;
			}
		}
		CanDive = true;
		ClearCoopAttackCount();
		ClearCoopDiveCount();
		NeedChangeToCoop = false;
		NeedRageForCoopDead = false;
		CreateDefent();
	}

	protected override void loadParameters()
	{
		base.loadParameters();
		mMaxDoubleSingleTime = 24f;
		mMaxCoopIdleTime = 0.1f;
		mMaxCoopFlyTime = 5f;
		mMaxDefentTime = 30f;
		mCoopDiveDistance = 20f;
		mMaxCoopAttackTimes = 3;
		mMaxCoopDiveTimes = 1;
		mCoopHookSpeed = 16f;
	}

	public override void OnDead()
	{
		base.OnDead();
		mCoopMantis.CanDive = true;
		mCoopMantis.ClearCoopAttackCount();
		mCoopMantis.ClearCoopDiveCount();
		mCoopMantis.NeedRageForCoopDead = true;
	}

	protected override void ChangeStateAfterHit()
	{
		SetDoubleSingleTimeNow();
		ClearCoopAttackCount();
		ClearCoopDiveCount();
		mCoopMantis.ClearCoopAttackCount();
		mCoopMantis.ClearCoopDiveCount();
	}

	public void StartFlyHigh()
	{
		if (mantisflyAudioTimer.Ready())
		{
			PlaySound("Audio/enemy/Mantis/feixingtanglang_qifei");
			mantisflyAudioTimer.Do();
		}
		FlyUpHigh();
	}

	public void FlyUpHigh()
	{
		if (enemyTransform.position.y < 8.5f)
		{
			enemyTransform.Translate(Vector3.up * 2f * 8.5f * Time.deltaTime);
		}
		else
		{
			enemyTransform.position = new Vector3(enemyTransform.position.x, 8.5f, enemyTransform.position.z);
		}
	}

	public byte GoToTargetPoint()
	{
		byte b = 0;
		float num = Vector3.Distance(new Vector3(enemyTransform.position.x, 0f, enemyTransform.position.y), mTargetPoints[(int)mTargetPoint]);
		if (num < mCoopDiveDistance)
		{
			SetState(COOP_FLY_STATE);
			SetCoopFlyTimeNow();
			b = 72;
		}
		else
		{
			SetState(COOP_FLY_START_DIVE_STATE);
			b = 75;
			base.DiveTargetPos = mTargetPoints[(int)mTargetPoint];
		}
		return b;
	}

	public void CoopFly()
	{
		CharacterController characterController = enemyObject.collider as CharacterController;
		if (characterController != null)
		{
			Vector3 vector = enemyTransform.forward * flySpeed;
			characterController.Move(vector * Time.deltaTime);
		}
		PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fly_walk");
	}

	public bool CloseToTargetPoint()
	{
		Vector3 vector = mTargetPoints[(int)mTargetPoint] - enemyTransform.position;
		vector.y = 0f;
		if (vector.sqrMagnitude < 9f)
		{
			mCurrentPoint = mTargetPoint;
			return true;
		}
		return false;
	}

	public override void OnIdle()
	{
		if (NeedRageForCoopDead)
		{
			SetState(Mantis.RAGE_STATE);
			OnRage();
		}
		else
		{
			if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
			{
				return;
			}
			byte b = 0;
			byte b2 = 0;
			if (NeedChangeToCoop)
			{
				if (mCoopMantis != null)
				{
					if (mCoopMantis.GetState() == Enemy.IDLE_STATE)
					{
						SetState(COOP_START_FLY_STATE);
						NeedChangeToCoop = false;
						b = 71;
						mCoopMantis.SetState(COOP_START_FLY_STATE);
						mCoopMantis.NeedChangeToCoop = false;
						b2 = 71;
					}
					else if (mCoopMantis.GetState() == Mantis.FLY_IDLE_STATE)
					{
						SetState(COOP_START_FLY_STATE);
						NeedChangeToCoop = false;
						b = 71;
						b2 = mCoopMantis.GoToTargetPoint();
						mCoopMantis.NeedChangeToCoop = false;
					}
					else if (mCoopMantis.GetState() == AssistMantis.ASSIST_IDLE_STATE)
					{
						SetState(COOP_START_FLY_STATE);
						NeedChangeToCoop = false;
						b = 71;
						mCoopMantis.SetState(JOIN_START_FLY_STATE);
						mCoopMantis.NeedChangeToCoop = false;
						mCoopMantis.DisableWallDefent();
						b2 = 70;
					}
					if (b != 0 && b2 != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, (byte)mTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						request = new EnemyStateRequest(mCoopMantis.EnemyID, b2, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
				return;
			}
			if (!CanDive && GetDoubleSingleTimeDuration() > mMaxDoubleSingleTime)
			{
				InitCoopAttackCount();
				if (mCoopMantis != null)
				{
					mCoopMantis.InitCoopAttackCount();
					if (mCoopMantis.GetState() == Enemy.IDLE_STATE)
					{
						SetState(COOP_START_FLY_STATE);
						NeedChangeToCoop = false;
						b = 71;
						mCoopMantis.SetState(COOP_START_FLY_STATE);
						mCoopMantis.NeedChangeToCoop = false;
						b2 = 71;
					}
					else if (mCoopMantis.GetState() == Mantis.FLY_IDLE_STATE)
					{
						SetState(COOP_START_FLY_STATE);
						NeedChangeToCoop = false;
						b = 71;
						b2 = mCoopMantis.GoToTargetPoint();
						mCoopMantis.NeedChangeToCoop = false;
					}
					if (b != 0 && b2 != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, (byte)mTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						request2 = new EnemyStateRequest(mCoopMantis.EnemyID, b2, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
				return;
			}
			int targetID = GetTargetPlayer().GetUserID();
			if (GetLandingTimeDuration() < base.MaxGroundTime)
			{
				if (GetGroundIdleDuration() > base.MaxGroundIdleTime)
				{
					float averagehorizontalDistance = GetAveragehorizontalDistance();
					if (averagehorizontalDistance < base.AttackRange)
					{
						targetID = GetNearestPlayer();
						ChangeTargetPlayer(targetID);
						float nearestDistanceToTargetPlayer = GetNearestDistanceToTargetPlayer();
						if (nearestDistanceToTargetPlayer < groupAttackDistance)
						{
							SetState(Mantis.CRITICAL_ATTACK_STATE);
							b = 12;
						}
						else
						{
							int num = Random.Range(0, 100);
							if (num < base.Probability4)
							{
								SetState(Mantis.NORMAL_ATTACK_STATE);
								b = 11;
							}
							else
							{
								SetState(Mantis.CRITICAL_ATTACK_STATE);
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
					else if (CanDive)
					{
						SetState(Mantis.START_FLY_STATE);
						b = 13;
						targetID = GetFarthestPlayer();
					}
					else
					{
						SetState(Enemy.CATCHING_STATE);
						SetCatchingTimeNow();
						b = 10;
						targetID = GetNearestPlayer();
					}
				}
			}
			else
			{
				SetState(Mantis.START_FLY_STATE);
				b = 13;
				targetID = GetFarthestPlayer();
			}
			if (b != 0)
			{
				ChangeTargetPlayer(targetID);
				LookAtTarget();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request3 = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, targetID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
			}
		}
	}

	public virtual void CoopFlyIdle()
	{
	}

	public override void FlyIdle()
	{
		FlySound();
		byte b = 0;
		byte b2 = 0;
		if (NeedRageForCoopDead)
		{
			base.NeedLandToRage = true;
			SetState(Mantis.LANDING_STATE);
			EnableGravity(true);
		}
		else
		{
			if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
			{
				return;
			}
			if (NeedChangeToCoop)
			{
				if (mCoopMantis.GetState() == Enemy.IDLE_STATE)
				{
					b = GoToTargetPoint();
					NeedChangeToCoop = false;
					mCoopMantis.SetState(COOP_START_FLY_STATE);
					mCoopMantis.NeedChangeToCoop = false;
					b2 = 71;
				}
				else if (mCoopMantis.GetState() == Mantis.FLY_IDLE_STATE)
				{
					b = GoToTargetPoint();
					NeedChangeToCoop = false;
					b2 = mCoopMantis.GoToTargetPoint();
					mCoopMantis.NeedChangeToCoop = false;
				}
				else if (mCoopMantis.GetState() == AssistMantis.ASSIST_IDLE_STATE)
				{
					b = GoToTargetPoint();
					NeedChangeToCoop = false;
					mCoopMantis.SetState(JOIN_START_FLY_STATE);
					mCoopMantis.NeedChangeToCoop = false;
					mCoopMantis.DisableWallDefent();
					b2 = 70;
				}
				if (b != 0 && b2 != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, (byte)mTargetPoint);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					request = new EnemyStateRequest(mCoopMantis.EnemyID, b2, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else if (!CanDive && GetDoubleSingleTimeDuration() > mMaxDoubleSingleTime)
			{
				InitCoopAttackCount();
				if (mCoopMantis != null)
				{
					mCoopMantis.InitCoopAttackCount();
					if (mCoopMantis.GetState() == Enemy.IDLE_STATE)
					{
						b = GoToTargetPoint();
						NeedChangeToCoop = false;
						mCoopMantis.SetState(COOP_START_FLY_STATE);
						mCoopMantis.NeedChangeToCoop = false;
						b2 = 71;
					}
					else if (mCoopMantis.GetState() == Mantis.FLY_IDLE_STATE)
					{
						b = GoToTargetPoint();
						NeedChangeToCoop = false;
						b2 = mCoopMantis.GoToTargetPoint();
						mCoopMantis.NeedChangeToCoop = false;
					}
					if (b != 0 && b2 != 0 && GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, (byte)mTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						request2 = new EnemyStateRequest(mCoopMantis.EnemyID, b2, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
			else
			{
				if (isAllPlayerDead())
				{
					return;
				}
				int targetID = GetTargetPlayer().GetUserID();
				if (GetFlyTimeDuration() < base.MaxFlyTime)
				{
					if (GetCoopIdleDuration() > mMaxCoopIdleTime)
					{
						float averagehorizontalDistance = GetAveragehorizontalDistance();
						if (averagehorizontalDistance < base.FlyAttackRange)
						{
							b = ChangeStateInFlyNear();
							targetID = GetNearestPlayer();
						}
						else if (averagehorizontalDistance < base.StartDiveDistance)
						{
							b = ChangeStateInFlyFar();
							targetID = ((b != 15) ? GetRandomPlayer() : GetNearestPlayer());
						}
						else
						{
							int num = Random.Range(0, 100);
							if (num < base.Probability3)
							{
								SetState(Mantis.FLY_BOOMERANG_STATE);
								b = 19;
								targetID = GetRandomPlayer();
								StartFlyBoomerang();
							}
							else if (CanDive)
							{
								SetState(Mantis.FLY_DIVESTART_STATE);
								b = 21;
								targetID = GetFarthestPlayer();
							}
							else
							{
								SetState(Mantis.FLY_BOOMERANG_STATE);
								b = 19;
								targetID = GetRandomPlayer();
								StartFlyBoomerang();
							}
						}
					}
				}
				else
				{
					SetState(Mantis.LANDING_STATE);
					b = 18;
					targetID = GetNearestPlayer();
				}
				if (b != 0)
				{
					ChangeTargetPlayer(targetID);
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request3 = new EnemyStateRequest(base.EnemyID, b, GetTransform().position, targetID);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
					}
				}
			}
		}
	}

	public void CoopFlyShot()
	{
		GameObject original = Resources.Load("Effect/Mantis/sfx_blade") as GameObject;
		Vector3 position = enemyTransform.FindChild(BoneName.MantisLeftArm).position;
		position.y -= 1f;
		GameObject[] array = new GameObject[7];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Object.Instantiate(original, position, Quaternion.identity) as GameObject;
			array[i].transform.LookAt(new Vector3(targetToLookAt.x, 0f, targetToLookAt.z));
			array[i].transform.RotateAround(array[i].transform.position, Vector3.up, (float)(3 - i) * 25f);
			MantisHookScript component = array[i].GetComponent<MantisHookScript>();
			Vector3 forward = array[i].transform.forward;
			forward.y *= 2f;
			forward.Normalize();
			component.speed = forward * mCoopHookSpeed;
			component.attackDamage = shotDamage;
		}
		PlaySoundSingle("Audio/enemy/Mantis/tanglang_fly_attack01");
	}

	public void CoopFlyLaserStart()
	{
		Transform transform = enemyTransform.FindChild(BoneName.MantisMouth);
		Vector3 worldPosition = new Vector3(targetToLookAt.x, 0.5f, targetToLookAt.z);
		mantisLaserObj.transform.position = transform.position;
		mantisLaserObj.transform.Translate(0.1f * Vector3.down);
		mantisLaserObj.transform.LookAt(worldPosition);
		mantisLaserObj.transform.RotateAround(transform.transform.position, Vector3.up, 80f);
		mantisLaserObj.SetActiveRecursively(true);
		MantisLaserScript component = mantisLaserObj.GetComponent<MantisLaserScript>();
		component.attackDamage = laserDamage;
		component.speed = -130f;
	}

	public void OnCoopDive()
	{
		base.DiveTargetPos = mTargetPoints[(int)mTargetPoint];
		dir = (base.DiveTargetPos - enemyTransform.position).normalized;
		EnableTrailEffect(true);
	}

	public void LookAtTargetPoint()
	{
		LookAtPoint(new Vector3(mTargetPoints[(int)mTargetPoint].x, enemyTransform.position.y, mTargetPoints[(int)mTargetPoint].z));
	}

	protected void OnTouchPlayer()
	{
		touchtimer.Do();
	}

	public override void OnRage()
	{
		if (NeedRageForCoopDead)
		{
			mIsInDefent = true;
			NeedRageForCoopDead = false;
			SetDefentTimeNow();
		}
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state == Enemy.DEAD_STATE)
		{
			return;
		}
		int num = dp.damage;
		if (NeedShowDefent())
		{
			num /= 10;
			ShowDefent();
			if (GetDefentDuration() > mMaxDefentTime)
			{
				mIsInDefent = false;
				if ((float)(maxHp - hp) < (float)maxHp * hpPercentagePerHit * (float)hitTimesForRage)
				{
					mindState = MindState.NORMAL;
				}
			}
		}
		hp -= num;
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
			SetState(Mantis.MANTIS_GOTHIT_STATE);
			DisableAllEffect();
			StopSoundOnHit();
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attacked");
			GameObject original2 = Resources.Load("Effect/Mantis/mantis_dead_blood") as GameObject;
			Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
			ChangeStateAfterHit();
		}
	}

	public override void OnHitResponse()
	{
		if (state == Enemy.DEAD_STATE)
		{
			return;
		}
		if (NeedShowDefent())
		{
			ShowDefent();
			if (GetDefentDuration() > mMaxDefentTime)
			{
				mIsInDefent = false;
				if ((float)(maxHp - hp) < (float)maxHp * hpPercentagePerHit * (float)hitTimesForRage)
				{
					mindState = MindState.NORMAL;
				}
			}
		}
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
			SetState(Mantis.MANTIS_GOTHIT_STATE);
			DisableAllEffect();
			StopSoundOnHit();
			PlaySoundSingle("Audio/enemy/Mantis/tanglang_attacked");
			GameObject original2 = Resources.Load("Effect/Mantis/mantis_dead_blood") as GameObject;
			Object.Instantiate(original2, bipObject.transform.position, Quaternion.identity);
			ChangeStateAfterHit();
		}
	}

	public override bool NeedShowDefent()
	{
		return mIsInDefent;
	}

	public override void TouchPlayer()
	{
		if (state == Enemy.DEAD_STATE || !touchtimer.Ready())
		{
			return;
		}
		if (isFlying())
		{
			if (flyBodyCollider.bounds.Intersects(player.GetCollider().bounds))
			{
				player.OnHit(touchDamage);
				CheckKnocked(touchKnockSpeed);
				OnTouchPlayer();
				mCoopMantis.OnTouchPlayer();
			}
		}
		else if (bodyCollider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(touchDamage);
			CheckKnocked(touchKnockSpeed);
			OnTouchPlayer();
			mCoopMantis.OnTouchPlayer();
		}
	}

	public void RageForCoopDead()
	{
		NeedRageForCoopDead = true;
	}

	public virtual void EnableWallDefent()
	{
	}

	public virtual void DisableWallDefent()
	{
	}

	public void CreateDefent()
	{
		GameObject original = Resources.Load("Effect/Mantis/mantis_rage_defent") as GameObject;
		defentObj = Object.Instantiate(original, bipObject.transform.position, Quaternion.identity) as GameObject;
		defentObj.transform.parent = bipObject.transform;
		for (int i = 0; i < 3; i++)
		{
			defentScript[i] = defentObj.transform.GetChild(i).GetComponent<FadeInAlphaAnimationScript>();
			defentOnebyOneScript[i] = defentObj.transform.GetChild(i).GetComponent<DefentOneByOneScript>();
		}
		showDefentTimer.SetTimer(0.2f, false);
	}

	public void ShowDefent()
	{
		if (showDefentTimer.Ready())
		{
			for (int i = 0; i < 3; i++)
			{
				defentOnebyOneScript[i].appearTime = Time.time + (float)i * 0.05f;
				defentScript[i].FadeIn();
			}
			showDefentTimer.Do();
		}
	}

	public void EnableHitCollider()
	{
		if (null != mHitCheckCollider)
		{
			mHitCheckCollider.enabled = true;
		}
	}
}
