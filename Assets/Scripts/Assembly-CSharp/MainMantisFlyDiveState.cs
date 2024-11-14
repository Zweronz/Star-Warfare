using UnityEngine;

public class MainMantisFlyDiveState : EnemyState
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
		if (mainMantis != null)
		{
			mainMantis.PlayAnimation(AnimationString.MANTIS_RUSH, WrapMode.Loop, 2f);
			mainMantis.Dive();
			mainMantis.CheckDiveHit();
			if (mainMantis.NearGround() || mainMantis.CloseToDiveTarget())
			{
				mainMantis.PlayAnimation(AnimationString.MANTIS_RUSH_END, WrapMode.ClampForever);
				mainMantis.SetState(Mantis.FLY_DIVEEND_STATE);
				mainMantis.EnableTrailEffect(false);
				mainMantis.EnableWallDefent();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
