using UnityEngine;

public class LandingState : EnemyState
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
		Dragon dragon = enemy as Dragon;
		if (dragon == null)
		{
			return;
		}
		dragon.PlayAnimation(AnimationString.ENEMY_LANDING, WrapMode.ClampForever);
		dragon.Land();
		if (dragon.AnimationPlayed(AnimationString.ENEMY_LANDING, 1f))
		{
			if (dragon.NeedLandToRage)
			{
				dragon.NeedLandToRage = false;
				dragon.SetState(Dragon.RAGE_STATE);
			}
			else
			{
				dragon.SetState(Enemy.IDLE_STATE);
				dragon.SetGroundIdleTimeNow();
				dragon.SetLandingTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
