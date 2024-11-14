using UnityEngine;

public class SpiderInitState : EnemyState
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
		if (spider == null)
		{
			return;
		}
		if (spider.GetIdleTimeDuration() < 2f)
		{
			spider.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		spider.PlayAnimation(AnimationString.SPIDER_CONTINUOUS_SHOT, WrapMode.ClampForever);
		if (spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 0.2f))
		{
			spider.PlaySoundSingle("Audio/enemy/Spider/juxingjiachong");
		}
		if (spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 1f))
		{
			spider.SetIdleTimeNow();
			spider.SetState(Enemy.IDLE_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
