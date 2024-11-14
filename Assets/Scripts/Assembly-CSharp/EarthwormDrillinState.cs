using UnityEngine;

public class EarthwormDrillinState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		Transform transform = enemy.GetTransform();
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm != null)
		{
			earthworm.PlayAnimation(AnimationString.EARTHWORM_DRILLIN, WrapMode.ClampForever, 1.2f);
			earthworm.Drillin();
			if (earthworm.AnimationPlayed(AnimationString.EARTHWORM_DRILLIN, 1f))
			{
				earthworm.SetState(Earthworm.DRILL_IDLE_STATE);
				earthworm.SetDrillIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
