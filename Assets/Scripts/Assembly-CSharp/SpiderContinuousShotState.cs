using UnityEngine;

public class SpiderContinuousShotState : EnemyState
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
			spider.PlayAnimation(AnimationString.SPIDER_CONTINUOUS_SHOT, WrapMode.ClampForever);
			spider.DoContinuousShot();
			if (spider.CanShot)
			{
				spider.LookAtTarget();
			}
			if (spider.CanShot && spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 0.27f))
			{
				spider.StartContinuousShot();
				spider.CanShot = false;
			}
			if (!spider.CanShot && spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 0.73f))
			{
				spider.StopContinuousShot();
			}
			if (spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 1f))
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
