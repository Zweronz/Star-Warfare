using UnityEngine;

public class MantisFlyDiveEndState : EnemyState
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
		if (mantis == null)
		{
			return;
		}
		mantis.PlayAnimation(AnimationString.MANTIS_RUSH_END, WrapMode.ClampForever);
		mantis.FlyUp();
		if (!mantis.AnimationPlayed(AnimationString.MANTIS_RUSH_END, 1f))
		{
			return;
		}
		if (mantis.isFlying())
		{
			if (mantis.GetFlyTimeDuration() > mantis.MaxFlyTime)
			{
				mantis.SetState(Mantis.LANDING_STATE);
				return;
			}
			mantis.SetState(Mantis.FLY_IDLE_STATE);
			mantis.SetFlyIdleTimeNow();
		}
		else if (mantis.GetLandingTimeDuration() > mantis.MaxGroundTime)
		{
			mantis.SetState(Mantis.FLY_IDLE_STATE);
			mantis.SetFlyTimeNow();
		}
		else
		{
			mantis.SetState(Mantis.LANDING_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
