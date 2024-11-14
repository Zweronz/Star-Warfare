using UnityEngine;

public class EarthwormGotHitState : EnemyState
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
		earthworm.PlayAnimation(AnimationString.ENEMY_GOTHIT, WrapMode.ClampForever, 1f);
		if (earthworm.AnimationPlayed(AnimationString.ENEMY_GOTHIT, 1f))
		{
			if (earthworm.NeedRage())
			{
				earthworm.SetState(Earthworm.RAGE_STATE);
				earthworm.StartRage();
			}
			else
			{
				earthworm.SetState(Enemy.IDLE_STATE);
				earthworm.SetIdleTimeNow();
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
