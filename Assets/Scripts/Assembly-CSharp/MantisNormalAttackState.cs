using UnityEngine;

public class MantisNormalAttackState : EnemyState
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
			mantis.PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
			mantis.CheckHit();
			mantis.NormalAttack();
			if (mantis.AnimationPlayed(AnimationString.ENEMY_ATTACK, 1f))
			{
				mantis.SetState(Enemy.IDLE_STATE);
				mantis.SetGroundIdleTimeNow();
			}
		}
	}
}
