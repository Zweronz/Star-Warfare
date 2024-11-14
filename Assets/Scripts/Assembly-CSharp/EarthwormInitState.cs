using UnityEngine;

public class EarthwormInitState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Earthworm earthworm = enemy as Earthworm;
		if (earthworm == null)
		{
			return;
		}
		if (earthworm.GetIdleTimeDuration() < 2f)
		{
			earthworm.PlayAnimation(AnimationString.Idle, WrapMode.Loop);
			return;
		}
		earthworm.PlayAnimation(AnimationString.ENEMY_RAGE, WrapMode.ClampForever);
		earthworm.Rage();
		if (earthworm.AnimationPlayed(AnimationString.ENEMY_RAGE, 1f))
		{
			earthworm.SetIdleTimeNow();
			earthworm.SetState(Enemy.IDLE_STATE);
			earthworm.CanShot = true;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
