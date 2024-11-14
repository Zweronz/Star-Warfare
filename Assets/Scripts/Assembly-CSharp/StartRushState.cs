using UnityEngine;

public class StartRushState : EnemyState
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
		Tank tank = enemy as Tank;
		if (tank != null)
		{
			if (tank.NeedLookAtTarget)
			{
				tank.LookAtTargetPlayer();
			}
			if (tank.AnimationPlayed(AnimationString.TANK_STARTRUSH, 1f))
			{
				tank.SetState(Tank.RUSH_STATE);
				tank.OnRush();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
