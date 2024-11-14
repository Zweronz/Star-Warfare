using UnityEngine;

public class AssistMantisInitState : EnemyState
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
		if (assistMantis == null)
		{
			return;
		}
		if (assistMantis.GetGroundIdleDuration() < assistMantis.InitIdleTime)
		{
			assistMantis.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		assistMantis.PlayAnimation(assistMantis.InitAniamtion, WrapMode.ClampForever);
		assistMantis.CriticalAttack();
		if (assistMantis.AnimationPlayed(assistMantis.InitAniamtion, 1f))
		{
			assistMantis.SetGroundIdleTimeNow();
			assistMantis.SetState(AssistMantis.SEE_PLAYER_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
