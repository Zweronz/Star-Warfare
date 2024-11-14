using UnityEngine;

public class CoopMantisFlyDiveState : EnemyState
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
		if (coopMantis != null)
		{
			coopMantis.PlayAnimation(AnimationString.MANTIS_RUSH, WrapMode.Loop, 2f);
			coopMantis.Dive();
			coopMantis.CheckDiveHit();
			if (coopMantis.NearGround() || coopMantis.CloseToTargetPoint())
			{
				coopMantis.PlayAnimation(AnimationString.MANTIS_RUSH_END, WrapMode.ClampForever);
				coopMantis.SetState(CoopMantis.COOP_FLY_DIVE_END_STATE);
				coopMantis.EnableTrailEffect(false);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
