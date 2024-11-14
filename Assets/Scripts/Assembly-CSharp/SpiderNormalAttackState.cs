using UnityEngine;

public class SpiderNormalAttackState : EnemyState
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
			spider.PlayAnimation(AnimationString.SPIDER_NORMAL_ATTACK, WrapMode.ClampForever);
			spider.CheckHit();
			spider.NormalAttack();
			if (spider.AnimationPlayed(AnimationString.SPIDER_NORMAL_ATTACK, 1f))
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
