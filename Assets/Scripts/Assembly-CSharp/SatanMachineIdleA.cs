using UnityEngine;

public class SatanMachineIdleA : EnemyState
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
			string sATANMACHINE_IDLE_A = AnimationString.SATANMACHINE_IDLE_A;
			satanMachine.PlayAnimation(sATANMACHINE_IDLE_A, WrapMode.ClampForever, 1f);
			if (satanMachine.AnimationPlayed(sATANMACHINE_IDLE_A, 1f))
			{
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
		}
		if (!AudioManager.GetInstance().IsPlaying("xunlu"))
		{
			satanMachine.PlaySound("Audio/enemy/SatanMachine/xunlu");
		}
	}
}
