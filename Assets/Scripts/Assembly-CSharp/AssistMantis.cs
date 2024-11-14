using UnityEngine;

public class AssistMantis : CoopMantis
{
	public static EnemyState SEE_PLAYER_STATE = new AssistMantisSeePlayerState();

	public static EnemyState INIT_START_FLY_STATE = new AssistMantisInitStartFlyState();

	public static EnemyState INIT_LANDING_STATE = new AssistMantisInitLandingState();

	public static EnemyState WATCH_IDLE_STATE = new AssistMantisWatchIdleState();

	public static EnemyState WATCH_STATE = new AssistMantisWatchState();

	public static EnemyState ASSIST_IDLE_STATE = new AssistMantisAssistIdleState();

	public static EnemyState SHOT_STATE = new AssistMantisShotState();

	public static EnemyState LASER_STATE = new AssistMantisLaserState();

	public static EnemyState BOOMERANG_STATE = new AssistMantisBoomerangState();

	protected float mAssistBoomerangDistance;

	protected int mAssistProbability1;

	public AssistMantis()
	{
		EnemyBoss.INIT_STATE = new AssistMantisInitState();
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		if (null != mHitCheckCollider)
		{
			mHitCheckCollider.enabled = false;
		}
	}

	protected override void loadParameters()
	{
		base.loadParameters();
		hp = 80000;
		hpPercentagePerHit = 0.15f;
		hitTimesForRage = 4;
		touchDamage = 400;
		attackDamage = 2000;
		rushDamage = 2800;
		shotDamage = 1444;
		boomerangDamage = 1600;
		laserDamage = 2222;
		base.InitAniamtion = AnimationString.ENEMY_ATTACK;
		base.InitIdleTime = 1.2f;
		base.SeePlayerIdleTime = 1.2f;
		mAssistBoomerangDistance = 32f;
		mAssistProbability1 = 50;
	}

	public override void AddCashAndExp()
	{
		mCoopMantis.AddCashAndExpToPlayer();
	}

	public void OnWatchIdle()
	{
		SetTargetPoint(TargetPointType.LEFT_MID);
		if ((!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle()) || !(GetGroundIdleDuration() > base.MaxGroundIdleTime))
		{
			return;
		}
		int num = Random.Range(0, 100);
		if (num < 30)
		{
			SetState(WATCH_STATE);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 60, enemyTransform.position, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			SetGroundIdleTimeNow();
		}
	}

	public override void CoopFlyIdle()
	{
		if (mCoopMantis.GetState() == Enemy.DEAD_STATE)
		{
			SetState(Mantis.FLY_IDLE_STATE);
			base.CanDive = true;
		}
		else if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && GetCoopIdleDuration() > mMaxCoopIdleTime && base.CoopDiveCount == 0 && base.CoopAttackCount == 0)
		{
			SetState(Mantis.FLY_IDLE_STATE);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 14, enemyTransform.position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public void OnAssistIdle()
	{
		if ((!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle()) || isAllPlayerDead() || base.NeedChangeToCoop)
		{
			return;
		}
		byte b = 0;
		int userID = GetTargetPlayer().GetUserID();
		if (!(GetGroundIdleDuration() > base.MaxGroundIdleTime))
		{
			return;
		}
		float averagehorizontalDistance = GetAveragehorizontalDistance();
		if (averagehorizontalDistance < mAssistBoomerangDistance)
		{
			userID = GetRandomPlayer();
			int num = Random.Range(0, 100);
			if (num < mAssistProbability1)
			{
				SetState(SHOT_STATE);
				StartShot();
				b = 61;
			}
			else
			{
				SetState(LASER_STATE);
				LaserFireStart();
				b = 63;
			}
		}
		else
		{
			SetState(BOOMERANG_STATE);
			StartBoomerang();
			userID = GetFarthestPlayer();
			b = 62;
		}
		ChangeTargetPlayer(userID);
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, b, enemyTransform.position, userID);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	public void StartShot()
	{
		base.CanShot = true;
		animation[AnimationString.MANTIS_STAND_SHOT].time = 0f;
	}

	public void StartBoomerang()
	{
		base.CanShot = true;
		animation[AnimationString.MANTIS_STAND_SHOT].time = 0f;
	}

	public override void EnableWallDefent()
	{
		(mCoopMantis as MainMantis).EnableWallDefent();
	}

	public override void DisableWallDefent()
	{
		(mCoopMantis as MainMantis).DisableWallDefent();
	}
}
