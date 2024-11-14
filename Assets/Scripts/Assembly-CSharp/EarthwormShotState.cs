using UnityEngine;

public class EarthwormShotState : EnemyState
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
			earthworm.PlayAnimation(AnimationString.EARTHWORM_SHOT, WrapMode.ClampForever);
			if (earthworm.CanShot)
			{
				earthworm.LookAtTarget();
			}
			if (earthworm.CanShot && earthworm.AnimationPlayed(AnimationString.EARTHWORM_SHOT, 0.48f))
			{
				earthworm.Shot();
				earthworm.CanShot = false;
			}
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_SHOT, 1f))
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
