using UnityEngine;

public class MantisFlyBoomerangState : EnemyState
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
			mantis.PlayAnimation(AnimationString.MANTIS_BOOMERANG, WrapMode.ClampForever);
			mantis.FlySound();
			if (mantis.CanShot && mantis.AnimationPlayed(AnimationString.MANTIS_BOOMERANG, 0.52f))
			{
				mantis.FlyBoomerang();
				mantis.CanShot = false;
			}
			if (mantis.AnimationPlayed(AnimationString.MANTIS_BOOMERANG, 1f))
			{
				mantis.SetState(Mantis.FLY_IDLE_STATE);
				mantis.SetFlyIdleTimeNow();
				mantis.CanShot = true;
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
