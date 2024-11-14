using UnityEngine;

public class SpiderStartJumpState : EnemyState
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
		if (!spider.CanJump)
		{
			spider.PlayAnimation(AnimationString.ENEMY_JUMPSTART, WrapMode.ClampForever);
			if (spider.AnimationPlayed(AnimationString.ENEMY_JUMPSTART, 0.54f))
			{
				spider.CanJump = true;
				spider.PlaySoundSingle("Audio/enemy/Spider/juxingjiachong_jump1");
			}
		}
		else if (!spider.Jump(deltaTime))
		{
			if (spider.AnimationPlayed(AnimationString.ENEMY_JUMPSTART, 1f))
			{
				spider.PlayAnimation(AnimationString.ENEMY_JUMPING, WrapMode.Loop);
			}
			else
			{
				spider.PlayAnimation(AnimationString.ENEMY_JUMPSTART, WrapMode.ClampForever);
			}
		}
		else
		{
			spider.SetState(Spider.JUMP_STATE);
			spider.SetJumpIdleTimeNow();
			spider.CanJump = false;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
