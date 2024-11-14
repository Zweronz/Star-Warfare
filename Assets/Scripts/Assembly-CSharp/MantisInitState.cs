using UnityEngine;

public class MantisInitState : EnemyState
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
		if (mantis == null)
		{
			return;
		}
		if (mantis.GetGroundIdleDuration() < 2f)
		{
			mantis.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		mantis.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
		mantis.PlaySoundSingle("Audio/enemy/Mantis/feixingtanglang_gongjiqian");
		if (mantis.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
		{
			int farthestPlayer = mantis.GetFarthestPlayer();
			mantis.ChangeTargetPlayer(farthestPlayer);
			mantis.LookAtTarget();
			mantis.SetFlyTimeNow();
			mantis.SetState(Mantis.START_FLY_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
