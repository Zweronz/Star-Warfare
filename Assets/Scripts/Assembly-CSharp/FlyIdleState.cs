using UnityEngine;

public class FlyIdleState : EnemyState
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
		dragon.PlayAnimation(AnimationString.ENEMY_FLY_IDLE, WrapMode.Loop);
		dragon.FlyUp();
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && !dragon.isAllPlayerDead())
		{
			byte b = 0;
			int targetID = dragon.GetTargetPlayer().GetUserID();
			if (dragon.GetFlyTimeDuration() < dragon.MaxFlyTime)
			{
				if (dragon.GetFlyIdleDuration() > dragon.MaxFlyIdleTime)
				{
					float averagehorizontalDistance = dragon.GetAveragehorizontalDistance();
					if (averagehorizontalDistance < dragon.AttackRange)
					{
						b = dragon.ChangeStateInFlyNear();
						targetID = dragon.GetNearestPlayer();
					}
					else if (averagehorizontalDistance < dragon.StartRushDistance)
					{
						b = dragon.ChangeStateInFlyFar();
						targetID = ((b != 47) ? dragon.GetRandomPlayer() : dragon.GetNearestPlayer());
					}
					else
					{
						dragon.SetState(Dragon.FLY_DIVE_STATE);
						b = 49;
						targetID = dragon.GetFarthestPlayer();
						dragon.ChangeTargetPlayer(targetID);
						dragon.OnDive();
					}
				}
			}
			else
			{
				dragon.SetState(Dragon.LANDING_STATE);
				dragon.EnableGravity(true);
				b = 52;
				targetID = dragon.GetNearestPlayer();
			}
			if (b != 0)
			{
				dragon.ChangeTargetPlayer(targetID);
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(dragon.EnemyID, b, dragon.GetTransform().position, targetID);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
		}
		dragon.LookAtTarget();
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
