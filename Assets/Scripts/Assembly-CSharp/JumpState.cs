using UnityEngine;

public class JumpState : EnemyState
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
		Zergling zergling = enemy as Zergling;
		if (zergling != null)
		{
			if (zergling.JumpInOne(deltaTime) && (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()))
			{
				zergling.SetState(Enemy.IDLE_STATE);
			}
			zergling.CheckPuAttack();
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
