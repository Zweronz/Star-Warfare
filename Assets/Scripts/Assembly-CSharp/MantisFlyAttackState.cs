using UnityEngine;

public class MantisFlyAttackState : EnemyState
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
			mantis.PlayAnimation(AnimationString.MANTIS_SHOT, WrapMode.ClampForever);
			mantis.CheckHit();
			mantis.FlyAttack();
			mantis.FlySound();
			if (mantis.AnimationPlayed(AnimationString.MANTIS_SHOT, 1f))
			{
				mantis.SetState(Mantis.FLY_IDLE_STATE);
				mantis.SetFlyIdleTimeNow();
			}
		}
	}
}
