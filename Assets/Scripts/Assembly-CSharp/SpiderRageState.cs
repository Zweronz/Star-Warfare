using UnityEngine;

public class SpiderRageState : EnemyState
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
			spider.PlayAnimation(AnimationString.SPIDER_CONTINUOUS_SHOT, WrapMode.ClampForever);
			if (spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 0.2f))
			{
				spider.PlaySoundSingle("Audio/enemy/Spider/juxingjiachong");
			}
			if (spider.AnimationPlayed(AnimationString.SPIDER_CONTINUOUS_SHOT, 1f))
			{
				spider.SetState(Enemy.IDLE_STATE);
				spider.SetIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
