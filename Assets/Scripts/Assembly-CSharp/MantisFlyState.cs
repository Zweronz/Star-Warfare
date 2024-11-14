using UnityEngine;

public class MantisFlyState : EnemyState
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
		if (mantis == null)
		{
			return;
		}
		mantis.PlayAnimation(AnimationString.ENEMY_FLY, WrapMode.Loop);
		mantis.Fly();
		if (mantis.GetCatchingration() > mantis.MaxCatchingTime)
		{
			mantis.SetState(Mantis.FLY_IDLE_STATE);
		}
		else if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && mantis.GetHorizontalSqrDistanceFromTarget() < mantis.FlyAttackRange * mantis.FlyAttackRange)
		{
			byte state = mantis.ChangeStateInFlyNear();
			int nearestPlayer = mantis.GetNearestPlayer();
			mantis.ChangeTargetPlayer(nearestPlayer);
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
