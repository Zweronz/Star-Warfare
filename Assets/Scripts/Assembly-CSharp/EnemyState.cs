public abstract class EnemyState
{
	public virtual void NextState(Enemy enemy, float deltaTime, Player player)
	{
	}

	public virtual void OnHit(Enemy enemy)
	{
		enemy.SetState(Enemy.GOTHIT_STATE);
	}
}
