using UnityEngine;

public class SatanMachineGiftBombState : EnemyState
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
			string sATANMACHINE_GIFT_BOMB = AnimationString.SATANMACHINE_GIFT_BOMB;
			satanMachine.PlayAnimation(sATANMACHINE_GIFT_BOMB, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoGiftBomb && satanMachine.AnimationPlayed(sATANMACHINE_GIFT_BOMB, 0.05f))
			{
				satanMachine.IsDoGiftBomb = true;
				satanMachine.ShotGiftBomb();
			}
			else if (satanMachine.AnimationPlayed(sATANMACHINE_GIFT_BOMB, 1f))
			{
				satanMachine.ResetGiftBomb();
				satanMachine.SetIdleTimeNow();
				satanMachine.SetState(Enemy.IDLE_STATE);
			}
		}
	}
}
