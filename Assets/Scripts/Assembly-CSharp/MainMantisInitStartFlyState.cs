using UnityEngine;

public class MainMantisInitStartFlyState : EnemyState
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
		MainMantis mainMantis = enemy as MainMantis;
		mainMantis.EnableGravity(false);
		if (mainMantis != null)
		{
			mainMantis.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
			mainMantis.StartFlyHigh();
			if (mainMantis.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
			{
				mainMantis.SetState(MainMantis.INIT_FLY_START_DIVE_STATE);
			}
			mainMantis.LookAtTargetPoint();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
