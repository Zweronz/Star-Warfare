using UnityEngine;

public class MantisCriticalAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Mantis mantis = enemy as Mantis;
		if (mantis != null)
		{
			mantis.PlayAnimation(AnimationString.MANTIS_CRITICAL_ATTACK, WrapMode.ClampForever);
			mantis.CheckHit();
			mantis.CriticalAttack();
			if (mantis.AnimationPlayed(AnimationString.MANTIS_CRITICAL_ATTACK, 1f))
			{
				mantis.SetState(Enemy.IDLE_STATE);
				mantis.SetGroundIdleTimeNow();
			}
		}
	}
}
