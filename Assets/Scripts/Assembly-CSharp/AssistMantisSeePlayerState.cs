using UnityEngine;

public class AssistMantisSeePlayerState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		AssistMantis assistMantis = enemy as AssistMantis;
		if (assistMantis != null)
		{
			if (assistMantis.GetGroundIdleDuration() < assistMantis.SeePlayerIdleTime)
			{
				assistMantis.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
				return;
			}
			assistMantis.SetTargetPoint(CoopMantis.TargetPointType.LEFT_MID);
			assistMantis.SetFlyTimeNow();
			assistMantis.SetState(AssistMantis.INIT_START_FLY_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
