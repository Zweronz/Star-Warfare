using UnityEngine;

public class SatanMachineRageState : EnemyState
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
			string sATANMACHINE_START_RAGE = AnimationString.SATANMACHINE_START_RAGE;
			satanMachine.PlayAnimation(sATANMACHINE_START_RAGE, WrapMode.ClampForever, 1f);
			if (satanMachine.AnimationPlayed(sATANMACHINE_START_RAGE, 1f))
			{
				satanMachine.ReadyToThrowBall();
			}
		}
	}
}
