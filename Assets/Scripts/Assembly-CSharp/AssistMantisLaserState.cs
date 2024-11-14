using UnityEngine;

public class AssistMantisLaserState : EnemyState
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
		AssistMantis assistMantis = enemy as AssistMantis;
		if (assistMantis != null)
		{
			assistMantis.PlayAnimation(AnimationString.MANTIS_STAND_LASER, WrapMode.ClampForever);
			if (assistMantis.CanShot)
			{
				assistMantis.LookAtTarget();
			}
			else if (assistMantis.AnimationPlayed(AnimationString.MANTIS_STAND_LASER, 0.83f))
			{
				assistMantis.FlyLaserStop();
			}
			if (assistMantis.CanShot && assistMantis.AnimationPlayed(AnimationString.MANTIS_STAND_LASER, 0.47f))
			{
				assistMantis.FlyLaserStart();
				assistMantis.CanShot = false;
			}
			if (assistMantis.AnimationPlayed(AnimationString.MANTIS_STAND_LASER, 1f))
			{
				assistMantis.SetState(AssistMantis.ASSIST_IDLE_STATE);
				assistMantis.LaserFireStop();
				assistMantis.SetGroundIdleTimeNow();
				assistMantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
