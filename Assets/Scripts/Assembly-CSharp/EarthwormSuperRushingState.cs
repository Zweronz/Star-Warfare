using UnityEngine;

public class EarthwormSuperRushingState : EnemyState
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
		earthworm.PlayAnimation(AnimationString.EARTHWORM_SUPER_RUSH, WrapMode.Loop, 2f);
		earthworm.SuperRush();
		if (!earthworm.CloseToRushTarget() && !earthworm.SuperRushTimeOut())
		{
			return;
		}
		if (earthworm.GetAttackCount() < earthworm.MaxRushTimes)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				earthworm.SetState(Earthworm.START_SUPER_RUSH_STATE);
				int randomPlayer = earthworm.GetRandomPlayer();
				earthworm.ChangeTargetPlayer(randomPlayer);
				earthworm.StartSuperRush();
				earthworm.LookAtTarget();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(earthworm.EnemyID, 86, earthworm.GetTransform().position, randomPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				earthworm.SetState(Enemy.IDLE_STATE);
			}
		}
		else
		{
			earthworm.SetState(Earthworm.RUSH_END_STATE);
			earthworm.ResetAttackCount();
		}
		earthworm.EnableTrailEffect(false);
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
