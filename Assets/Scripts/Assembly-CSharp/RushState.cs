using UnityEngine;

public class RushState : EnemyState
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
		Tank tank = enemy as Tank;
		if (tank == null)
		{
			return;
		}
		tank.PlayAnimation(AnimationString.TANK_RUSH, WrapMode.Loop);
		bool flag = tank.Rush();
		if ((Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle()) && flag)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(tank.EnemyID, 0, tank.GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			tank.SetState(Enemy.IDLE_STATE);
		}
	}

	public override void OnHit(Enemy enemy)
	{
	}
}
