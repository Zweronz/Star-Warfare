using UnityEngine;

public class GraveBornState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
		}
		enemy.PlayAnimation(AnimationString.ENEMY_RUN, WrapMode.Loop);
		if (enemy.MoveFromGrave(deltaTime))
		{
			enemy.SetInGrave(false);
			enemy.SetState(Enemy.IDLE_STATE);
		}
	}
}
