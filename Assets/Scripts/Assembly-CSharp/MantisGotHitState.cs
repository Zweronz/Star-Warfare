using UnityEngine;

public class MantisGotHitState : EnemyState
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
		bool flag = mantis.isFlying();
		string name = AnimationString.ENEMY_GOTHIT;
		if (flag)
		{
			name = AnimationString.ENEMY_FLY_GOTHIT;
		}
		mantis.PlayAnimation(name, WrapMode.ClampForever, 1f);
		if (!mantis.AnimationPlayed(name, 1f))
		{
			return;
		}
		if (mantis.NeedRage())
		{
			if (flag)
			{
				mantis.NeedLandToRage = true;
				mantis.SetState(Mantis.LANDING_STATE);
				mantis.EnableGravity(true);
			}
			else
			{
				mantis.SetState(Mantis.RAGE_STATE);
			}
		}
		else if (flag)
		{
			mantis.SetState(Mantis.FLY_IDLE_STATE);
			mantis.SetFlyIdleTimeNow();
		}
		else
		{
			mantis.SetState(Enemy.IDLE_STATE);
			mantis.SetGroundIdleTimeNow();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
