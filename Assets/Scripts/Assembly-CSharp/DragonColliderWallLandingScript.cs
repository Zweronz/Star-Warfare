using UnityEngine;

public class DragonColliderWallLandingScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter()
	{
		if (!Lobby.GetInstance().IsMasterPlayer && !GameApp.GetInstance().GetGameMode().IsSingle())
		{
			return;
		}
		string enemyID = base.transform.parent.name;
		Dragon dragon = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyID) as Dragon;
		if (dragon != null && (dragon.GetState() == Dragon.FLY_DIVE_STATE || dragon.GetState() == Dragon.FLY_RUSH_STATE || dragon.GetState() == Dragon.PU_STATE))
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				EnemyStateRequest request = new EnemyStateRequest(dragon.EnemyID, 52, dragon.GetTransform().position, Vector3.zero);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			dragon.SetState(Dragon.LANDING_STATE);
			dragon.EnableGravity(true);
			dragon.SetLandingTimeNow();
		}
	}
}
