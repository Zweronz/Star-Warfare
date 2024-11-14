using UnityEngine;

public class SpiderShotState : EnemyState
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
			spider.PlayAnimation(AnimationString.SPIDER_SHOT, WrapMode.ClampForever);
			if (spider.CanShot)
			{
				spider.LookAtTarget();
			}
			if (spider.CanShot && spider.AnimationPlayed(AnimationString.SPIDER_SHOT, 0.48f))
			{
				spider.Shot();
				spider.CanShot = false;
			}
			if (spider.AnimationPlayed(AnimationString.SPIDER_SHOT, 1f))
			{
				spider.SetState(Enemy.IDLE_STATE);
				spider.SetIdleTimeNow();
				spider.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
