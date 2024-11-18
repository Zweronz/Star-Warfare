using UnityEngine;

public class Mutalisk : Enemy
{
	public const float FLY_HEIGHT = 2.4f;

	public static int ShotAttackDamage;

	public static int ShotAreaDamage;

	public static float ShotExplodeRadius;

	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected void RandomRunAnimation()
	{
		runAnimationName = AnimationString.ENEMY_RUN;
	}

	public override void OnDead()
	{
		if (enemyObject.GetComponent<Rigidbody>() != null)
		{
			enemyTransform.GetComponent<Rigidbody>().useGravity = true;
		}
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
		shoutAudioName = "Audio/enemy/feixingchong";
		enemyTransform.GetComponent<Rigidbody>().useGravity = false;
		enemyTransform.GetComponent<Rigidbody>().isKinematic = true;
		ShotAttackDamage = attackDamage;
		ShotAreaDamage = (int)monsterConfig.attack[0].splashDamage;
		ShotExplodeRadius = monsterConfig.attack[0].splashRange;
	}

	public override void SetInGrave(bool inGrave)
	{
		if (inGrave)
		{
			SetState(Enemy.GRAVEBORN_STATE);
			enemyTransform.Translate(Vector3.down * 2f);
			enemyObject.layer = PhysicsLayer.Default;
			if (enemyObject.GetComponent<Rigidbody>() != null)
			{
				enemyTransform.GetComponent<Rigidbody>().useGravity = false;
			}
		}
		else
		{
			enemyObject.layer = PhysicsLayer.ENEMY;
			enemyTransform.GetComponent<Rigidbody>().useGravity = true;
		}
	}

	public override bool MoveFromGrave(float deltaTime)
	{
		enemyTransform.Translate(Vector3.up * deltaTime * 2f);
		if (enemyTransform.position.y >= (float)Global.FLOORHEIGHT)
		{
			return true;
		}
		return false;
	}

	public override void CheckHit()
	{
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && !attacked && AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.6f))
		{
			Vector3 vector = new Vector3(enemyTransform.position.x, enemyTransform.position.y + 0.8f, enemyTransform.position.z);
			float num = 1f;
			Vector3 vector2 = new Vector3(target.position.x, target.position.y + 0.6f, target.position.z) - vector;
			float num2 = 12f;
			Vector3 vector3 = vector2.normalized * num2;
			GameObject original = Resources.Load("Effect/Mutalisk/Mutalisk_Shot") as GameObject;
			GameObject gameObject = Object.Instantiate(original, vector, Quaternion.LookRotation(-vector3)) as GameObject;
			EnemyShotScript component = gameObject.GetComponent<EnemyShotScript>();
			component.speed = vector3;
			component.attackDamage = attackDamage;
			component.areaDamage = (int)monsterConfig.attack[0].splashDamage;
			component.trType = TrajectoryType.Straight;
			component.damageType = DamageType.Sputtering;
			component.explodeRadius = monsterConfig.attack[0].splashRange;
			component.enemyType = enemyType;
			attacked = true;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyShotRequest request = new EnemyShotRequest(6, vector, vector3);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public override void FindPath()
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
			ChangeTarget(target.position + Vector3.up * 1.9000001f, Lobby.GetInstance().GetChannelID(), false);
		}
		GetGround();
		Vector3 vector = new Vector3(enemyTransform.position.x, floorHeight, enemyTransform.position.z);
		ray = new Ray(vector + new Vector3(0f, 0.5f, 0f), target.position + new Vector3(0f, 0.5f, 0f) - (vector + new Vector3(0f, 0.5f, 0f)));
		if (!Physics.Raycast(ray, out rayhit, 100f, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.PLAYER) | (1 << PhysicsLayer.REMOTE_PLAYER)))
		{
			return;
		}
		if ((targetPlayer.IsLocal() && rayhit.collider.gameObject.layer == PhysicsLayer.PLAYER) || (!targetPlayer.IsLocal() && rayhit.collider.gameObject.layer == PhysicsLayer.REMOTE_PLAYER))
		{
			ChangeTarget(position, targetPlayer.GetUserID(), false);
			aStarPathFinding.Reset();
			seePlayer = true;
			return;
		}
		seePlayer = false;
		if (aStarPathFinding.GetNextWayPoint(vector, targetPlayer, this) && aStarPathFinding.Target != null)
		{
			ChangeTarget(aStarPathFinding.Target.Position + Vector3.up * 1.9000001f, 0, false);
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		GetGround();
		if (state == Enemy.DEAD_STATE)
		{
			collider.GetComponent<Rigidbody>().useGravity = true;
			collider.GetComponent<Rigidbody>().isKinematic = false;
			return;
		}
		float num = 0f;
		if (state == Enemy.DEAD_STATE)
		{
			num = floorHeight + 0.2f - enemyTransform.position.y;
			return;
		}
		float y = floorHeight + 2.4f;
		enemyTransform.position = Vector3.Lerp(enemyTransform.position, new Vector3(enemyTransform.position.x, y, enemyTransform.position.z), Time.time);
	}

	public override bool CouldEnterAttackState()
	{
		if (base.SqrDistanceFromPlayer < base.AttackRange * base.AttackRange && seePlayer)
		{
			return true;
		}
		return false;
	}

	public override Vector3 GetGround()
	{
		floorHeight = Global.FLOORHEIGHT;
		Vector3 vector = Vector3.up;
		ray = new Ray(enemyTransform.position + new Vector3(0f, 4.5f, 0f), Vector3.down);
		if (Physics.Raycast(ray, out rayhit, 100f, 1 << PhysicsLayer.FLOOR))
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

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		attacked = false;
		lastAttackTime = Time.time;
	}
}
