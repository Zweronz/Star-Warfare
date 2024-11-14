using UnityEngine;

internal class GetSceneStateResponse : Response
{
	public byte playerLength;

	public PlayerInfo[] playerInfos;

	public byte mithrilDropRate;

	public float mithrilDropRateAttenuation;

	public byte minDrop;

	public byte maxDrop;

	public long bossDate;

	public short bossKillTime;

	public short bossDropMithrilTime;

	public short pvpKillCash;

	public short pvpAssistCash;

	public byte[] coopBossKilledTime = new byte[Global.TOTAL_COOP_BOSS_NUM];

	public byte[] coopBossDropMithrilTime = new byte[Global.TOTAL_COOP_BOSS_NUM];

	public EnemyInfo[] enemyInfos;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerLength = bytesBuffer.ReadByte();
		playerInfos = new PlayerInfo[playerLength];
		for (int i = 0; i < playerLength; i++)
		{
			playerInfos[i] = new PlayerInfo();
			playerInfos[i].channelID = bytesBuffer.ReadInt();
			playerInfos[i].seatID = bytesBuffer.ReadByte();
			playerInfos[i].bagIdOfWeapon = bytesBuffer.ReadByte();
			for (int j = 0; j < Global.BAG_MAX_NUM; j++)
			{
				playerInfos[i].bags[j] = bytesBuffer.ReadByte();
			}
			for (int k = 0; k < Global.AVATAR_PART_NUM; k++)
			{
				playerInfos[i].armors[k] = bytesBuffer.ReadByte();
			}
		}
		mithrilDropRate = bytesBuffer.ReadByte();
		minDrop = bytesBuffer.ReadByte();
		maxDrop = bytesBuffer.ReadByte();
		mithrilDropRateAttenuation = (float)(int)bytesBuffer.ReadByte() / 100f;
		bossDate = bytesBuffer.ReadLong();
		bossKillTime = bytesBuffer.ReadShort();
		bossDropMithrilTime = bytesBuffer.ReadShort();
		for (int l = 0; l < Global.TOTAL_COOP_BOSS_NUM; l++)
		{
			coopBossKilledTime[l] = bytesBuffer.ReadByte();
		}
		for (int m = 0; m < Global.TOTAL_COOP_BOSS_NUM; m++)
		{
			coopBossDropMithrilTime[m] = bytesBuffer.ReadByte();
		}
		pvpKillCash = bytesBuffer.ReadShort();
		pvpAssistCash = bytesBuffer.ReadShort();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		int channelID = Lobby.GetInstance().GetChannelID();
		Room currentRoom = Lobby.GetInstance().GetCurrentRoom();
		for (int i = 0; i < playerLength; i++)
		{
			PlayerInfo playerInfo = playerInfos[i];
			if (channelID == playerInfo.channelID)
			{
				gameWorld.Init();
				gameWorld.GetPlayer().SetSeatID(playerInfos[i].seatID);
				gameWorld.GetPlayer().Team = (TeamName)(gameWorld.GetPlayer().GetSeatID() / 4);
				if (GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					gameWorld.GetPlayer().DropAtSpawnPositionVS();
				}
				else
				{
					gameWorld.GetPlayer().DropAtSpawnPosition();
				}
				if (!GameApp.GetInstance().GetGameMode().IsVSMode())
				{
				}
				continue;
			}
			RemotePlayer remotePlayer = new RemotePlayer();
			remotePlayer.SetUserID(playerInfo.channelID);
			remotePlayer.SetSeatID(playerInfo.seatID);
			remotePlayer.Team = (TeamName)(remotePlayer.GetSeatID() / 4);
			remotePlayer.CreateUserState(playerInfo.bags, playerInfo.armors);
			remotePlayer.Init();
			remotePlayer.ChangeWeaponInBag(playerInfo.bagIdOfWeapon);
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
		}
		gameWorld.CreateTeamSkills();
		MithrilDropInfo mithrilDropInfo = new MithrilDropInfo();
		mithrilDropInfo.dropRate = (float)(int)mithrilDropRate * 1f / 100f;
		mithrilDropInfo.minDrop = minDrop;
		mithrilDropInfo.maxDrop = maxDrop;
		mithrilDropInfo.dropRateAttenuation = mithrilDropRateAttenuation;
		gameWorld.MithrilDrops = mithrilDropInfo;
		Debug.Log("drop rate:" + gameWorld.MithrilDrops.dropRate + ", attenuation: " + gameWorld.MithrilDrops.dropRateAttenuation + ", " + gameWorld.MithrilDrops.minDrop + "~" + gameWorld.MithrilDrops.maxDrop);
		gameWorld.PVPReward.cashPerKill = pvpKillCash;
		gameWorld.PVPReward.cashPerAssist = pvpAssistCash;
		Debug.Log("pvp rewards:" + gameWorld.PVPReward.cashPerKill + "," + gameWorld.PVPReward.cashPerAssist);
		UserState userState = GameApp.GetInstance().GetUserState();
		userState.SetBossDate(bossDate);
		userState.SetSuccBossStage(bossKillTime);
		userState.SetSuccBossStageGetMithril(bossDropMithrilTime);
		userState.SetSuccCoopBoss(coopBossKilledTime);
		userState.SetSuccCoopBossGetMithril(coopBossDropMithrilTime);
		gameWorld.VSTimeStopResume();
	}

	public override void ProcessRobotLogic(Robot robot)
	{
		robot.o.transform.position = new Vector3(-10f, 0.1f, -10f);
	}
}
