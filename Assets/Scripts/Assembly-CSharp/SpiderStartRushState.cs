using UnityEngine;

public class SpiderStartRushState : EnemyState
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
		Spider spider = enemy as Spider;
		if (spider != null)
		{
			spider.PlayAnimation(AnimationString.SPIDER_START_RUSH, WrapMode.ClampForever);
			spider.LookAtTarget();
			if (spider.AnimationPlayed(AnimationString.SPIDER_START_RUSH, 1f))
			{
				spider.SetState(Spider.RUSH_STATE);
				spider.OnRush();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
