using UnityEngine;

public class SatanMachineLaserState : EnemyState
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
			string sATANMACHINE_SHOT_LASER = AnimationString.SATANMACHINE_SHOT_LASER;
			satanMachine.PlayAnimation(sATANMACHINE_SHOT_LASER, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoShotLaser && satanMachine.AnimationPlayed(sATANMACHINE_SHOT_LASER, 0.42f))
			{
				satanMachine.IsDoShotLaser = true;
				satanMachine.ShotLaser();
			}
			else if (satanMachine.AnimationPlayed(sATANMACHINE_SHOT_LASER, 1f))
			{
				satanMachine.ResetLaser();
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
		}
	}
}
