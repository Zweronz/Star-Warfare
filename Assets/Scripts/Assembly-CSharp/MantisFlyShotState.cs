using UnityEngine;

public class MantisFlyShotState : EnemyState
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
		mantis.PlayAnimation(AnimationString.MANTIS_SHOT, WrapMode.ClampForever);
		mantis.LookAtTarget();
		mantis.FlySound();
		if (mantis.CanShot && mantis.AnimationPlayed(AnimationString.MANTIS_SHOT, 0.52f))
		{
			mantis.FlyShot();
			mantis.CanShot = false;
		}
		if (!mantis.AnimationPlayed(AnimationString.MANTIS_SHOT, 1f))
		{
			return;
		}
		mantis.IncreaseAttackCount();
		if (mantis.GetAttackCount() < mantis.MaxShotTimes)
		{
			if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
			{
				mantis.SetState(Mantis.FLY_SHOT_STATE);
				int randomPlayer = mantis.GetRandomPlayer();
				mantis.ChangeTargetPlayer(randomPlayer);
				mantis.LookAtTarget();
				mantis.StartFlyShot();
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					EnemyStateRequest request = new EnemyStateRequest(mantis.EnemyID, 16, mantis.GetTransform().position, randomPlayer);
					GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				}
			}
			else
			{
				mantis.SetState(Mantis.FLY_IDLE_STATE);
			}
		}
		else
		{
			mantis.SetState(Mantis.FLY_IDLE_STATE);
			mantis.SetFlyIdleTimeNow();
			mantis.ResetAttackCount();
			mantis.CanShot = true;
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
