using UnityEngine;

public class CoopMantisJoinStartFlyState : EnemyState
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
		coopMantis.EnableGravity(false);
		if (coopMantis != null)
		{
			coopMantis.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
			coopMantis.StartFlyHigh();
			coopMantis.LookAtTargetPoint();
			if (coopMantis.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_START_DIVE_STATE);
				coopMantis.EnableHitCollider();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
