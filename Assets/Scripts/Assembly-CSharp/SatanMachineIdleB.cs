using UnityEngine;

public class SatanMachineIdleB : EnemyState
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
			string sATANMACHINE_IDLE_B = AnimationString.SATANMACHINE_IDLE_B;
			satanMachine.PlayAnimation(sATANMACHINE_IDLE_B, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoPlayReloadSound && satanMachine.AnimationPlayed(sATANMACHINE_IDLE_B, 0.44f))
			{
				satanMachine.IsDoPlayReloadSound = true;
				satanMachine.PlaySound("Audio/enemy/SatanMachine/XMAS_reload");
			}
			else if (satanMachine.AnimationPlayed(sATANMACHINE_IDLE_B, 1f))
			{
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
			if (!AudioManager.GetInstance().IsPlaying("xunlu"))
			{
				satanMachine.PlaySound("Audio/enemy/SatanMachine/xunlu");
			}
		}
	}
}
