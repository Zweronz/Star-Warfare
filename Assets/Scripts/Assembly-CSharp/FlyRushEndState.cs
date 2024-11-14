using UnityEngine;

public class FlyRushEndState : EnemyState
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
		Dragon dragon = enemy as Dragon;
		if (dragon != null)
		{
			dragon.PlayAnimation(AnimationString.ENEMY_FLY_RUSH_END, WrapMode.ClampForever);
			dragon.PlaySoundSingle("Audio/enemy/Dragon/long_luodi");
			if (dragon.AnimationPlayed(AnimationString.ENEMY_FLY_RUSH_END, 1f))
			{
				dragon.SetState(Enemy.IDLE_STATE);
				dragon.SetGroundIdleTimeNow();
				dragon.SetLandingTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
