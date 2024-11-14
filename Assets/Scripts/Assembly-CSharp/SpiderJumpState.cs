public class SpiderJumpState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Spider spider = enemy as Spider;
		if (spider != null)
		{
			spider.PlayAnimation(AnimationString.ENEMY_JUMPING);
			if (spider.GetJumpIdleTimeDuration() > spider.MaxJumpIdleTime)
			{
				spider.SetState(Spider.LANDING_STATE);
				spider.StartLand();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
