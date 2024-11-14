using UnityEngine;

internal class PlayerRebirthResponse : Response
{
	protected int playerID;

	protected byte spawnPointIndex;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		spawnPointIndex = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player == null)
		{
			return;
		}
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			ThirdPersonStandardCameraScript component = Camera.mainCamera.GetComponent<ThirdPersonStandardCameraScript>();
			if (null != component)
			{
				component.ResetAngleV();
			}
			player.ReSpawnAtPoint(spawnPointIndex);
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				GameObject gameObject = GameObject.Find("GameUI");
				if (gameObject != null)
				{
					InGameUIScript component2 = gameObject.GetComponent<InGameUIScript>();
					if (component2 != null)
					{
						component2.OnVSRebirth();
					}
				}
			}
			Debug.Log("rebirth response:" + player.State);
		}
		else
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.ReSpawnAtPoint(spawnPointIndex);
				gameWorld.CreateTeamSkills();
			}
		}
	}
}
