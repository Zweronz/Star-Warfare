using UnityEngine;

public class SatanMachineRageEndState : EnemyState
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
			string sATANMACHINE_END_RAGE = AnimationString.SATANMACHINE_END_RAGE;
			satanMachine.PlayAnimation(sATANMACHINE_END_RAGE, WrapMode.ClampForever, 1f);
			if (satanMachine.AnimationPlayed(sATANMACHINE_END_RAGE, 1f))
			{
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
		}
	}
}
