using UnityEngine;

public class TankColliderWallRushStopScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter()
	{
		if (Lobby.GetInstance().IsMasterPlayer || GameApp.GetInstance().GetGameMode().IsSingle())
		{
			string enemyID = base.transform.parent.name;
			Tank tank = GameApp.GetInstance().GetGameWorld().GetEnemyByID(enemyID) as Tank;
			if (tank != null && tank.GetState() == Tank.RUSH_STATE)
			{
				tank.RushCollidesWall();
			}
		}
	}
}
