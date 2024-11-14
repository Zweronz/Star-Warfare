using UnityEngine;

public class FlyState : EnemyState
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
		if (dragon == null)
		{
			return;
		}
		dragon.PlayAnimation(AnimationString.ENEMY_FLY, WrapMode.Loop);
		dragon.Fly();
		if (dragon.GetCatchingration() > dragon.MaxCatchingTime)
		{
			dragon.SetState(Dragon.FLY_IDLE_STATE);
		}
		else if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && dragon.GetHorizontalSqrDistanceFromTarget() < dragon.AttackRange * dragon.AttackRange)
		{
			byte state = dragon.ChangeStateInFlyNear();
			int nearestPlayer = dragon.GetNearestPlayer();
			dragon.ChangeTargetPlayer(nearestPlayer);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(enemy.EnemyID, state, enemy.GetTransform().position, nearestPlayer);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
