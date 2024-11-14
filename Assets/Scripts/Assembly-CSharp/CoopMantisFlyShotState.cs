using UnityEngine;

public class CoopMantisFlyShotState : EnemyState
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
			coopMantis.PlayAnimation(AnimationString.MANTIS_SHOT, WrapMode.ClampForever);
			coopMantis.FlySound();
			if (coopMantis.CanShot && coopMantis.AnimationPlayed(AnimationString.MANTIS_SHOT, 0.52f))
			{
				coopMantis.CoopFlyShot();
				coopMantis.CanShot = false;
			}
			if (coopMantis.AnimationPlayed(AnimationString.MANTIS_SHOT, 1f))
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_IDLE_STATE);
				coopMantis.SetFlyIdleTimeNow();
				coopMantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
