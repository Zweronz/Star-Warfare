using UnityEngine;

public class EarthwormDoubleAttackState : EnemyState
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
			earthworm.PlayAnimation(AnimationString.EARTHWORM_DOUBLE_ATTACK, WrapMode.ClampForever);
			earthworm.CheckHit();
			earthworm.NormalAttack();
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_DOUBLE_ATTACK, 1f))
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
