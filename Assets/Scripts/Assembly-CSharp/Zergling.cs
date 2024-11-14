using System;
using UnityEngine;

public class Zergling : Enemy
{
	public static EnemyState STARTJUMP_STATE = new StartJumpState();

	public static EnemyState JUMP_STATE = new JumpState();

	public static EnemyState LOOKAROUND_STATE = new LookAroundState();

	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected float lastRushingTime;

	protected bool jumpended;

	protected float lookAroundStartTime;

	public Vector3 speed;

	public bool JumpEnded
	{
		get
		{
			return jumpended;
		}
	}

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
		attackRange = 2.8f;
		localScale = Vector3.one * 0.8f;
		rushDamage = monsterConfig.attack[1].damage;
		RandomRunAnimation();
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			rushDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		shoutAudioName = "Audio/enemy/gaosuchong";
	}

	public override void CheckHit()
	{
		if (!attacked && AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.4f))
		{
			Vector3 vector = enemyTransform.InverseTransformPoint(player.GetTransform().position);
			if (Vector3.Distance(enemyTransform.position, player.GetTransform().position) < 3f && vector.z > 0f && Mathf.Abs(vector.z / vector.x) > Mathf.Tan((float)Math.PI / 3f))
			{
				player.OnHit(attackDamage);
				attacked = true;
			}
		}
	}

	public void CheckPuAttack()
	{
		Collider collider = player.GetCollider();
		if (collider != null && !attacked && base.collider.bounds.Intersects(collider.bounds))
		{
			player.UnderAttack(rushDamage);
			attacked = true;
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && state != JUMP_STATE && state != Enemy.GRAVEBORN_STATE)
		{
			CharacterController characterController = enemyObject.collider as CharacterController;
			if (characterController != null)
			{
				characterController.Move(new Vector3(0f, -5f, 0f) * deltaTime);
			}
		}
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
		if (Time.time - lastRushingTime > 3f && Time.time - lookAroundStartTime > 4f)
		{
			int num = Random.Range(0, 100);
			if (num < 10)
			{
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(base.EnemyID, 2, GetTransform().position, Vector3.zero);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
				SetState(LOOKAROUND_STATE);
				lookAroundStartTime = Time.time;
				spawnCenter = enemyTransform.position;
			}
			else if (ReadyForJump())
			{
				LookAtTarget();
				dir = enemyTransform.forward;
				PlayAnimation(AnimationString.ENEMY_JUMP, WrapMode.ClampForever);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(base.EnemyID, 3, GetTransform().position, speed);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
				result = JUMP_STATE;
				attacked = false;
			}
		}
		return result;
	}

	public override void DoMove(float deltaTime)
	{
		CharacterController characterController = enemyObject.collider as CharacterController;
		if (characterController != null)
		{
			characterController.Move((dir * runSpeed + Vector3.down * 10f) * deltaTime);
		}
		Vector3 ground = GetGround();
		Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, ground);
		enemyTransform.rotation = quaternion * enemyRotation;
	}

	public bool ReadyForJump()
	{
		if (seePlayer && Time.time - lastRushingTime > 3.5f && (enemyTransform.position - target.position).sqrMagnitude > 25f && (enemyTransform.position - target.position).sqrMagnitude < 225f)
		{
			float num = animation[AnimationString.ENEMY_JUMP].clip.length * 0.5f;
			float num2 = Mathf.Sqrt(base.SqrDistanceFromPlayer);
			speed = dir * (num2 / num);
			lastRushingTime = Time.time;
			return true;
		}
		return false;
	}

	public bool JumpInOne(float deltaTime)
	{
		CharacterController characterController = enemyObject.collider as CharacterController;
		if (AnimationPlayed(AnimationString.ENEMY_JUMP, 0.2f) && !AnimationPlayed(AnimationString.ENEMY_JUMP, 0.66f) && characterController != null)
		{
			characterController.Move(speed * deltaTime);
		}
		if (AnimationPlayed(AnimationString.ENEMY_JUMP, 1f))
		{
			return true;
		}
		return false;
	}

	public bool LookAroundTimOut()
	{
		if (Time.time - lookAroundStartTime > 1f)
		{
			return true;
		}
		return false;
	}
}
