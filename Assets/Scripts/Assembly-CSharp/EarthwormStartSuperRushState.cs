using UnityEngine;

public class EarthwormStartSuperRushState : EnemyState
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
		if (earthworm == null)
		{
			return;
		}
		earthworm.PlayAnimation(AnimationString.EARTHWORM_START_RUSH, WrapMode.ClampForever);
		earthworm.LookAtTarget();
		if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_START_RUSH, 1f))
		{
			earthworm.SetState(Earthworm.SUPER_RUSH_STATE);
			earthworm.SetStartRushTimeNow();
			if (earthworm.GetAttackCount() > 0)
			{
				earthworm.CreateEnterBlackhole();
			}
			if (earthworm.GetAttackCount() == earthworm.MaxRushTimes - 1)
			{
				earthworm.OnRush();
			}
			else
			{
				earthworm.OnSuperRush();
			}
			earthworm.IncreaseAttackCount();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
