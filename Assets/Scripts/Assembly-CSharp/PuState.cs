using UnityEngine;

public class PuState : EnemyState
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
			dragon.PlayAnimation(dragon.PuAnimation, WrapMode.ClampForever);
			dragon.LookAtTarget();
			dragon.CheckPuHit();
			if (dragon.AnimationPlayed(dragon.PuAnimation, 0.11f))
			{
				dragon.Pu();
			}
			if (dragon.AnimationPlayed(dragon.PuAnimation, 1f))
			{
				dragon.SetState(Enemy.IDLE_STATE);
				dragon.SetGroundIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
