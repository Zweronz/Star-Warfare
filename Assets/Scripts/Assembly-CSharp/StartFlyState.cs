using UnityEngine;

public class StartFlyState : EnemyState
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
			dragon.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
			if (dragon.AnimationPlayed(AnimationString.ENEMY_START_FLY, 0.37f))
			{
				dragon.LookAtTarget();
				dragon.StartFly();
			}
			if (dragon.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				dragon.SetState(Dragon.FLY_IDLE_STATE);
				dragon.SetFlyIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
