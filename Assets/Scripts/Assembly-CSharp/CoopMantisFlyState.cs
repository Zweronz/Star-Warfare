using UnityEngine;

public class CoopMantisFlyState : EnemyState
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
			coopMantis.PlayAnimation(AnimationString.ENEMY_FLY, WrapMode.Loop);
			coopMantis.CoopFly();
			coopMantis.LookAtTargetPoint();
			if (coopMantis.CloseToTargetPoint() || coopMantis.GetCoopFlyDuration() > coopMantis.MaxCoopFlyTime)
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_IDLE_STATE);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
