using UnityEngine;

public class CoopMantisStartFlyState : EnemyState
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
			coopMantis.StartFly();
			coopMantis.LookAtTargetPoint();
			if (coopMantis.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				coopMantis.GoToTargetPoint();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
