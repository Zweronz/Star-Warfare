using UnityEngine;

public class FlyRushState : EnemyState
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
		dragon.PlayAnimation(AnimationString.ENEMY_FLY_RUSH, WrapMode.Loop);
		dragon.Rush();
		dragon.CheckRushHit();
		if (dragon.CloseToRushTarget())
		{
			if (dragon.isFlying())
			{
				dragon.SetState(Dragon.START_FLY_STATE);
			}
			else
			{
				dragon.SetState(Dragon.FLY_RUSH_END_STATE);
				dragon.EnableGravity(true);
			}
			dragon.EnableTrailEffect(false);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
