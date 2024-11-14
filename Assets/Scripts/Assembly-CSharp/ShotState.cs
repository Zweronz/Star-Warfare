using UnityEngine;

public class ShotState : EnemyState
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
		dragon.PlayAnimation(AnimationString.ENEMY_ATTACK, WrapMode.ClampForever);
		if (dragon.CanShot && dragon.AnimationPlayed(AnimationString.ENEMY_ATTACK, 0.52f))
		{
			dragon.Shot();
			dragon.CanShot = false;
		}
		if (!dragon.AnimationPlayed(AnimationString.ENEMY_ATTACK, 1f))
		{
			return;
		}
		dragon.IncreaseAttackCount();
		if (dragon.GetAttackCount() < dragon.MaxShotTimes)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				dragon.SetState(Dragon.SHOT_STATE);
				int randomPlayer = dragon.GetRandomPlayer();
				dragon.ChangeTargetPlayer(randomPlayer);
				dragon.LookAtTarget();
				dragon.StartShot();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(dragon.EnemyID, 51, dragon.GetTransform().position, randomPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				dragon.SetState(Enemy.IDLE_STATE);
			}
		}
		else
		{
			dragon.SetState(Enemy.IDLE_STATE);
			dragon.SetGroundIdleTimeNow();
			dragon.ResetAttackCount();
			dragon.CanShot = true;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
