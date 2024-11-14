using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
	protected const float SLOW_DOWN_EFFECT = 0.7f;

	public static EnemyState GRAVEBORN_STATE = new GraveBornState();

	public static EnemyState IDLE_STATE = new EnemyIdleState();

	public static EnemyState CATCHING_STATE = new CatchingState();

	public static EnemyState GOTHIT_STATE = new GotHitState();

	public static EnemyState ATTACK_STATE = new EnemyAttackState();

	public static EnemyState DEAD_STATE = new DeadState();

	public static EnemyState GRAVITY_FORCE_STATE = new GravityForceState();

	protected GameObject enemyObject;

	protected Transform enemyTransform;

	protected Animation animation;

	protected Rigidbody rigidbody;

	protected Transform aimedTransform;

	protected Transform target;

	protected Vector3 spawnCenter;

	protected Vector3 patrolTarget;

	protected Collider collider;

	protected EnemyType enemyType;

	protected Vector3 lastTarget;

	protected GameWorld gameWorld;

	protected Player player;

	protected Vector3 dir;

	protected AStarPathFinding aStarPathFinding;

	protected Timer deadRemoveBodyTimer;

	protected Player targetPlayer;

	protected int hp;

	protected int maxHp;

	protected float runSpeed;

	protected bool beWokeUp;

	protected float deadTime;

	protected bool visible;

	protected bool moveWithCharacterController;

	protected float lastUpdateTime;

	protected float lastPathFindingTime;

	protected float lastCheckWaypointTime;

	protected float lastReCheckPathTime = Time.time;

	protected EnemyState state;

	protected float attackRange;

	protected float detectionRange;

	protected float minRange;

	protected float attackFrequency;

	protected int attackDamage;

	protected int rushDamage;

	protected int shotDamage;

	protected int touchDamage;

	protected float bombRange;

	protected int score;

	protected int lootCash;

	protected int exp;

	protected float gotHitTime;

	protected float idleStartTime;

	protected float lastAttackTime = -100f;

	protected string runAnimationName = AnimationString.ENEMY_RUN;

	protected GameObject targetObj;

	protected float onhitRate = 100f;

	protected bool criticalAttacked;

	protected bool attacked;

	protected Quaternion deadRotation;

	protected Vector3 deadPosition;

	protected Vector3[] path;

	protected int lastTargetCode = -1;

	protected Timer catchPlayerTargetChangeTimer = new Timer();

	protected BloodColor bloodColor;

	protected float lastReachPointTime;

	public Ray ray;

	public RaycastHit rayhit;

	public float lastStateTime;

	protected Timer shoutAudioTimer = new Timer();

	protected string shoutAudioName;

	protected MonsterConfig monsterConfig;

	protected Vector3 smoothPos = Vector3.zero;

	protected float floorHeight = Global.FLOORHEIGHT;

	protected bool seePlayer;

	protected Timer recordTimer = new Timer();

	protected Quaternion enemyRotation;

	protected float lastMoveTime;

	protected Vector3 lastPos;

	protected Timer gotHitAnimationCDTimer = new Timer();

	protected Vector3 localScale = Vector3.zero;

	protected float wayPointSqrDistance;

	protected Timer lastBloodEffectTimer = new Timer();

	protected bool mIsSlowDown;

	protected Timer mSlowDownTimer = new Timer();

	protected bool mAim;

	protected Timer mAimTimer = new Timer();

	public bool isAiming;

	protected Vector3 mGravityForceTarget;

	protected float mStartGravityFoceTime;

	protected GameObject mGravityForceBallObj;

	protected GameObject mGravityForceBeamObj;

	public string EnemyName { get; set; }

	public short EnemyID { get; set; }

	public bool IsElite { get; set; }

	public string RunAnimationName
	{
		get
		{
			return runAnimationName;
		}
	}

	public int HP
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
		}
	}

	public int MaxHP
	{
		get
		{
			return maxHp;
		}
		set
		{
			maxHp = value;
		}
	}

	public float DetectionRange
	{
		get
		{
			return detectionRange;
		}
	}

	public float AttackRange
	{
		get
		{
			return attackRange;
		}
	}

	public EnemyType EnemyType
	{
		get
		{
			return enemyType;
		}
		set
		{
			enemyType = value;
		}
	}

	public Vector3 LastTarget
	{
		get
		{
			return lastTarget;
		}
	}

	public float WayPointSqrDistance
	{
		get
		{
			return wayPointSqrDistance;
		}
	}

	public float SqrDistanceFromPlayer
	{
		get
		{
			if (target == null)
			{
				return 99999f;
			}
			return (target.position - enemyTransform.position).sqrMagnitude;
		}
	}

	public Timer GetLastBloodEffectTimer()
	{
		return lastBloodEffectTimer;
	}

	public Vector3 GetLocalScale()
	{
		return localScale;
	}

	public Transform GetTransform()
	{
		return enemyTransform;
	}

	public bool CouldMakeNextAttack()
	{
		if (Time.time - lastAttackTime >= attackFrequency)
		{
			return true;
		}
		return false;
	}

	public virtual bool CouldEnterAttackState()
	{
		if (SqrDistanceFromPlayer < AttackRange * AttackRange)
		{
			return true;
		}
		return false;
	}

	public virtual void PlayAnimation(string name)
	{
		animation.CrossFade(name);
	}

	public virtual void PlayAnimation(string name, WrapMode mode)
	{
		if (!animation.IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			animation[name].wrapMode = mode;
			animation.CrossFade(name);
		}
	}

	public virtual void PlayAnimation(string name, WrapMode mode, float speed)
	{
		if (!animation.IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			animation[name].wrapMode = mode;
			animation[name].speed = speed;
			animation.CrossFade(name);
		}
	}

	public void StopAnimation()
	{
		animation.Stop();
	}

	public virtual void SetInGrave(bool inGrave)
	{
		if (inGrave)
		{
			SetState(GRAVEBORN_STATE);
			enemyTransform.Translate(Vector3.down * 2f);
			enemyObject.layer = PhysicsLayer.Default;
			if (enemyObject.rigidbody != null)
			{
				enemyTransform.rigidbody.useGravity = false;
				enemyTransform.rigidbody.isKinematic = true;
			}
		}
		else
		{
			enemyObject.layer = PhysicsLayer.ENEMY;
			if (enemyObject.rigidbody != null)
			{
				enemyTransform.rigidbody.useGravity = true;
				enemyTransform.rigidbody.isKinematic = false;
			}
			GameObject original = Resources.Load("Effect/GraveRock2") as GameObject;
			Object.Instantiate(original, enemyTransform.position + Vector3.down * 0f, Quaternion.identity);
		}
	}

	public bool IsBoss()
	{
		if (EnemyType == EnemyType.Beetle || EnemyType == EnemyType.Dragon || EnemyType == EnemyType.Mantis || EnemyType == EnemyType.MainMantis || EnemyType == EnemyType.AssistMantis || EnemyType == EnemyType.Earthworm || EnemyType == EnemyType.SantaMachine)
		{
			return true;
		}
		return false;
	}

	public virtual bool MoveFromGrave(float deltaTime)
	{
		enemyTransform.Translate(Vector3.up * deltaTime * 2f);
		if (enemyTransform.position.y >= (float)Global.FLOORHEIGHT + 0.2f)
		{
			return true;
		}
		return false;
	}

	public virtual void UpdatePosition(Vector3 position)
	{
		GetTransform().position = position;
	}

	public virtual void LoadConfig()
	{
		UnitDataTable unitDataTable = Res2DManager.GetInstance().vDataTable[0];
		monsterConfig = new MonsterConfig();
		monsterConfig.hp = unitDataTable.GetData((int)EnemyType, 1, 0, false);
		monsterConfig.walkSpeed = unitDataTable.GetData((int)EnemyType, 2, 0, false);
		monsterConfig.exp = unitDataTable.GetData((int)EnemyType, 4, 0, false);
		monsterConfig.lootCash = unitDataTable.GetData((int)EnemyType, 5, 0, false);
		monsterConfig.attackID = (byte)unitDataTable.GetData((int)EnemyType, 3, 0, false);
		UnitDataTable unitDataTable2 = Res2DManager.GetInstance().vDataTable[monsterConfig.attackID];
		monsterConfig.attack = new MonsterConfig.Attack[unitDataTable2.sRows];
		int difficultyLevel = GameApp.GetInstance().GetGameWorld().DifficultyLevel;
		float num = 1f;
		float num2 = 1f;
		for (int i = 0; i < unitDataTable2.sRows; i++)
		{
			monsterConfig.attack[i] = new MonsterConfig.Attack();
			monsterConfig.attack[i].damage = unitDataTable2.GetData(i, 1, 0, false);
			monsterConfig.attack[i].attackRate = unitDataTable2.GetData(i, 2, 0, false);
			monsterConfig.attack[i].range = unitDataTable2.GetData(i, 3, 0, false);
			monsterConfig.attack[i].bombRange = unitDataTable2.GetData(i, 4, 0, false);
			monsterConfig.attack[i].splashDamage = unitDataTable2.GetData(i, 5, 0, false);
			monsterConfig.attack[i].splashDuration = (float)unitDataTable2.GetData(i, 6, 0, false) / 10f;
			monsterConfig.attack[i].splashRange = unitDataTable2.GetData(i, 7, 0, false);
			monsterConfig.attack[i].moveSpeed = unitDataTable2.GetData(i, 8, 0, false);
			num = 1f;
			num2 = 1f;
			float num3 = monsterConfig.attack[i].damage;
			for (int j = 0; j < difficultyLevel; j++)
			{
				num2 = 1.1f + 0.01f * (float)j;
				if (num2 > 1.25f)
				{
					num2 = 1.25f;
				}
				num *= num2;
				if (num > 70f)
				{
					num = 70f;
				}
			}
			num3 *= num;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				switch (gameWorld.GetPlayingPlayerCount())
				{
				case 2:
					num3 *= 1.2f;
					break;
				case 3:
					num3 *= 1.5f;
					break;
				}
			}
			monsterConfig.attack[i].damage = (int)num3;
		}
		attackDamage = monsterConfig.attack[0].damage;
		attackFrequency = monsterConfig.attack[0].attackRate;
		attackRange = monsterConfig.attack[0].range;
		bombRange = monsterConfig.attack[0].bombRange;
		runSpeed = monsterConfig.walkSpeed;
		hp = monsterConfig.hp;
		lootCash = monsterConfig.lootCash;
		exp = monsterConfig.exp;
		int num4 = hp;
		float num5 = hp;
		float num6 = lootCash;
		float num7 = exp;
		num2 = 1f;
		float num8 = 1f;
		if (enemyType != EnemyType.Beetle && enemyType != EnemyType.Dragon && enemyType != EnemyType.Mantis && enemyType != EnemyType.MainMantis && enemyType != EnemyType.AssistMantis && enemyType != EnemyType.Earthworm && enemyType != EnemyType.SantaMachine)
		{
			for (int k = 0; k < difficultyLevel; k++)
			{
				num2 = 1.25f - (float)k * 0.01f;
				if (num2 < 1.1f)
				{
					num2 = 1.1f;
				}
				num8 *= num2;
				if (num8 > 80f)
				{
					num8 = 80f;
				}
				num6 *= 1.07f;
				num7 *= 1.1f;
			}
		}
		num5 *= num8;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				switch (gameWorld.GetPlayingPlayerCount())
				{
				case 2:
					num5 *= 1.6f;
					if (!IsBoss())
					{
						num6 *= 1.5f;
						num7 *= 1.5f;
					}
					else
					{
						num6 *= 1.2f;
					}
					break;
				case 3:
					num5 *= 2f;
					if (!IsBoss())
					{
						num6 *= 1.8f;
						num7 *= 1.8f;
					}
					else
					{
						num6 *= 1.5f;
					}
					break;
				}
			}
		}
		else if (IsBoss())
		{
			num6 *= 0.75f;
		}
		if (IsElite)
		{
			num6 *= 3f;
			num7 *= 3f;
		}
		hp = (int)num5;
		lootCash = (int)num6;
		exp = (int)num7;
	}

	public virtual void Init(GameObject gObject)
	{
		gameWorld = GameApp.GetInstance().GetGameWorld();
		player = gameWorld.GetPlayer();
		enemyObject = gObject;
		EnemyName = enemyObject.name;
		enemyTransform = enemyObject.transform;
		animation = enemyObject.animation;
		rigidbody = enemyObject.rigidbody;
		collider = enemyObject.transform.collider;
		detectionRange = 150f;
		attackRange = 2.5f;
		minRange = 2.5f;
		LoadConfig();
		criticalAttacked = false;
		spawnCenter = enemyTransform.position;
		target = GameApp.GetInstance().GetGameWorld().GetPlayer()
			.GetTransform();
		aStarPathFinding = new AStarPathFinding();
		if (enemyObject.animation != null)
		{
			animation.wrapMode = WrapMode.Loop;
			animation.Play(AnimationString.ENEMY_IDLE);
		}
		state = IDLE_STATE;
		lastUpdateTime = Time.time;
		lastPathFindingTime = Time.time;
		lastCheckWaypointTime = Time.time;
		idleStartTime = -2f;
		if (enemyObject.animation != null)
		{
			enemyObject.animation[AnimationString.ENEMY_ATTACK].wrapMode = WrapMode.ClampForever;
			enemyObject.animation[AnimationString.ENEMY_RUN].speed = 1f;
			enemyObject.animation[AnimationString.ENEMY_GOTHIT].speed = 1f;
		}
		catchPlayerTargetChangeTimer.SetTimer(0.5f, false);
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			targetPlayer = GameApp.GetInstance().GetGameWorld().GetPlayer();
		}
		shoutAudioTimer.SetTimer(5f, true);
		shoutAudioName = "Audio/enemy/gongchong";
		gotHitTime = Time.time;
		lastReachPointTime = Time.time;
		recordTimer.SetTimer(3f, false);
		lastMoveTime = Time.time;
		lastPos = enemyTransform.position;
		gotHitAnimationCDTimer = new Timer();
		gotHitAnimationCDTimer.SetTimer(0.2f, true);
		mSlowDownTimer.SetTimer(3f, false);
		mAimTimer.SetTimer(5f, true);
		localScale = Vector3.one;
		wayPointSqrDistance = 0.1f;
		lastBloodEffectTimer.SetTimer(0.3f, false);
		StopGravityForceEffect();
	}

	public virtual void SetState(EnemyState newState)
	{
		state = newState;
	}

	public EnemyState GetState()
	{
		return state;
	}

	public virtual void CheckHit()
	{
	}

	public virtual bool AttackAnimationEnds()
	{
		if (Time.time - lastAttackTime > enemyObject.animation[AnimationString.ENEMY_ATTACK].length)
		{
			return true;
		}
		return false;
	}

	public virtual bool AttackAnimationEnds(string name)
	{
		if (Time.time - lastAttackTime > enemyObject.animation[name].length)
		{
			return true;
		}
		return false;
	}

	public Transform GetAimedTransform()
	{
		return aimedTransform;
	}

	public Vector3 GetPosition()
	{
		return enemyTransform.position;
	}

	public Collider GetCollider()
	{
		return collider;
	}

	public bool GotHitAnimationEnds()
	{
		if (Time.time - gotHitTime >= animation[AnimationString.ENEMY_GOTHIT].clip.length)
		{
			return true;
		}
		return false;
	}

	public virtual void OnHit(DamageProperty dp)
	{
		if (state != GRAVEBORN_STATE && state != DEAD_STATE)
		{
			beWokeUp = true;
			hp -= dp.damage;
			if (IsElite)
			{
				criticalAttacked = false;
			}
			else
			{
				criticalAttacked = dp.criticalAttack;
			}
			gotHitTime = Time.time;
			SetState(GOTHIT_STATE);
		}
	}

	public virtual void OnHitResponse()
	{
		beWokeUp = true;
		if (lastBloodEffectTimer.Ready())
		{
			gameWorld.GetHitBloodPool().CreateObject(GetPosition(), Vector3.zero, Quaternion.identity);
			lastBloodEffectTimer.Do();
		}
		SetState(GOTHIT_STATE);
		gotHitTime = Time.time;
	}

	public virtual void SetCriticalAttack(bool criticalAttack)
	{
		if (IsElite)
		{
			criticalAttacked = false;
		}
		else
		{
			criticalAttacked = criticalAttack;
		}
	}

	public virtual void OnIdle()
	{
		SetState(CATCHING_STATE);
	}

	public virtual void OnAttack()
	{
	}

	public Player GetTargetPlayer()
	{
		return targetPlayer;
	}

	public virtual void LookAtPoint(Vector3 target)
	{
		enemyTransform.LookAt(target);
	}

	public void ChangeTarget(Vector3 target, int targetCode, bool fly)
	{
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			bool flag = false;
			if (lastTargetCode != targetCode || targetCode == 0)
			{
				flag = true;
			}
			if (catchPlayerTargetChangeTimer.Ready())
			{
				flag = true;
				catchPlayerTargetChangeTimer.Do();
			}
			if (flag)
			{
				lastTarget = target;
				LookAtPoint(new Vector3(lastTarget.x, enemyTransform.position.y, lastTarget.z));
				enemyRotation = enemyTransform.rotation;
				dir = (lastTarget - enemyTransform.position).normalized;
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyMoveRequest request = new EnemyMoveRequest(enemyTransform.position, lastTarget, EnemyID, targetCode, fly);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				lastTargetCode = targetCode;
			}
			return;
		}
		lastTarget = target;
		if (targetCode != 0)
		{
			if (player.GetUserID() == targetCode)
			{
				targetPlayer = player;
			}
			else
			{
				targetPlayer = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(targetCode);
			}
		}
		LookAtPoint(new Vector3(lastTarget.x, enemyTransform.position.y, lastTarget.z));
		enemyRotation = enemyTransform.rotation;
		dir = (lastTarget - enemyTransform.position).normalized;
	}

	public void ChangeTargetPlayer(int targetID)
	{
		if (targetID != 0)
		{
			if (Lobby.GetInstance().GetChannelID() == targetID)
			{
				targetPlayer = player;
			}
			else
			{
				targetPlayer = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(targetID);
			}
			target = targetPlayer.GetTransform();
		}
	}

	public virtual void LookAtTargetPlayer()
	{
		if (targetPlayer != null)
		{
			LookAtPoint(new Vector3(targetPlayer.GetTransform().position.x, enemyTransform.position.y, targetPlayer.GetTransform().position.z));
		}
	}

	public virtual void LookAtTarget()
	{
		LookAtPoint(new Vector3(target.transform.position.x, enemyTransform.position.y, target.transform.position.z));
	}

	public virtual void FindNewTarget()
	{
	}

	public virtual void FindTarget()
	{
		Player player = this.player;
		float num = Vector3.Distance(this.player.GetTransform().position, enemyTransform.position);
		if (!this.player.InPlayingState())
		{
			num = 99999f;
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				float num2 = Vector3.Distance(item.GetTransform().position, enemyTransform.position);
				if (num2 < num)
				{
					player = item;
					num = num2;
				}
			}
		}
		target = player.GetTransform();
		targetPlayer = player;
	}

	public int GetRandomPlayer()
	{
		List<int> list = new List<int>();
		if (player.InPlayingState())
		{
			list.Add(Lobby.GetInstance().GetChannelID());
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				list.Add(item.GetUserID());
			}
		}
		if (list.Count == 0)
		{
			return Lobby.GetInstance().GetChannelID();
		}
		int index = Random.Range(0, list.Count);
		return list[index];
	}

	public float GetNearestDistanceToPlayer()
	{
		int channelID = Lobby.GetInstance().GetChannelID();
		float num = Vector3.Distance(player.GetTransform().position, enemyTransform.position);
		if (!player.InPlayingState())
		{
			num = 99999f;
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				float num2 = Vector3.Distance(item.GetTransform().position, enemyTransform.position);
				if (num2 < num)
				{
					channelID = item.GetUserID();
					num = num2;
				}
			}
		}
		return num;
	}

	public int GetNearestPlayer()
	{
		int result = Lobby.GetInstance().GetChannelID();
		float num = Vector3.Distance(player.GetTransform().position, enemyTransform.position);
		if (!player.InPlayingState())
		{
			num = 99999f;
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				float num2 = Vector3.Distance(item.GetTransform().position, enemyTransform.position);
				if (num2 < num)
				{
					result = item.GetUserID();
					num = num2;
				}
			}
		}
		return result;
	}

	public int GetFarthestPlayer()
	{
		int result = Lobby.GetInstance().GetChannelID();
		float num = Vector3.Distance(player.GetTransform().position, enemyTransform.position);
		if (!player.InPlayingState())
		{
			num = 0f;
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				float num2 = Vector3.Distance(item.GetTransform().position, enemyTransform.position);
				if (num2 > num)
				{
					result = item.GetUserID();
					num = num2;
				}
			}
		}
		return result;
	}

	public virtual float GetAveragehorizontalDistance()
	{
		int num = 0;
		float num2 = 0f;
		if (player.InPlayingState())
		{
			num2 += Vector3.Distance(new Vector3(player.GetTransform().position.x, 0f, player.GetTransform().position.z), new Vector3(enemyTransform.position.x, 0f, enemyTransform.position.z));
			num++;
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState())
			{
				num2 += Vector3.Distance(new Vector3(item.GetTransform().position.x, 0f, item.GetTransform().position.z), new Vector3(enemyTransform.position.x, 0f, enemyTransform.position.z));
				num++;
			}
		}
		return (num != 0) ? (num2 / (float)num) : 0f;
	}

	public virtual float GetNearestDistanceToTargetPlayer()
	{
		float num = 99999f;
		if (player.InPlayingState() && player != targetPlayer)
		{
			num = Vector3.Distance(player.GetTransform().position, target.position);
		}
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item.InPlayingState() && item != targetPlayer)
			{
				float num2 = Vector3.Distance(item.GetTransform().position, target.position);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	protected bool ArriveAtTargetWaypoint()
	{
		if (Time.time - lastCheckWaypointTime > 0.3f / runSpeed)
		{
			lastCheckWaypointTime = Time.time;
			if (aStarPathFinding.Target == null)
			{
				return false;
			}
			Vector3 vector = enemyTransform.position - aStarPathFinding.Target.Position;
			vector.y = 0f;
			return vector.sqrMagnitude < WayPointSqrDistance;
		}
		return false;
	}

	protected virtual bool RaycastTargetPlayer(out RaycastHit rayhit)
	{
		ray = new Ray(enemyTransform.position + new Vector3(0f, 0.5f, 0f), target.position + new Vector3(0f, 0.5f, 0f) - (enemyTransform.position + new Vector3(0f, 0.5f, 0f)));
		return Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER));
	}

	public virtual void FindPath()
	{
		FindTarget();
		Vector3 position = target.position;
		if (!ArriveAtTargetWaypoint() && !(Time.time - lastPathFindingTime > 3f / runSpeed))
		{
			return;
		}
		lastPathFindingTime = Time.time;
		position.y = enemyTransform.position.y;
		if (lastTarget == Vector3.zero)
		{
			ChangeTarget(target.position, Lobby.GetInstance().GetChannelID(), false);
		}
		if (!RaycastTargetPlayer(out rayhit))
		{
			return;
		}
		float y = (target.position - enemyTransform.position).y;
		if ((targetPlayer.IsLocal() && rayhit.collider.gameObject.layer == PhysicsLayer.PLAYER) || (!targetPlayer.IsLocal() && rayhit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
		{
			ChangeTarget(position, targetPlayer.GetUserID(), false);
			aStarPathFinding.Reset();
			seePlayer = true;
			return;
		}
		seePlayer = false;
		if (aStarPathFinding.GetNextWayPoint(enemyTransform.position, targetPlayer, this) && aStarPathFinding.Target != null)
		{
			ChangeTarget(aStarPathFinding.Target.Position, 0, false);
		}
		if (!(Time.time - lastReCheckPathTime > 5f) || !(enemyTransform != null) || aStarPathFinding.Target == null)
		{
			return;
		}
		ray = new Ray(enemyTransform.position + new Vector3(0f, 0.5f, 0f), aStarPathFinding.Target.Position - (enemyTransform.position + new Vector3(0f, 0.5f, 0f)));
		if (Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL)))
		{
			aStarPathFinding.Reset();
			if (aStarPathFinding.GetNextWayPoint(enemyTransform.position, targetPlayer, this) && aStarPathFinding.Target != null)
			{
				ChangeTarget(aStarPathFinding.Target.Position, 0, false);
			}
		}
		lastReCheckPathTime = Time.time;
	}

	public int GetLoot(LootType lootType)
	{
		int num = 0;
		switch (lootType)
		{
		case LootType.Money:
			num = ((!IsElite) ? Random.Range(50, 150) : Random.Range(150, 200));
			break;
		case LootType.Enegy:
			num = ((!IsElite) ? Random.Range(50, 80) : Random.Range(100, 150));
			break;
		}
		float num2 = 1f;
		for (int i = 0; i < gameWorld.DifficultyLevel; i++)
		{
			num2 *= 1.075f;
		}
		return (int)((float)num * num2);
	}

	public virtual void OnDead()
	{
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			LootManagerScript component = enemyTransform.GetComponent<LootManagerScript>();
			if (component != null)
			{
				component.OnLoot();
			}
		}
		enemyObject.layer = PhysicsLayer.DEADBODY;
		deadRemoveBodyTimer = new Timer();
		if (EnemyType == EnemyType.Boomer)
		{
			criticalAttacked = true;
		}
		if (criticalAttacked)
		{
			deadRemoveBodyTimer.SetTimer(3f, true);
		}
		else
		{
			deadRemoveBodyTimer.SetTimer(3f, false);
		}
		GameObject original = null;
		GameObject original2 = null;
		Vector3 ground = GetGround();
		int num = Random.Range(0, 100);
		if (bloodColor == BloodColor.Red)
		{
			original = ((num >= 50) ? (Resources.Load("Effect/BugDeadBlood2") as GameObject) : (Resources.Load("Effect/BugDeadBlood") as GameObject));
			int num2 = num % 3 + 1;
			original2 = Resources.Load("Effect/Blood_Ground" + num2) as GameObject;
		}
		else if (bloodColor == BloodColor.Green)
		{
			original = Resources.Load("Effect/BugDeadBlood3") as GameObject;
			original2 = Resources.Load("Effect/Blood_Ground4") as GameObject;
		}
		Object.Instantiate(original, enemyTransform.position + Vector3.up * 1.2f, Quaternion.identity);
		GameObject gameObject = Object.Instantiate(original2, new Vector3(enemyTransform.position.x, floorHeight + 0.1f, enemyTransform.position.z), Quaternion.Euler(270f, 0f, 0f)) as GameObject;
		if (!criticalAttacked)
		{
			gameObject.transform.localScale *= 0.3f;
			ScaleScript scaleScript = gameObject.AddComponent<ScaleScript>();
			scaleScript.enabled = true;
			scaleScript.scaleSpeed = 0.1f;
			scaleScript.enableMaxScale = true;
			scaleScript.maxScale = 0.3f;
		}
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground);
		gameObject.transform.rotation = quaternion * gameObject.transform.rotation;
		if (criticalAttacked)
		{
			num = Random.Range(0, 360);
			GameObject gameObject2 = gameWorld.GetDeadBodyPool(enemyType).CreateObject(new Vector3(enemyTransform.position.x, floorHeight + 0.1f, enemyTransform.position.z), Vector3.zero, Quaternion.Euler(0f, num, 0f));
			gameObject2.transform.rotation = quaternion * gameObject2.transform.rotation;
			num = Random.Range(0, 100);
			if (num < 30)
			{
				gameObject2.animation.Play("ani");
			}
			else if (num < 65)
			{
				gameObject2.animation.Play("ani02");
			}
			else
			{
				gameObject2.animation.Play("ani03");
			}
		}
		num = Random.Range(0, 100);
		if (num < 50)
		{
			AudioManager.GetInstance().PlaySound("Audio/enemies_smash1");
		}
		else
		{
			AudioManager.GetInstance().PlaySound("Audio/enemies_smash2");
		}
		if (GameApp.GetInstance().GetGameMode().IsSingle())
		{
			AddCashAndExpToPlayer();
			player.RecoveryWhenMakeKills();
		}
		mAim = false;
		isAiming = false;
		StopGravityForceEffect();
	}

	public void AddCashAndExpToPlayer()
	{
		player.MakeCombo();
		player.Kills++;
		float skill = player.GetSkills().GetSkill(SkillsType.MONEY_BOOTH);
		float skill2 = player.GetSkills().GetSkill(SkillsType.EXP_BOOTH);
		float num = 0f;
		if (player.GetCombo() >= 10)
		{
			num = 1f;
		}
		else if (player.GetCombo() >= 8)
		{
			num = 0.5f;
		}
		else if (player.GetCombo() >= 5)
		{
			num = 0.2f;
		}
		else if (player.GetCombo() >= 3)
		{
			num = 0.1f;
		}
		if (IsBoss())
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			int num2 = 0;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				int level = userState.GetNetStage() - Global.TOTAL_STAGE;
				num2 = userState.GetSuccCoopBoss(level);
				int playingPlayerCount = gameWorld.GetPlayingPlayerCount();
				userState.AddSuccCoopBossStage(level);
			}
			else
			{
				int level2 = userState.GetStage() - Global.TOTAL_STAGE;
				num2 = userState.GetSuccSoloBoss(level2);
				userState.AddSuccSoloBossStage(level2);
			}
			int a = (int)((float)lootCash * (1f + skill) * Mathf.Pow(0.5f, num2));
			a = Mathf.Max(a, 0);
			player.SetBossCash(a);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				int num3 = userState.GetNetStage() - Global.TOTAL_STAGE;
				Debug.Log("bossStage: " + num3);
				int succCoopBossGetMithril = userState.GetSuccCoopBossGetMithril(num3);
				Debug.Log("powerMithril: " + succCoopBossGetMithril);
				float num4 = Mathf.Pow(gameWorld.MithrilDrops.dropRateAttenuation, succCoopBossGetMithril);
				float num5 = Mathf.Max(gameWorld.MithrilDrops.dropRate * 100f * num4, 0f);
				float num6 = Random.Range(0f, 100f);
				if (num6 < num5)
				{
					int value = Random.Range(gameWorld.MithrilDrops.minDrop, gameWorld.MithrilDrops.maxDrop + 1);
					value = Mathf.Clamp(value, 0, 10);
					userState.AddSuccCoopBossGetMithril(num3);
					player.SetBossMithril(value);
				}
				UploadKillBossStateRequest request = new UploadKillBossStateRequest(userState);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			player.AddMonsterCash((int)((float)lootCash * (1f + skill)));
		}
		player.AddBounsCash((int)((float)lootCash * num));
		player.AddExp((int)((float)exp * (1f + skill2 + num)));
		GameApp.GetInstance().GetUserState().Achievement.KillEnemy();
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			GameApp.GetInstance().GetUserState().Achievement.TeamKillEnemy();
		}
	}

	public virtual bool OnSpecialState(float deltaTime)
	{
		return false;
	}

	public virtual EnemyState EnterSpecialState(float deltaTime)
	{
		return null;
	}

	public virtual void DoMove(float deltaTime)
	{
		enemyTransform.Translate(dir * runSpeed * deltaTime, Space.World);
		Vector3 ground = GetGround();
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground);
		enemyTransform.rotation = quaternion * enemyRotation;
	}

	public float GetSqrDistanceFromPlayer()
	{
		return (enemyTransform.position - player.GetTransform().position).sqrMagnitude;
	}

	public float GetHorizontalSqrDistanceFromPlayer()
	{
		Vector3 vector = player.GetTransform().position - enemyTransform.position;
		vector.y = 0f;
		return vector.sqrMagnitude;
	}

	public float GetHorizontalSqrDistanceFromTarget()
	{
		Vector3 vector = target.position - enemyTransform.position;
		vector.y = 0f;
		return vector.sqrMagnitude;
	}

	public virtual void DoShoutAudio()
	{
		if (shoutAudioTimer.Ready())
		{
			AudioManager.GetInstance().PlaySoundAt(shoutAudioName, enemyTransform.position);
			if (enemyTransform.position.y < -50f)
			{
				gameWorld.AddLostEnemy(EnemyID);
				DamageProperty damageProperty = new DamageProperty();
				damageProperty.damage = hp;
				damageProperty.criticalAttack = true;
				damageProperty.hitpoint = Vector3.down * -1000f;
				damageProperty.isLocal = true;
				damageProperty.wType = WeaponType.AssaultRifle;
				HitEnemy(damageProperty);
			}
			shoutAudioTimer.Do();
		}
	}

	public virtual void DoLogic(float deltaTime)
	{
		if (seePlayer)
		{
		}
		if (targetPlayer != null && targetPlayer.GetGameObject() == null)
		{
			targetPlayer = null;
			if (Lobby.GetInstance().IsMasterPlayer)
			{
				FindPath();
				FindNewTarget();
			}
		}
		if (targetPlayer != null)
		{
			state.NextState(this, deltaTime, targetPlayer);
		}
		else if (state == DEAD_STATE)
		{
			state.NextState(this, deltaTime, targetPlayer);
		}
		DoShoutAudio();
		if (mAim && mAimTimer.Ready())
		{
			mAim = false;
			isAiming = false;
			runSpeed /= 0.7f;
		}
		if (mIsSlowDown && mSlowDownTimer.Ready())
		{
			mIsSlowDown = false;
			runSpeed /= 0.7f;
		}
		if (recordTimer.Ready())
		{
			if (Vector3.Distance(lastPos, enemyTransform.position) > 1f)
			{
				lastMoveTime = Time.time;
			}
			lastPos = enemyTransform.position;
			recordTimer.Do();
		}
		if (GameApp.GetInstance().GetGameMode().IsSingle() && enemyType != EnemyType.Dragon && enemyType != EnemyType.Mantis && enemyType != EnemyType.Beetle && enemyType != EnemyType.MainMantis && enemyType != EnemyType.AssistMantis && enemyType != EnemyType.Earthworm && enemyType != EnemyType.SantaMachine && ((Time.time - gotHitTime > 15f && Time.time - lastAttackTime > 15f && Time.time - lastMoveTime > 15f) || Mathf.Abs(enemyTransform.position.x) > 100f || Mathf.Abs(enemyTransform.position.z) > 100f))
		{
			gameWorld.GetEnemies().Remove(enemyObject.name);
			enemyObject.SetActiveRecursively(false);
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.ENEMY_SPAWN_POINT);
			gameWorld.SpawnEnemy(enemyType, EnemyID, array[0].transform.position, IsElite);
			gameWorld.AddRespawnedEnemy();
		}
		if (gameWorld.State != 0 || !player.InPlayingState() || GameApp.GetInstance().GetGameMode().ModePlay != Mode.Boss)
		{
			return;
		}
		bool flag = false;
		if (EnemyType == EnemyType.Beetle || EnemyType == EnemyType.Dragon || EnemyType == EnemyType.Mantis || EnemyType == EnemyType.Earthworm || EnemyType == EnemyType.SantaMachine)
		{
			if (GetState() == DEAD_STATE && RemoveDeadBodyTimer())
			{
				flag = true;
			}
		}
		else if (EnemyType == EnemyType.MainMantis && (this as MainMantis).AreBothDead() && RemoveDeadBodyTimer())
		{
			flag = true;
		}
		if (flag)
		{
			player.OnWin();
			player.SetState(Player.WIN_STATE);
			if (GameApp.GetInstance().GetGameWorld().GetPlayer()
				.NeverGotHit)
			{
				GameApp.GetInstance().GetUserState().Achievement.CheckAchievement_Ghost();
			}
		}
	}

	public bool AnimationPlayed(string name, float percent)
	{
		if (animation[name].time >= animation[name].clip.length * percent)
		{
			return true;
		}
		return false;
	}

	public bool isAllPlayerDead()
	{
		bool flag = true;
		if (GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InPlayingState())
		{
			flag = false;
		}
		if (flag)
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item.InPlayingState())
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	public virtual bool RemoveDeadBodyTimer()
	{
		if (deadRemoveBodyTimer.Ready())
		{
			gameWorld.GetEnemies().Remove(EnemyName);
			enemyObject.SetActiveRecursively(false);
			enemyObject.transform.position = new Vector3(0f, 1000f, 0f);
		}
		return true;
	}

	public virtual Vector3 GetGround()
	{
		floorHeight = Global.FLOORHEIGHT;
		Vector3 vector = Vector3.up;
		ray = new Ray(enemyTransform.position + new Vector3(0f, 0.5f, 0f), Vector3.down);
		if (Physics.Raycast(ray, out rayhit, 10f, 1 << PhysicsLayer.FLOOR))
		{
			floorHeight = rayhit.point.y;
			vector = rayhit.normal;
		}
		if (vector.y < 0f)
		{
			vector = -vector;
		}
		return vector;
	}

	public virtual bool NeedShowDefent()
	{
		return false;
	}

	public float GetFloorHeight()
	{
		return floorHeight;
	}

	protected void CheckKnocked(float speed)
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

	public static GameObject GetEnemyByCollider(Collider c)
	{
		GameObject gameObject = c.gameObject;
		while (gameObject.transform.parent != null && !(gameObject.transform.parent.gameObject.tag == TagName.OBJECT_POOL))
		{
			gameObject = gameObject.transform.parent.gameObject;
		}
		return gameObject;
	}

	public void SlowDownEffect()
	{
		mSlowDownTimer.Do();
		if (!mIsSlowDown)
		{
			mIsSlowDown = true;
			runSpeed *= 0.7f;
		}
	}

	public void SetAimEffect()
	{
		mAimTimer.Do();
		if (!mAim)
		{
			mAim = true;
			isAiming = true;
			runSpeed *= 0.7f;
		}
	}

	public void HitEnemy(DamageProperty dp)
	{
		if (player.IsPowerUp(0))
		{
			dp.damage = (int)((float)dp.damage * 1.5f);
		}
		if (player.GetAttackItemAssist() > 0f)
		{
			dp.damage = (int)((float)dp.damage * (1f + player.GetAttackItemAssist()));
		}
		if (dp.wType == WeaponType.AdvancedSniper || dp.weaponid == 38)
		{
			SlowDownEffect();
		}
		if (dp.wType == WeaponType.TrackWave)
		{
			SetAimEffect();
		}
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			if (dp.isLocal && GetState() != GRAVEBORN_STATE)
			{
				int num = dp.damage;
				if (NeedShowDefent())
				{
					num /= 10;
				}
				Debug.Log("realDamage = " + num);
				EnemyOnHitRequest request = new EnemyOnHitRequest(EnemyID, (short)num, (byte)dp.wType, dp.criticalAttack, dp.weaponid);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		else
		{
			if (player.IsPowerUp(4))
			{
				dp.damage = (int)((float)dp.damage * 0.65f);
				player.Hp += (int)((float)dp.damage * 1f);
				player.Hp = Mathf.Clamp(player.Hp, 0, player.MaxHp);
			}
			else if (dp.wType == WeaponType.ImpactWave)
			{
				player.Hp += (int)((float)dp.damage * 0.5f);
				player.Hp = Mathf.Clamp(player.Hp, 0, player.MaxHp);
			}
			OnHit(dp);
		}
		if (!IsBoss() && GetLastBloodEffectTimer().Ready())
		{
			gameWorld.GetHitBloodPool().CreateObject(dp.hitpoint, Vector3.zero, Quaternion.identity);
			GetLastBloodEffectTimer().Do();
		}
	}

	public virtual Vector3 GetColliderCenterPosition()
	{
		return enemyTransform.position + Vector3.up * 0.8f;
	}

	public void SetGravityForceTarget(Vector3 gfTarget)
	{
		mGravityForceTarget = gfTarget;
	}

	public void SetStartGravityForceTimeNow()
	{
		mStartGravityFoceTime = Time.time;
	}

	public void StartGravityForceEffect()
	{
		if (mGravityForceBallObj == null)
		{
			Transform transform = enemyObject.transform.FindChild("GravityForceBall");
			if (transform != null)
			{
				mGravityForceBallObj = transform.gameObject;
			}
			else
			{
				GameObject original = Resources.Load("SW2_Effect/C12_Low_qiu") as GameObject;
				mGravityForceBallObj = Object.Instantiate(original) as GameObject;
				mGravityForceBallObj.name = "GravityForceBall";
				mGravityForceBallObj.transform.parent = enemyObject.transform;
				mGravityForceBallObj.transform.localPosition = Vector3.up;
				mGravityForceBallObj.transform.localRotation = Quaternion.identity;
			}
		}
		mGravityForceBallObj.transform.localScale = 2f * Vector3.one / mGravityForceBallObj.transform.parent.lossyScale.x;
		mGravityForceBallObj.SetActive(true);
		if (mGravityForceBeamObj == null)
		{
			Transform transform2 = enemyObject.transform.FindChild("GravityForceBeam");
			if (transform2 != null)
			{
				mGravityForceBeamObj = transform2.gameObject;
			}
			else
			{
				GameObject original2 = Resources.Load("SW2_Effect/C12_Low_gpizi") as GameObject;
				mGravityForceBeamObj = Object.Instantiate(original2) as GameObject;
				mGravityForceBeamObj.name = "GravityForceBeam";
				mGravityForceBeamObj.transform.parent = enemyObject.transform;
				mGravityForceBeamObj.transform.localPosition = Vector3.up;
			}
		}
		mGravityForceBeamObj.transform.LookAt(mGravityForceTarget + Vector3.up);
		mGravityForceBeamObj.transform.localScale = new Vector3(1f / mGravityForceBeamObj.transform.parent.lossyScale.x, 1f / mGravityForceBeamObj.transform.parent.lossyScale.y, (mGravityForceTarget - enemyObject.transform.position).magnitude / mGravityForceBeamObj.transform.parent.lossyScale.z);
		mGravityForceBeamObj.SetActive(true);
		AudioManager.GetInstance().PlaySoundAt("Audio/specialweapon/lightbow_hum", mGravityForceBeamObj.transform.position);
	}

	public void StopGravityForceEffect()
	{
		if (mGravityForceBallObj == null)
		{
			Transform transform = enemyObject.transform.FindChild("GravityForceBall");
			if (transform != null)
			{
				mGravityForceBallObj = transform.gameObject;
			}
		}
		if (mGravityForceBallObj != null)
		{
			mGravityForceBallObj.SetActive(false);
		}
		if (mGravityForceBeamObj == null)
		{
			Transform transform2 = enemyObject.transform.FindChild("GravityForceBeam");
			if (transform2 != null)
			{
				mGravityForceBeamObj = transform2.gameObject;
			}
		}
		if (mGravityForceBeamObj != null)
		{
			mGravityForceBeamObj.SetActive(false);
		}
	}

	public bool DoGravityForce()
	{
		if (Time.time - mStartGravityFoceTime > 2f)
		{
			return true;
		}
		Vector3 vector = mGravityForceTarget - enemyTransform.position;
		if (vector.x * vector.x + vector.z * vector.z > 4f)
		{
			dir = (mGravityForceTarget - enemyTransform.position).normalized;
			enemyTransform.Translate(dir * 20f * Time.deltaTime, Space.World);
			Vector3 ground = GetGround();
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground);
			enemyTransform.rotation = quaternion * enemyRotation;
			if (mGravityForceBeamObj != null)
			{
				mGravityForceBeamObj.transform.LookAt(mGravityForceTarget + Vector3.up);
				mGravityForceBeamObj.transform.localScale = new Vector3(1f / mGravityForceBeamObj.transform.parent.lossyScale.x, 1f / mGravityForceBeamObj.transform.parent.lossyScale.y, (mGravityForceTarget - enemyObject.transform.position).magnitude / mGravityForceBeamObj.transform.parent.lossyScale.z);
			}
		}
		else if (mGravityForceBeamObj != null)
		{
			mGravityForceBeamObj.SetActive(false);
		}
		return false;
	}
}
