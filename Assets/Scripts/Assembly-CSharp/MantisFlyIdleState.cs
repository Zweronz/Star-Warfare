using UnityEngine;

public class MantisFlyIdleState : EnemyState
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
		Mantis mantis = enemy as Mantis;
		if (mantis != null)
		{
			mantis.PlayAnimation(AnimationString.ENEMY_FLY_IDLE, WrapMode.Loop);
			mantis.FlyIdle();
			mantis.LookAtTarget();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
