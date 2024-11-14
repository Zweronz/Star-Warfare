public class FlameState : EnemyState
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
			dragon.OnFlame();
			dragon.CheckFlame();
			if (dragon.AnimationPlayed(AnimationString.ENEMY_ATTACK02, 1f))
			{
				dragon.SetState(Enemy.IDLE_STATE);
				dragon.SetGroundIdleTimeNow();
			}
		}
	}
}
