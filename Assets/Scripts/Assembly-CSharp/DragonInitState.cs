using UnityEngine;

public class DragonInitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Dragon dragon = enemy as Dragon;
		if (dragon == null)
		{
			return;
		}
		if (dragon.GetGroundIdleDuration() < 2f)
		{
			dragon.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		dragon.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
		dragon.OnRage();
		if (dragon.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
		{
			int farthestPlayer = dragon.GetFarthestPlayer();
			dragon.ChangeTargetPlayer(farthestPlayer);
			dragon.LookAtTarget();
			dragon.SetFlyTimeNow();
			dragon.SetState(Dragon.START_FLY_STATE);
			dragon.EnableGravity(false);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
