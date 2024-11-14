using UnityEngine;

public class GotHitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		enemy.PlayAnimation(AnimationString.ENEMY_GOTHIT, WrapMode.ClampForever);
		if (enemy.AnimationPlayed(AnimationString.ENEMY_GOTHIT, 1f))
		{
			enemy.SetState(Enemy.IDLE_STATE);
		}
	}
}
