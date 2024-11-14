using UnityEngine;

public class EarthwormStartRushState : EnemyState
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
			earthworm.PlayAnimation(AnimationString.EARTHWORM_START_RUSH, WrapMode.ClampForever);
			earthworm.LookAtTarget();
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_START_RUSH, 1f))
			{
				earthworm.SetState(Earthworm.RUSH_STATE);
				earthworm.OnRush();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
