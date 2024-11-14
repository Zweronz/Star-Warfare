using UnityEngine;

public class MantisFlyDiveState : EnemyState
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
		Mantis mantis = enemy as Mantis;
		if (mantis != null)
		{
			mantis.PlayAnimation(AnimationString.MANTIS_RUSH, WrapMode.Loop, 2f);
			mantis.Dive();
			mantis.CheckDiveHit();
			if (mantis.NearGround() || mantis.CloseToDiveTarget())
			{
				mantis.PlayAnimation(AnimationString.MANTIS_RUSH_END, WrapMode.ClampForever);
				mantis.SetState(Mantis.FLY_DIVEEND_STATE);
				mantis.EnableTrailEffect(false);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
