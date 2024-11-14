using UnityEngine;

public class SatanMachineThrowBallState : EnemyState
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
			string sATANMACHINE_THROW_BALL = AnimationString.SATANMACHINE_THROW_BALL;
			satanMachine.PlayAnimation(sATANMACHINE_THROW_BALL, WrapMode.ClampForever, 1f);
			if (!satanMachine.IsDoThrowBall && satanMachine.AnimationPlayed(sATANMACHINE_THROW_BALL, 0.05f))
			{
				satanMachine.IsDoThrowBall = true;
				satanMachine.ThrowBall();
			}
			else if (satanMachine.AnimationPlayed(sATANMACHINE_THROW_BALL, 1f))
			{
				satanMachine.ReadyToWhirlingAttack();
			}
		}
	}
}
