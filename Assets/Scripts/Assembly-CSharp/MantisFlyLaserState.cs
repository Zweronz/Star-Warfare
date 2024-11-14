using UnityEngine;

public class MantisFlyLaserState : EnemyState
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
		Mantis mantis = enemy as Mantis;
		if (mantis != null)
		{
			mantis.PlayAnimation(AnimationString.MANTIS_LASER, WrapMode.ClampForever);
			mantis.FlySound();
			if (mantis.CanShot)
			{
				mantis.LookAtTarget();
			}
			else if (mantis.AnimationPlayed(AnimationString.MANTIS_LASER, 0.83f))
			{
				mantis.FlyLaserStop();
			}
			if (mantis.CanShot && mantis.AnimationPlayed(AnimationString.MANTIS_LASER, 0.47f))
			{
				mantis.FlyLaserStart();
				mantis.CanShot = false;
			}
			if (mantis.AnimationPlayed(AnimationString.MANTIS_LASER, 1f))
			{
				mantis.SetState(Mantis.FLY_IDLE_STATE);
				mantis.LaserFireStop();
				mantis.SetFlyIdleTimeNow();
				mantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
