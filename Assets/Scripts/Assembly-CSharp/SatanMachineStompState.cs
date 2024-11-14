using UnityEngine;

public class SatanMachineStompState : EnemyState
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
			satanMachine.PlayAnimation(AnimationString.SATANMACHINE_STOMP, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoStomp && satanMachine.AnimationPlayed(AnimationString.SATANMACHINE_STOMP, 0.25f))
			{
				satanMachine.IsDoStomp = true;
				satanMachine.CheckStomp();
			}
			else if (satanMachine.AnimationPlayed(AnimationString.SATANMACHINE_STOMP, 1f))
			{
				satanMachine.ResetStomp();
				satanMachine.ReadyToShotLaser();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
