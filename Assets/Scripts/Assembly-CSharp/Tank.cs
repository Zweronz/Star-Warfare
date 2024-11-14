using System;
using UnityEngine;

public class Tank : Enemy
{
	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	public static EnemyState STARTRUSH_STATE = new StartRushState();

	public static EnemyState RUSH_STATE = new RushState();

	protected float lastRushingTime;

	protected float rushSpeed;

	protected bool rushCollidesWall;

	protected float lastGotHitStateTime;

	protected bool rushed;

	public bool NeedLookAtTarget { get; set; }

	protected void RandomRunAnimation()
	{
		runAnimationName = AnimationString.ENEMY_RUN;
	}

	public override void OnDead()
	{
		base.OnDead();
		PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		lastTarget = Vector3.zero;
		localScale = Vector3.one * 0.018f;
		attackFrequency = monsterConfig.attack[2].attackRate;
		attackDamage = monsterConfig.attack[2].damage;
		rushDamage = monsterConfig.attack[0].damage;
		attackRange = 5f;
		rushSpeed = monsterConfig.attack[0].moveSpeed;
		RandomRunAnimation();
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		shoutAudioName = "Audio/enemy/daxingjiachong";
		rushed = false;
		lastGotHitStateTime = Time.time;
		wayPointSqrDistance = 5f;
	}

	public override void CheckHit()
	{
		if (!attacked && AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.4f))
		{
			Vector3 vector = enemyTransform.InverseTransformPoint(player.GetTransform().position);
			if (Vector3.Distance(enemyTransform.position, player.GetTransform().position) < 6f && vector.z > 0f && Mathf.Abs(vector.z / vector.x) > Mathf.Tan((float)Math.PI / 3f))
			{
				player.OnHit(attackDamage);
				attacked = true;
			}
		}
	}

	public void RushCollidesWall()
	{
		rushCollidesWall = true;
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.GRAVEBORN_STATE)
		{
			beWokeUp = true;
			hp -= dp.damage;
			criticalAttacked = false;
			if (state != STARTRUSH_STATE && state != RUSH_STATE && Time.time - lastGotHitStateTime > 10f)
			{
				gotHitTime = Time.time;
				lastGotHitStateTime = Time.time;
				SetState(Enemy.GOTHIT_STATE);
			}
		}
	}

	public override void SetCriticalAttack(bool criticalAttack)
	{
		base.SetCriticalAttack(false);
	}

	public override void OnHitResponse()
	{
		beWokeUp = true;
		gameWorld.GetHitBloodPool().CreateObject(GetPosition(), Vector3.zero, Quaternion.identity);
		if (state != STARTRUSH_STATE && state != RUSH_STATE && Time.time - lastGotHitStateTime > 10f)
		{
			gotHitTime = Time.time;
			lastGotHitStateTime = Time.time;
			SetState(Enemy.GOTHIT_STATE);
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public void OnStartRush()
	{
		lastRushingTime = Time.time;
		rushed = false;
		PlayAnimation(AnimationString.TANK_STARTRUSH, WrapMode.Loop, 1.6f);
		if ((targetPlayer.GetTransform().position - enemyTransform.position).sqrMagnitude > 64f)
		{
			NeedLookAtTarget = true;
		}
		else
		{
			NeedLookAtTarget = false;
		}
		LookAtTargetPlayer();
	}

	public void OnRush()
	{
		if (NeedLookAtTarget)
		{
			dir = (targetPlayer.GetTransform().position - enemyTransform.position).normalized;
		}
	}

	public bool Rush()
	{
		enemyTransform.Translate(dir * rushSpeed * Time.deltaTime, Space.World);
		if (!rushed && collider.bounds.Intersects(player.GetCollider().bounds))
		{
			player.OnHit(rushDamage);
			CheckKnocked(0.08f);
			rushed = true;
		}
		if (Time.time - lastRushingTime > 5f || base.SqrDistanceFromPlayer < 1f || rushCollidesWall)
		{
			rushed = false;
			return true;
		}
		return false;
	}

	public bool CouldRush()
	{
		bool result = false;
		if (seePlayer)
		{
			bool flag = false;
			bool flag2 = false;
			RaycastHit hitInfo = default(RaycastHit);
			Vector3 vector = enemyTransform.position + new Vector3(0f, 0.5f, 0f) + enemyTransform.right * 1f;
			Ray ray = new Ray(vector, target.position + new Vector3(0f, 0.5f, 0f) - vector);
			if (Physics.Raycast(ray, out hitInfo, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER)) && ((targetPlayer.IsLocal() && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER) || (!targetPlayer.IsLocal() && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)))
			{
				flag = true;
			}
			hitInfo = default(RaycastHit);
			vector = enemyTransform.position + new Vector3(0f, 0.5f, 0f) - enemyTransform.right * 1f;
			ray = new Ray(vector, target.position + new Vector3(0f, 0.5f, 0f) - vector);
			if (Physics.Raycast(ray, out hitInfo, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER)) && ((targetPlayer.IsLocal() && hitInfo.collider.gameObject.layer == PhysicsLayer.PLAYER) || (!targetPlayer.IsLocal() && hitInfo.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER)))
			{
				flag2 = true;
			}
			result = flag && flag2;
		}
		return result;
	}

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		attacked = false;
		lastAttackTime = Time.time;
	}

	public override EnemyState EnterSpecialState(float deltaTime)
	{
		EnemyState result = null;
		if (Time.time - lastRushingTime > 10f && CouldRush())
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 4, GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			rushCollidesWall = false;
			SetState(STARTRUSH_STATE);
			OnStartRush();
		}
		return result;
	}
}
