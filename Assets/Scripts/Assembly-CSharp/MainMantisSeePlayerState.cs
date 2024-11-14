using UnityEngine;

public class MainMantisSeePlayerState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		MainMantis mainMantis = enemy as MainMantis;
		if (mainMantis != null)
		{
			if (mainMantis.GetGroundIdleDuration() < mainMantis.SeePlayerIdleTime)
			{
				mainMantis.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
				return;
			}
			mainMantis.SetTargetPoint(CoopMantis.TargetPointType.RIGHT_MID);
			mainMantis.SetFlyTimeNow();
			mainMantis.SetState(MainMantis.INIT_START_FLY_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
