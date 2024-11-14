using UnityEngine;

public class FlyDiveState : EnemyState
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
		Dragon dragon = enemy as Dragon;
		if (dragon != null)
		{
			dragon.PlayAnimation(AnimationString.ENEMY_FLY_DIVE, WrapMode.Loop);
			dragon.Dive();
			dragon.CheckRushHit();
			if (dragon.NearGround())
			{
				dragon.SetState(Dragon.FLY_RUSH_STATE);
				dragon.SetDir((dragon.RushTargetPos - transform.position).normalized);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
