using UnityEngine;

public class EarthwormDrillIdleState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		Transform transform = enemy.GetTransform();
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.DrillIdle();
			if (earthworm.GetDrillIdleTimeDuration() > earthworm.MaxDrillIdleTime)
			{
				earthworm.SetState(Earthworm.DRILLOUT_STATE);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
