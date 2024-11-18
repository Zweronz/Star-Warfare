using UnityEngine;

public class MainMantis : CoopMantis
{
	public static EnemyState SEE_PLAYER_STATE = new MainMantisSeePlayerState();

	public static EnemyState INIT_START_FLY_STATE = new MainMantisInitStartFlyState();

	public static EnemyState INIT_FLY_START_DIVE_STATE = new MainMantisFlyStartDiveState();

	public static EnemyState INIT_FLY_DIVE_STATE = new MainMantisFlyDiveState();

	protected float mLastDoubleSingleTime;

	protected int hitTimesForAssist;

	protected int hitTimesForCoop;

	protected int hitTimesForCure;

	protected int mCoopProbability1;

	protected int mCoopProbability2;

	protected int mCoopProbability3;

	protected GameObject wallDefentObject;

	public MainMantis()
	{
		EnemyBoss.INIT_STATE = new MainMantisInitState();
	}

	public override void SetDoubleSingleTimeNow()
	{
		mLastDoubleSingleTime = Time.time;
	}

	public override float GetDoubleSingleTimeDuration()
	{
		return Time.time - mLastDoubleSingleTime;
	}

	public override void InitBossLevelTime()
	{
		base.InitBossLevelTime();
		mLastDoubleSingleTime = Time.time;
	}

	protected override void loadParameters()
	{
		base.loadParameters();
		hp = 180000;
		hpPercentagePerHit = 0.2f;
		hitTimesForAssist = 1;
		hitTimesForRage = 2;
		hitTimesForCoop = 3;
		hitTimesForCure = 4;
		touchDamage = 400;
		attackDamage = 2000;
		rushDamage = 2800;
		shotDamage = 1444;
		boomerangDamage = 1600;
		laserDamage = 2222;
		base.InitAniamtion = AnimationString.MANTIS_CRITICAL_ATTACK;
		base.InitIdleTime = 0f;
		base.SeePlayerIdleTime = 0f;
		mCoopProbability1 = 44;
		mCoopProbability2 = 16;
		mCoopProbability3 = 50;
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		CreateWallDefent();
	}

	public bool AreBothDead()
	{
		if (mCoopMantis == null)
		{
			return false;
		}
		return state == Enemy.DEAD_STATE && mCoopMantis.GetState() == Enemy.DEAD_STATE;
	}

	public override void AddCashAndExp()
	{
		AddCashAndExpToPlayer();
	}

	public override bool RemoveDeadBodyTimer()
	{
		if (AreBothDead() && deadRemoveBodyTimer.Ready() && mCoopMantis.RemoveDeadBodyTimer())
		{
			return true;
		}
		return false;
	}

