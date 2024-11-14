using UnityEngine;

public class SpiderDoubleAttackState : EnemyState
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
			spider.PlayAnimation(AnimationString.SPIDER_DOUBLE_ATTACK, WrapMode.ClampForever);
			spider.CheckHit();
			spider.DoubleAttack();
			if (spider.AnimationPlayed(AnimationString.SPIDER_DOUBLE_ATTACK, 1f))
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
