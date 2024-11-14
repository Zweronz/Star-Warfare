using UnityEngine;

public class SpiderRushingState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Spider spider = enemy as Spider;
		if (spider == null)
		{
			return;
		}
		spider.PlayAnimation(AnimationString.SPIDER_RUSH, WrapMode.Loop, 2f);
		spider.Rush();
		if (!spider.CloseToRushTarget())
		{
			return;
		}
		spider.IncreaseAttackCount();
		if (spider.GetAttackCount() < spider.MaxRushTimes)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				spider.SetState(Spider.START_RUSH_STATE);
				int farthestPlayer = spider.GetFarthestPlayer();
				spider.ChangeTargetPlayer(farthestPlayer);
				spider.StartRush();
				spider.LookAtTarget();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(spider.EnemyID, 34, spider.GetTransform().position, farthestPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				spider.SetState(Enemy.IDLE_STATE);
			}
		}
		else
		{
			spider.SetState(Enemy.IDLE_STATE);
			spider.SetIdleTimeNow();
			spider.ResetAttackCount();
		}
		spider.EnableTrailEffect(false);
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
