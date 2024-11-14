using UnityEngine;

public class CoopMantisFlyDiveEndState : EnemyState
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
		CoopMantis coopMantis = enemy as CoopMantis;
		if (coopMantis != null)
		{
			coopMantis.PlayAnimation(AnimationString.MANTIS_RUSH_END, WrapMode.ClampForever);
			coopMantis.FlyUp();
			if (coopMantis.AnimationPlayed(AnimationString.MANTIS_RUSH_END, 1f))
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_IDLE_STATE);
				coopMantis.SetFlyIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
