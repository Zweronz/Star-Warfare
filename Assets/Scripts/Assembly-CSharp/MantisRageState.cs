using UnityEngine;

public class MantisRageState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Mantis mantis = enemy as Mantis;
		if (mantis != null)
		{
			mantis.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
			mantis.PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_gongjiqian");
			if (mantis.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
			{
				mantis.SetState(Enemy.IDLE_STATE);
				mantis.SetGroundIdleTimeNow();
				mantis.SetLandingTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
