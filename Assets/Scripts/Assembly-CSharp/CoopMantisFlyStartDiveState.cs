using UnityEngine;

public class CoopMantisFlyStartDiveState : EnemyState
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
			coopMantis.PlayAnimation(AnimationString.MANTIS_RUSH_START, WrapMode.ClampForever);
			coopMantis.StopSound("feixingtanglang_fly_idle");
			coopMantis.PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fuchong2");
			coopMantis.LookAtTargetPoint();
			if (coopMantis.AnimationPlayed(AnimationString.MANTIS_RUSH_START, 1f))
			{
				coopMantis.SetState(CoopMantis.COOP_FLY_DIVE_STATE);
				coopMantis.OnCoopDive();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
