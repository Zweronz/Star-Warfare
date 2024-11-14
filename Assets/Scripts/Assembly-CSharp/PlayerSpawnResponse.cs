using UnityEngine;

internal class PlayerSpawnResponse : Response
{
	public int channelID;

	public byte seatID;

	public PlayerInfo playerInfo;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
		seatID = bytesBuffer.ReadByte();
		playerInfo = new PlayerInfo();
		for (int i = 0; i < Global.BAG_MAX_NUM; i++)
		{
			playerInfo.bags[i] = bytesBuffer.ReadByte();
		}
		for (int j = 0; j < Global.AVATAR_PART_NUM; j++)
		{
			playerInfo.armors[j] = bytesBuffer.ReadByte();
		}
	}

	public override void ProcessLogic()
	{
		Debug.Log("receive player spawn..");
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
		if (!gameWorld.RemotePlayerExists(channelID))
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Respawn");
			RemotePlayer remotePlayer = new RemotePlayer();
			remotePlayer.SetUserID(channelID);
			remotePlayer.SetSeatID(seatID);
			remotePlayer.Team = (TeamName)(remotePlayer.GetSeatID() / 4);
			remotePlayer.CreateUserState(playerInfo.bags, playerInfo.armors);
			remotePlayer.Init();
			Debug.Log("remotePlayer.hp" + remotePlayer.Hp + ": " + remotePlayer.MaxHp);
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				remotePlayer.DropAtSpawnPositionVS();
			}
			else
			{
				remotePlayer.DropAtSpawnPosition();
			}
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				remotePlayer.CreatePlayerSign();
			}
			gameWorld.AddRemotePlayer(remotePlayer);
			gameWorld.CreateTeamSkills();
			gameWorld.VSTimeStopResume();
		}
		if (Lobby.GetInstance().IsMasterPlayer)
		{
			if (Lobby.GetInstance().WinCondition == 0)
			{
				SendVSTimeRequest request = new SendVSTimeRequest(channelID, Lobby.GetInstance().GetVSClock().GetCurrentTime());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			if (GameApp.GetInstance().GetGameMode().IsCatchTheFlagMode())
			{
				SendFlagTimeRequest request2 = new SendFlagTimeRequest(channelID, gameWorld.FlagClock.GetCurrentTime());
				GameApp.GetInstance().GetNetworkManager().SendRequest(request2);
			}
		}
	}
}
