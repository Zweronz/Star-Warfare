using UnityEngine;

public class AssistMantisBoomerangState : EnemyState
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
			assistMantis.PlayAnimation(AnimationString.MANTIS_STAND_SHOT, WrapMode.ClampForever);
			assistMantis.LookAtTarget();
			if (assistMantis.CanShot && assistMantis.AnimationPlayed(AnimationString.MANTIS_STAND_SHOT, 0.52f))
			{
				assistMantis.FlyBoomerang();
				assistMantis.CanShot = false;
			}
			if (assistMantis.AnimationPlayed(AnimationString.MANTIS_STAND_SHOT, 1f))
			{
				assistMantis.SetState(AssistMantis.ASSIST_IDLE_STATE);
				assistMantis.SetGroundIdleTimeNow();
				assistMantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
