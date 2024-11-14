using UnityEngine;

public class MantisFlyStartDiveState : EnemyState
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
			mantis.PlayAnimation(AnimationString.MANTIS_RUSH_START, WrapMode.ClampForever);
			mantis.LookAtTarget();
			mantis.StopSound("feixingtanglang_fly_idle");
			mantis.PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fuchong2");
			if (mantis.AnimationPlayed(AnimationString.MANTIS_RUSH_START, 1f))
			{
				mantis.SetState(Mantis.FLY_DIVE_STATE);
				mantis.OnDive();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
