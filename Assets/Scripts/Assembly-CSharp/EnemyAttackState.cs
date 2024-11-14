using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		if (enemy.CouldMakeNextAttack())
		{
			enemy.OnAttack();
		}
		else if (enemy.AttackAnimationEnds())
		{
			enemy.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		}
		enemy.CheckHit();
		bool flag = false;
		if (enemy.GetTargetPlayer() != null)
		{
			if (!enemy.GetTargetPlayer().InPlayingState())
			{
				flag = true;
			}
			Vector3 position = enemy.GetTargetPlayer().GetTransform().position;
			position.y = enemy.GetTransform().position.y;
			enemy.GetTransform().LookAt(position);
		}
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		bool flag2 = true;
		if (GameApp.GetInstance().GetGameWorld().GetPlayer()
			.InPlayingState())
		{
			flag2 = false;
		}
		if (flag2)
		{
			List<RemotePlayer> remotePlayers = GameApp.GetInstance().GetGameWorld().GetRemotePlayers();
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item.InPlayingState())
				{
					flag2 = false;
					break;
				}
			}
		}
		if ((enemy.SqrDistanceFromPlayer >= enemy.AttackRange * enemy.AttackRange && (enemy.AnimationPlayed(AnimationString.ENEMY_ATTACK, 1f) || enemy.AnimationPlayed(AnimationString.ENEMY_IDLE, 0.1f))) || (flag && !flag2))
		{
			enemy.SetState(Enemy.CATCHING_STATE);
		}
	}
}
