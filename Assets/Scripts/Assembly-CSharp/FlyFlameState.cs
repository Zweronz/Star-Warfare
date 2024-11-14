using UnityEngine;

public class FlyFlameState : EnemyState
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
		if (dragon != null)
		{
			dragon.FlyOnFlame();
			dragon.FlyCheckFlame();
			if (dragon.AnimationPlayed(AnimationString.ENEMY_FLY_ATTACK01, 1f))
			{
				dragon.SetState(Dragon.FLY_IDLE_STATE);
				dragon.SetFlyIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
