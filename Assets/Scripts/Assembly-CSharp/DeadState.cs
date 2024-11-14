public class DeadState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		enemy.RemoveDeadBodyTimer();
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
