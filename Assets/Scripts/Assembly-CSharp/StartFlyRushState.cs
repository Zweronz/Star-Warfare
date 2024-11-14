using UnityEngine;

public class StartFlyRushState : EnemyState
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
		dragon.EnableGravity(false);
		if (dragon != null)
		{
			dragon.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
			dragon.LookAtTarget();
			if (dragon.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				dragon.SetState(Dragon.FLY_RUSH_STATE);
				dragon.OnRush();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
