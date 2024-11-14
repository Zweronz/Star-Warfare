using UnityEngine;

public class EarthwormRushingState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.EARTHWORM_RUSH, WrapMode.Loop, 2f);
			earthworm.Rush();
			if (earthworm.CloseToRushTarget())
			{
				earthworm.SetState(Earthworm.RUSH_END_STATE);
				earthworm.EnableTrailEffect(false);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
