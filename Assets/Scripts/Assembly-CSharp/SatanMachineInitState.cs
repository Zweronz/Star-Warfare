using UnityEngine;

public class SatanMachineInitState : EnemyState
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
			if (satanMachine.GetIdleTimeDuration() < 2f)
			{
				satanMachine.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
				return;
			}
			satanMachine.SetIdleTimeNow();
			satanMachine.SetState(Enemy.IDLE_STATE);
			satanMachine.CanShot = true;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
