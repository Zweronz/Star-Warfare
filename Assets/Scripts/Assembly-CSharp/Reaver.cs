using UnityEngine;

public class Reaver : Enemy
{
	public static int ShotAttackDamage;

	public static int ShotAreaDamage;

	public static float ShotExplodeRadius;

	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected float lastGotHitStateTime;

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
		RandomRunAnimation();
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		shoutAudioName = "Audio/enemy/jinjiachong";
		lastGotHitStateTime = Time.time;
		ShotAttackDamage = attackDamage;
		ShotAreaDamage = attackDamage;
		ShotExplodeRadius = monsterConfig.attack[0].bombRange;
	}

	public override void CheckHit()
	{
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && !attacked && AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.6f))
		{
			GetGround();
			Vector3 vector = new Vector3(enemyTransform.position.x, floorHeight, enemyTransform.position.z);
			float num = -2f;
			Vector3 vector2 = new Vector3(target.position.x, target.position.y + 1.5f, target.position.z) - vector;
			float magnitude = vector2.magnitude;
			float num2 = 12f;
			float num3 = magnitude / num2;
			float num4 = (num - 0.5f * Physics.gravity.y * num3 * num3) / num3;
			Vector3 vector3 = Vector3.up * num4 + vector2.normalized * num2;
			GameObject original = Resources.Load("Effect/Reaver/Reaver_Shot") as GameObject;
			GameObject gameObject = Object.Instantiate(original, vector + Vector3.up * (0f - num), Quaternion.LookRotation(-vector3)) as GameObject;
			EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
			component.trType = TrajectoryType.Parabola;
			component.damageType = DamageType.Explosion;
			component.speed = vector3;
			component.attackDamage = attackDamage;
			component.areaDamage = attackDamage;
			component.explodeRadius = monsterConfig.attack[0].bombRange;
			component.enemyType = enemyType;
			attacked = true;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyShotRequest request = new EnemyShotRequest(5, vector + Vector3.up * (0f - num), vector3);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public override void OnHit(DamageProperty dp)
	{
		if (state != Enemy.GRAVEBORN_STATE)
		{
			beWokeUp = true;
			hp -= dp.damage;
			criticalAttacked = false;
			if (Time.time - lastGotHitStateTime > 11f)
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
		if (Time.time - lastGotHitStateTime > 11f)
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

	protected override bool RaycastTargetPlayer(out RaycastHit rayhit)
	{
		ray = new Ray(enemyTransform.position + new Vector3(0f, 0.5f, 0f), target.position + new Vector3(0f, 0.5f, 0f) - (enemyTransform.position + new Vector3(0f, 0.5f, 0f)));
		return Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PATH_FINDING_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER));
	}

	public override bool CouldEnterAttackState()
	{
		if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange && seePlayer)
		{
			return true;
		}
		return false;
	}

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		attacked = false;
		lastAttackTime = Time.time;
	}
}
