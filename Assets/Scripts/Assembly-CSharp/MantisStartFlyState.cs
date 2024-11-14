using UnityEngine;

public class MantisStartFlyState : EnemyState
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
		mantis.EnableGravity(false);
		if (mantis == null)
		{
			return;
		}
		mantis.PlayAnimation(AnimationString.ENEMY_START_FLY, WrapMode.ClampForever);
		mantis.StartFly();
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && mantis.AnimationPlayed(AnimationString.ENEMY_START_FLY, 1f))
		{
			if (mantis.GetLandingTimeDuration() > mantis.MaxGroundTime)
			{
				mantis.SetState(Mantis.FLY_IDLE_STATE);
				mantis.SetFlyIdleTimeNow();
				mantis.SetFlyTimeNow();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(mantis.EnemyID, 14, mantis.GetTransform().position, Vector3.zero);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				mantis.SetState(Mantis.FLY_DIVESTART_STATE);
				int farthestPlayer = mantis.GetFarthestPlayer();
				mantis.ChangeTargetPlayer(farthestPlayer);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request2 = new EnemyStateRequest(mantis.EnemyID, 21, mantis.GetTransform().position, farthestPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
				}
			}
		}
		mantis.LookAtTarget();
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
