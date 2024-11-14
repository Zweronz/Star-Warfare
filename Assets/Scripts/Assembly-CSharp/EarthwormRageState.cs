using UnityEngine;

public class EarthwormRageState : EnemyState
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
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
			earthworm.Rage();
			if (earthworm.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
			{
				earthworm.SetState(Enemy.IDLE_STATE);
				earthworm.SetIdleTimeNow();
				earthworm.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
