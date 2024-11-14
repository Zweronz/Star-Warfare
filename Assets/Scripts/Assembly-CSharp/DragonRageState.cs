using UnityEngine;

public class DragonRageState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Dragon dragon = enemy as Dragon;
		if (dragon != null)
		{
			dragon.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
			dragon.OnRage();
			if (dragon.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
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
