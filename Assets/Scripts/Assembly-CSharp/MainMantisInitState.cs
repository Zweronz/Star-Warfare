using UnityEngine;

public class MainMantisInitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		CoopMantis coopMantis = enemy as CoopMantis;
		if (coopMantis == null)
		{
			return;
		}
		if (coopMantis.GetGroundIdleDuration() < coopMantis.InitIdleTime)
		{
			coopMantis.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		coopMantis.PlayAnimation(coopMantis.InitAniamtion, WrapMode.ClampForever);
		coopMantis.CriticalAttack();
		if (coopMantis.AnimationPlayed(coopMantis.InitAniamtion, 1f))
		{
			coopMantis.SetGroundIdleTimeNow();
			coopMantis.SetState(MainMantis.SEE_PLAYER_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
