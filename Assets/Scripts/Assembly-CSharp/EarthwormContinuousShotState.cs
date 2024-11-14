using UnityEngine;

public class EarthwormContinuousShotState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.EARTHWORM_CONTINUOUS_SHOT, WrapMode.ClampForever);
			earthworm.DoContinuousShot();
			if (earthworm.CanShot)
			{
				earthworm.LookAtTarget();
			}
			if (earthworm.CanShot && earthworm.AnimationPlayed(AnimationString.EARTHWORM_CONTINUOUS_SHOT, 0.27f))
			{
				earthworm.StartContinuousShot();
				earthworm.CanShot = false;
			}
			if (!earthworm.CanShot && earthworm.AnimationPlayed(AnimationString.EARTHWORM_CONTINUOUS_SHOT, 0.73f))
			{
				earthworm.StopContinuousShot();
			}
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_CONTINUOUS_SHOT, 1f))
			{
				earthworm.SetState(Enemy.IDLE_STATE);
				earthworm.SetIdleTimeNow();
				earthworm.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