	public void DecideCoopJoinPoint()
	{
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			if (enemyTransform.position.x < mCoopMantis.GetTransform().position.x)
			{
				SetTargetPoint(TargetPointType.LEFT_MID);
				mCoopMantis.SetTargetPoint(TargetPointType.RIGHT_MID);
			}
			else
			{
				SetTargetPoint(TargetPointType.RIGHT_MID);
				mCoopMantis.SetTargetPoint(TargetPointType.LEFT_MID);
			}
		}
		else if (enemyTransform.position.x < mCoopMantis.GetTransform().position.x)
		{
			SetTargetPoint(TargetPointType.LEFT_BOTTOM);
			mCoopMantis.SetTargetPoint(TargetPointType.RIGHT_TOP);
		}
		else
		{
			SetTargetPoint(TargetPointType.RIGHT_BOTTOM);
			mCoopMantis.SetTargetPoint(TargetPointType.LEFT_TOP);
		}
	}

	public void DecideCoopStartPoint()
	{
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			if (enemyTransform.position.x < mCoopMantis.GetTransform().position.x)
			{
				SetTargetPoint(TargetPointType.LEFT_MID);
				mCoopMantis.SetTargetPoint(TargetPointType.RIGHT_MID);
			}
			else
			{
				SetTargetPoint(TargetPointType.RIGHT_MID);
				mCoopMantis.SetTargetPoint(TargetPointType.LEFT_MID);
			}
			return;
		}
		num = Random.Range(0, 100);
		if (num < 50)
		{
			if (enemyTransform.position.x < mCoopMantis.GetTransform().position.x)
			{
				SetTargetPoint(TargetPointType.LEFT_BOTTOM);
				mCoopMantis.SetTargetPoint(TargetPointType.RIGHT_TOP);
			}
			else
			{
				SetTargetPoint(TargetPointType.RIGHT_BOTTOM);
				mCoopMantis.SetTargetPoint(TargetPointType.LEFT_TOP);
			}
		}
		else if (enemyTransform.position.x < mCoopMantis.GetTransform().position.x)
		{
			SetTargetPoint(TargetPointType.LEFT_TOP);
			mCoopMantis.SetTargetPoint(TargetPointType.RIGHT_BOTTOM);
		}
		else
		{
			SetTargetPoint(TargetPointType.RIGHT_TOP);
			mCoopMantis.SetTargetPoint(TargetPointType.LEFT_BOTTOM);
		}
	}

	public void DecideCoopAttack()
	{
		byte b = 0;
		bool coopAttackTargetPoint = Random.Range(0, 100) < 50;
		int num = Random.Range(0, 100);
		if (mCurrentPoint < TargetPointType.LEFT_TOP)
		{
			SetCoopAttackTargetPoint(coopAttackTargetPoint);
			mCoopMantis.SetCoopAttackTargetPoint(coopAttackTargetPoint);
			if (num < mCoopProbability1)
			{
				SetState(CoopMantis.COOP_FLY_LASER_STATE);
				LaserFireStart();
				mCoopMantis.SetState(CoopMantis.COOP_FLY_LASER_STATE);
				mCoopMantis.LaserFireStart();
				b = 73;
			}
			else
			{
				SetState(CoopMantis.COOP_FLY_SHOT_STATE);
				StartFlyShot();
				mCoopMantis.SetState(CoopMantis.COOP_FLY_SHOT_STATE);
				mCoopMantis.StartFlyShot();
				b = 74;
			}
		}
		else if (num < mCoopProbability2)
		{
			InitCoopDiveCount();
			mCoopMantis.InitCoopDiveCount();
		}
		else
		{
			SetCoopAttackTargetPoint(coopAttackTargetPoint);
			mCoopMantis.SetCoopAttackTargetPoint(coopAttackTargetPoint);
			if (num < mCoopProbability3)
			{
				SetState(CoopMantis.COOP_FLY_LASER_STATE);
				LaserFireStart();
				mCoopMantis.SetState(CoopMantis.COOP_FLY_LASER_STATE);
				mCoopMantis.LaserFireStart();
				b = 73;
			}
			else
			{
				SetState(CoopMantis.COOP_FLY_SHOT_STATE);
				StartFlyShot();
				mCoopMantis.SetState(CoopMantis.COOP_FLY_SHOT_STATE);
				mCoopMantis.StartFlyShot();
				b = 74;
			}
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, (byte)mTargetPoint);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			request = new EnemyStateRequest(mCoopMantis.EnemyID, b, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	protected override void ChangeStateAfterHit()
	{
		base.ChangeStateAfterHit();
		if ((float)(maxHp - hp) > (float)maxHp * hpPercentagePerHit * (float)hitTimesForCure)
		{
			return;
		}
		if ((float)(maxHp - hp) > (float)maxHp * hpPercentagePerHit * (float)hitTimesForCoop)
		{
			DecideCoopJoinPoint();
			base.NeedChangeToCoop = true;
			base.CanDive = false;
			InitCoopAttackCount();
			if (mCoopMantis != null)
			{
				mCoopMantis.NeedChangeToCoop = true;
				mCoopMantis.CanDive = false;
				mCoopMantis.InitCoopAttackCount();
			}
		}
		else if ((float)(maxHp - hp) > (float)maxHp * hpPercentagePerHit * (float)hitTimesForAssist && mCoopMantis != null)
		{
			mCoopMantis.SetState(AssistMantis.ASSIST_IDLE_STATE);
		}
	}

	public override void CoopFlyIdle()
	{
		if (mCoopMantis.GetState() == Enemy.DEAD_STATE)
		{
			SetState(Mantis.FLY_IDLE_STATE);
			base.CanDive = true;
		}
		else
		{
			if ((!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle()) || !(GetCoopIdleDuration() > mMaxCoopIdleTime))
			{
				return;
			}
			if (mCoopMantis.GetState() == CoopMantis.COOP_FLY_IDLE_STATE)
			{
				if (base.CoopDiveCount > 0)
				{
					DecreaseCoopDiveCount();
					SetCoopDiveAttackTargetPoint();
					SetState(CoopMantis.COOP_FLY_START_DIVE_STATE);
					mCoopMantis.SetCoopDiveAttackTargetPoint();
					mCoopMantis.SetState(CoopMantis.COOP_FLY_START_DIVE_STATE);
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 75, enemyTransform.position, (byte)mTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
						request = new EnemyStateRequest(mCoopMantis.EnemyID, 75, mCoopMantis.GetTransform().position, (byte)mCoopMantis.CoopTargetPoint);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request);
					}
				}
				else if (base.CoopAttackCount > 0)
				{
					DecreaseCoopAttackCount();
					DecideCoopAttack();
				}
				else
				{
					SetState(Mantis.FLY_IDLE_STATE);
					SetDoubleSingleTimeNow();
					mCoopMantis.SetState(Mantis.FLY_IDLE_STATE);
					mCoopMantis.SetDoubleSingleTimeNow();
					if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
					{
						EnemyStateRequest request2 = new EnemyStateRequest(base.EnemyID, 14, enemyTransform.position, Vector3.zero);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
						request2 = new EnemyStateRequest(mCoopMantis.EnemyID, 14, mCoopMantis.GetTransform().position, Vector3.zero);
						GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
					}
				}
			}
			else if (base.CoopDiveCount == 0 && base.CoopAttackCount == 0)
			{
				SetState(Mantis.FLY_IDLE_STATE);
				SetDoubleSingleTimeNow();
				mCoopMantis.ClearCoopAttackCount();
				mCoopMantis.ClearCoopDiveCount();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request3 = new EnemyStateRequest(base.EnemyID, 14, enemyTransform.position, Vector3.zero);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request3);
				}
			}
		}
	}

	public void CreateWallDefent()
	{
		GameObject original = Resources.Load("Effect/Mantis/mantis_wall_defent") as GameObject;
		wallDefentObject = Object.Instantiate(original, new Vector3(5f, 13f, -25f), Quaternion.identity) as GameObject;
		wallDefentObject.GetComponent<Renderer>().enabled = false;
		SphereCollider component = wallDefentObject.GetComponent<SphereCollider>();
		component.enabled = true;
	}

	public override void EnableWallDefent()
	{
		wallDefentObject.GetComponent<Renderer>().enabled = true;
		SphereCollider component = wallDefentObject.GetComponent<SphereCollider>();
		component.enabled = true;
	}

	public override void DisableWallDefent()
	{
		wallDefentObject.GetComponent<Renderer>().enabled = false;
		SphereCollider component = wallDefentObject.GetComponent<SphereCollider>();
		component.enabled = false;
	}
}
