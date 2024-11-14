using UnityEngine;

public class FlyShotState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Dragon dragon = enemy as Dragon;
		if (dragon == null)
		{
			return;
		}
		dragon.PlayAnimation(AnimationString.ENEMY_FLY_ATTACK, WrapMode.ClampForever);
		dragon.LookAtTarget();
		if (dragon.CanShot && dragon.AnimationPlayed(AnimationString.ENEMY_FLY_ATTACK, 0.6f))
		{
			dragon.FlyShot();
			dragon.CanShot = false;
		}
		if (!dragon.AnimationPlayed(AnimationString.ENEMY_FLY_ATTACK, 1f))
		{
			return;
		}
		dragon.IncreaseAttackCount();
		if (dragon.GetAttackCount() < dragon.MaxFlyShotTimes)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				dragon.SetState(Dragon.FLY_SHOT_STATE);
				int randomPlayer = dragon.GetRandomPlayer();
				dragon.ChangeTargetPlayer(randomPlayer);
				dragon.LookAtTarget();
				dragon.StartFlyShot();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(dragon.EnemyID, 48, dragon.GetTransform().position, randomPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				dragon.SetState(Dragon.FLY_IDLE_STATE);
			}
		}
		else
		{
			dragon.SetState(Dragon.FLY_IDLE_STATE);
			dragon.SetFlyIdleTimeNow();
			dragon.ResetAttackCount();
			dragon.CanShot = true;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
