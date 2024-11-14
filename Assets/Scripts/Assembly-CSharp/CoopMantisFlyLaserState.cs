using UnityEngine;

public class CoopMantisFlyLaserState : EnemyState
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
			coopMantis.PlayAnimation(AnimationString.MANTIS_LASER_180, WrapMode.ClampForever);
			coopMantis.FlySound();
			if (!coopMantis.CanShot && coopMantis.AnimationPlayed(AnimationString.MANTIS_LASER_180, 0.77f))
			{
				coopMantis.FlyLaserStop();
			}
			if (coopMantis.CanShot && coopMantis.AnimationPlayed(AnimationString.MANTIS_LASER_180, 0.41f))
			{
				coopMantis.CoopFlyLaserStart();
				coopMantis.CanShot = false;
			}
			if (coopMantis.AnimationPlayed(AnimationString.MANTIS_LASER_180, 1f))
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_IDLE_STATE);
				coopMantis.LaserFireStop();
				coopMantis.SetFlyIdleTimeNow();
				coopMantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
