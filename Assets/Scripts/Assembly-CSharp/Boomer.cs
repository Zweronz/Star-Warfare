using UnityEngine;

public class Boomer : Enemy
{
	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected float explodeRadius;

	protected bool exploded;

	protected void RandomRunAnimation()
	{
		runAnimationName = AnimationString.ENEMY_RUN;
	}

	public override void OnDead()
	{
		base.OnDead();
		StopAnimation();
		GameObject original = Resources.Load("Effect/RPG_EXP") as GameObject;
		Object.Instantiate(original, enemyTransform.position, Quaternion.identity);
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		lastTarget = Vector3.zero;
		localScale = Vector3.one * 0.7f;
		RandomRunAnimation();
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		shoutAudioName = "Audio/enemy/zibaochong";
		attackRange = 2.5f;
		bloodColor = BloodColor.Green;
		exploded = false;
	}

	public override void CheckHit()
	{
		if (!exploded)
		{
			if ((player.GetTransform().position - enemyTransform.position).sqrMagnitude < bombRange * bombRange)
			{
				player.OnHit(attackDamage);
			}
			exploded = true;
		}
	}

	public override void DoLogic(float deltaTime)
	{
		base.DoLogic(deltaTime);
	}

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyOnHitRequest request = new EnemyOnHitRequest(base.EnemyID, (short)base.HP, 0, true, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				return;
			}
			DamageProperty damageProperty = new DamageProperty();
			damageProperty.damage = base.HP;
			damageProperty.isLocal = true;
			damageProperty.criticalAttack = true;
			damageProperty.wType = WeaponType.NoGun;
			OnHit(damageProperty);
		}
	}
}
