using System;
using UnityEngine;

public class Drone : Enemy
{
	protected Collider handCollider;

	protected Vector3 targetPosition;

	protected Vector3[] p = new Vector3[4];

	protected void RandomRunAnimation()
	{
		int num = Random.Range(0, 100);
		if (num < 40 || base.IsElite)
		{
			runAnimationName = AnimationString.ENEMY_RUN;
		}
		else if (num < 60)
		{
			runAnimationName = AnimationString.ENEMY_RUN01;
		}
		else
		{
			runAnimationName = AnimationString.ENEMY_RUN02;
		}
	}

	public override void OnDead()
	{
		base.OnDead();
		int num = Random.Range(0, 100);
		if (num < 50)
		{
			PlayAnimation(AnimationString.ENEMY_DEAD, WrapMode.ClampForever);
		}
		else
		{
			PlayAnimation(AnimationString.ENEMY_DEAD01, WrapMode.ClampForever);
		}
	}

	public override void Init(GameObject gObject)
	{
		base.Init(gObject);
		lastTarget = Vector3.zero;
		attackRange = 2.5f;
		localScale = Vector3.one * 0.7f;
		if (base.IsElite)
		{
			hp *= 3;
			runSpeed += 2f;
			attackDamage *= 2;
			animation[runAnimationName].speed = 1.5f;
		}
		RandomRunAnimation();
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

	public override void OnAttack()
	{
		base.OnAttack();
		PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		attacked = false;
		lastAttackTime = Time.time;
	}
}
