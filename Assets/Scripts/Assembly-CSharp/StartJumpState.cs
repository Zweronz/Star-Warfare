using UnityEngine;

public class StartJumpState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
		}
		else
		{
			Transform transform = enemy.GetTransform();
			Zergling zergling = enemy as Zergling;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
