using UnityEngine;

public class CoopMantisFlyIdleState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Transform transform = enemy.GetTransform();
		CoopMantis coopMantis = enemy as CoopMantis;
		if (coopMantis != null)
		{
			coopMantis.PlayAnimation(AnimationString.ENEMY_FLY_IDLE, WrapMode.Loop);
			coopMantis.CoopFlyIdle();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
