using UnityEngine;

public class SpiderGotHitState : EnemyState
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
		spider.PlayAnimation(AnimationString.ENEMY_GOTHIT, WrapMode.ClampForever, 1f);
		if (spider.AnimationPlayed(AnimationString.ENEMY_GOTHIT, 1f))
		{
			if (spider.NeedRage())
			{
				spider.SetState(Spider.RAGE_STATE);
				return;
			}
			spider.SetState(Enemy.IDLE_STATE);
			spider.SetIdleTimeNow();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
