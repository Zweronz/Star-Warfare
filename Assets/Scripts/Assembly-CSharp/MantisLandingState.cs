using UnityEngine;

public class MantisLandingState : EnemyState
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
		mantis.PlayAnimation(AnimationString.ENEMY_LANDING, WrapMode.ClampForever);
		mantis.Land();
		if (mantis.AnimationPlayed(AnimationString.ENEMY_LANDING, 1f))
		{
			if (mantis.NeedLandToRage)
			{
				mantis.NeedLandToRage = false;
				mantis.SetState(Mantis.RAGE_STATE);
				mantis.OnRage();
			}
			else
			{
				mantis.SetState(Enemy.IDLE_STATE);
				mantis.SetGroundIdleTimeNow();
				mantis.SetLandingTimeNow();
			}
			mantis.StopSound("feixingtanglang_fly_idle");
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
