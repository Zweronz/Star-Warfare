using UnityEngine;

public class GravityForceState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		enemy.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		if (enemy.DoGravityForce())
		{
			enemy.SetState(Enemy.IDLE_STATE);
			enemy.StopGravityForceEffect();
		}
	}
}
