using UnityEngine;

public class CatchingState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			enemy.FindPath();
		}
		enemy.DoMove(deltaTime);
		enemy.PlayAnimation(enemy.RunAnimationName, WrapMode.Loop);
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		EnemyState enemyState = enemy.EnterSpecialState(deltaTime);
		if (enemyState != null)
		{
			enemy.SetState(enemyState);
		}
		else if (enemy.CouldEnterAttackState())
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(enemy.EnemyID, 1, enemy.GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			enemy.SetState(Enemy.ATTACK_STATE);
		}
	}
}
