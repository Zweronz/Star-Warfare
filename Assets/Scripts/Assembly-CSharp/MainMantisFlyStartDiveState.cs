using UnityEngine;

public class MainMantisFlyStartDiveState : EnemyState
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
			mainMantis.PlayAnimation(AnimationString.MANTIS_RUSH_START, WrapMode.ClampForever);
			mainMantis.StopSound("feixingtanglang_fly_idle");
			mainMantis.PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_fuchong2");
			mainMantis.LookAtTargetPoint();
			if (mainMantis.AnimationPlayed(AnimationString.MANTIS_RUSH_START, 1f))
			{
				mainMantis.SetState(MainMantis.INIT_FLY_DIVE_STATE);
				mainMantis.OnCoopDive();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
