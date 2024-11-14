using UnityEngine;

public class LookAroundState : EnemyState
{
	public override void NextState(Enemy enemy, float deltaTime, Player player)
	{
		if (enemy.HP <= 0)
		{
			enemy.OnDead();
			enemy.SetState(Enemy.DEAD_STATE);
			return;
		}
		Zergling zergling = enemy as Zergling;
		enemy.PlayAnimation(AnimationString.ENEMY_IDLE, WrapMode.Loop);
		enemy.GetTransform().LookAt(player.GetTransform());
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		zergling.LookAtTarget();
		if (!zergling.LookAroundTimOut())
		{
			return;
		}
		if (zergling.ReadyForJump())
		{
			zergling.PlayAnimation(AnimationString.ENEMY_JUMP, WrapMode.ClampForever);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(zergling.EnemyID, 3, zergling.GetTransform().position, zergling.speed);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			zergling.SetState(Zergling.JUMP_STATE);
		}
		else
		{
			zergling.SetState(Enemy.CATCHING_STATE);
		}
	}
}
