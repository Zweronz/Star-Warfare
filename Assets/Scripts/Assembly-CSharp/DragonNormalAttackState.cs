using UnityEngine;

public class DragonNormalAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Dragon dragon = enemy as Dragon;
		if (dragon != null)
		{
			dragon.PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
			dragon.NormalAttack();
			dragon.CheckHit();
			if (dragon.AnimationPlayed(AnimationString.ENEMY_ATTACK, 1f))
			{
				dragon.SetState(Enemy.IDLE_STATE);
				dragon.SetGroundIdleTimeNow();
			}
		}
	}
}
