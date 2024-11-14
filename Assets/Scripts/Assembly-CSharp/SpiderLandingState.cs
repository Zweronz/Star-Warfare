using UnityEngine;

public class SpiderLandingState : EnemyState
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
		if (!spider.HasLanded)
		{
			spider.Land(deltaTime);
		}
		if (Time.time < spider.StartLandTime)
		{
			spider.PlayAnimation(AnimationString.ENEMY_JUMPING, WrapMode.Loop);
			return;
		}
		spider.PlayAnimation(AnimationString.ENEMY_JUMPGEND, WrapMode.ClampForever);
		if (spider.AnimationPlayed(AnimationString.ENEMY_JUMPGEND, 1f))
		{
			spider.SetState(Enemy.IDLE_STATE);
			spider.SetIdleTimeNow();
			spider.EnableGravity(true);
			spider.LookAtTarget();
			spider.HasLanded = false;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
