using UnityEngine;

public class AssistMantisWatchState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		AssistMantis assistMantis = enemy as AssistMantis;
		if (assistMantis != null)
		{
			assistMantis.PlayAnimation(AnimationString.MANTIS_WATCH, WrapMode.ClampForever);
			if (assistMantis.AnimationPlayed(AnimationString.MANTIS_WATCH, 1f))
			{
				assistMantis.SetState(AssistMantis.WATCH_IDLE_STATE);
				assistMantis.SetGroundIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
