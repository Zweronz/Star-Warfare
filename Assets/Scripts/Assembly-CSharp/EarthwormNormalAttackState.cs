using UnityEngine;

public class EarthwormNormalAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.EARTHWORM_NORMAL_ATTACK, WrapMode.ClampForever, 1.5f);
			earthworm.CheckHit();
			earthworm.CheckNormalAttackHit();
			earthworm.NormalAttack();
			if (!earthworm.AnimationPlayed(AnimationString.EARTHWORM_NORMAL_ATTACK, 0.64f))
			{
				earthworm.LookAtTargetInNormalAttack();
			}
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_NORMAL_ATTACK, 1f))
			{
				earthworm.SetState(Enemy.IDLE_STATE);
				earthworm.SetIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
