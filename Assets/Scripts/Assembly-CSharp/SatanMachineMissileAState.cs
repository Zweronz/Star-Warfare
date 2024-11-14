using UnityEngine;

public class SatanMachineMissileAState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		SatanMachine satanMachine = enemy as SatanMachine;
		if (satanMachine != null)
		{
			string sATANMACHINE_LAUNCH_MISSILE_ = AnimationString.SATANMACHINE_LAUNCH_MISSILE_1;
			satanMachine.PlayAnimation(sATANMACHINE_LAUNCH_MISSILE_, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoLaunchMissile[0] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.275f))
			{
				satanMachine.IsDoLaunchMissile[0] = true;
				satanMachine.LaunchMissile();
			}
			else if (!satanMachine.IsDoLaunchMissile[1] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.325f))
			{
				satanMachine.IsDoLaunchMissile[1] = true;
				satanMachine.LaunchMissile();
			}
			else if (!satanMachine.IsDoLaunchMissile[2] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.4f))
			{
				satanMachine.IsDoLaunchMissile[2] = true;
				satanMachine.LaunchMissile();
			}
			else if (!satanMachine.IsDoLaunchMissile[3] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.475f))
			{
				satanMachine.IsDoLaunchMissile[3] = true;
				satanMachine.LaunchMissile();
			}
			else if (!satanMachine.IsDoLaunchMissile[4] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.54f))
			{
				satanMachine.IsDoLaunchMissile[4] = true;
				satanMachine.LaunchMissile();
			}
			else if (!satanMachine.IsDoLaunchMissile[5] && satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 0.625f))
			{
				satanMachine.IsDoLaunchMissile[5] = true;
				satanMachine.LaunchMissile();
			}
			else if (satanMachine.AnimationPlayed(sATANMACHINE_LAUNCH_MISSILE_, 1f))
			{
				satanMachine.ResetMissile();
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
