using UnityEngine;

public class AssistMantisInitLandingState : EnemyState
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
		if (assistMantis != null)
		{
			assistMantis.PlayAnimation(AnimationString.ENEMY_LANDING, WrapMode.ClampForever);
			assistMantis.Land();
			if (assistMantis.AnimationPlayed(AnimationString.ENEMY_LANDING, 1f))
			{
				assistMantis.SetState(AssistMantis.WATCH_IDLE_STATE);
				assistMantis.SetGroundIdleTimeNow();
				assistMantis.SetLandingTimeNow();
				assistMantis.StopSound("feixingtanglang_fly_idle");
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
