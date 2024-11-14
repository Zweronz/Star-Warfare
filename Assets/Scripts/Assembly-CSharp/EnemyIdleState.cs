using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		enemy.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		float sqrMagnitude = (enemy.GetTransform().position - player.GetTransform().position).sqrMagnitude;
		bool flag = true;
		if (GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InPlayingState())
		{
			flag = false;
		}
		if (flag)
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item.InPlayingState())
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag && sqrMagnitude < enemy.DetectionRange * enemy.DetectionRange)
		{
			enemy.OnIdle();
		}
	}
}
