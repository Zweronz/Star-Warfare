using UnityEngine;

public class AssistMantisInitStartFlyState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Transform transform = enemy.GetTransform();
		AssistMantis assistMantis = enemy as AssistMantis;
		assistMantis.EnableGravity(false);
		if (assistMantis != null)
		{
			assistMantis.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
			assistMantis.StartFlyHigh();
			assistMantis.LookAtTargetPoint();
			if (assistMantis.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				assistMantis.SetState(AssistMantis.INIT_LANDING_STATE);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
