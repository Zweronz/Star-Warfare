using UnityEngine;

public class EarthwormDrilloutState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		Transform transform = enemy.GetTransform();
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.EARTHWORM_DRILLOUT, WrapMode.ClampForever, 1f);
			earthworm.DrillOut();
			earthworm.CheckDrillout();
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_DRILLOUT, 1f))
			{
				earthworm.SetState(Enemy.IDLE_STATE);
				earthworm.SetIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
