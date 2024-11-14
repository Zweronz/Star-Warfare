using UnityEngine;

public class DragonGotHitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Dragon dragon = enemy as Dragon;
		if (dragon == null)
		{
			return;
		}
		bool flag = dragon.isFlying();
		string name = AnimationString.ENEMY_GOTHIT;
		if (flag)
		{
			name = AnimationString.ENEMY_FLY_GOTHIT;
		}
		dragon.PlayAnimation(name, WrapMode.ClampForever, 1f);
		if (!dragon.AnimationPlayed(name, 1f))
		{
			return;
		}
		if (dragon.NeedRage())
		{
			if (flag)
			{
				dragon.NeedLandToRage = true;
				dragon.SetState(Dragon.LANDING_STATE);
				dragon.EnableGravity(true);
			}
			else
			{
				dragon.SetState(Dragon.RAGE_STATE);
			}
		}
		else if (flag)
		{
			dragon.SetState(Dragon.FLY_IDLE_STATE);
			dragon.SetFlyIdleTimeNow();
		}
		else
		{
			dragon.SetState(Enemy.IDLE_STATE);
			dragon.SetGroundIdleTimeNow();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
