using UnityEngine;

internal class PlayerKillPlayerResponse : Response
{
	protected int killerID;

	protected int assistID;

	protected int killedID;

	protected byte assisterCount;

	protected int[] assisterList;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		killerID = bytesBuffer.ReadInt();
		assistID = bytesBuffer.ReadInt();
		killedID = bytesBuffer.ReadInt();
		assisterCount = bytesBuffer.ReadByte();
		if (assisterCount > 0)
		{
			assisterList = new int[assisterCount];
			for (int i = 0; i < assisterCount; i++)
			{
				assisterList[i] = bytesBuffer.ReadInt();
			}
		}
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
		bool flag = true;
		string killerName = string.Empty;
		string killedName = string.Empty;
		int num = 0;
		int num2 = 0;
		if (killerID == channelID)
		{
			num = player.GetSeatID();
			killerName = "P" + (num + 1);
			player.VSStatistics.Kills++;
			player.VSStatistics.MakeCombo();
			player.VSStatistics.AddKillScore();
			player.RecoveryWhenMakeKills();
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP && gameWorld.VIPInPlayerID == killedID)
			{
				player.VSStatistics.VIPAssist++;
				player.VSStatistics.AddKillVIPScore();
			}
		}
		else
		{
			RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(killerID);
			if (remotePlayerByUserID != null)
			{
				num = remotePlayerByUserID.GetSeatID();
				killerName = "P" + (num + 1);
			}
			else
			{
				flag = false;
			}
		}
		if (assistID != 0)
		{
			if (assistID == channelID)
			{
				player.VSStatistics.Assist++;
				player.VSStatistics.PlayerHit++;
			}
			else
			{
				RemotePlayer remotePlayerByUserID2 = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(assistID);
				if (remotePlayerByUserID2 == null)
				{
					assistID = 0;
				}
			}
		}
		if (assisterCount > 0)
		{
			for (int i = 0; i < assisterCount; i++)
			{
				if (assisterList[i] == channelID)
				{
					player.VSStatistics.PlayerHit++;
				}
			}
		}
		player.VSStatistics.UpdateCashReward();
		if (killedID == channelID)
		{
			num2 = player.GetSeatID();
			killedName = "P" + (num2 + 1);
			player.VSStatistics.Death++;
		}
		else
		{
			RemotePlayer remotePlayerByUserID3 = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(killedID);
			if (remotePlayerByUserID3 != null)
			{
				num2 = remotePlayerByUserID3.GetSeatID();
				killedName = "P" + (num2 + 1);
			}
			else
			{
				flag = false;
			}
		}
		if (!flag || gameWorld.BattleInfo == null)
		{
			return;
		}
		gameWorld.BattleInfo.AddWhoKillsWho(killerName, killedName);
		GameObject gameObject = GameObject.Find("GameUI");
		if (!(gameObject != null))
		{
			return;
		}
		InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
		if (!(component != null))
		{
			return;
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
		{
			if (gameWorld.VIPInPlayerID == killedID)
			{
				component.AddWhoKillsWho(num, HUDAction.KILL_VIP, num2);
			}
			else if (gameWorld.VIPInPlayerID == killerID)
			{
				component.AddWhoKillsWho(num, HUDAction.VIP_KILL, num2);
			}
			else
			{
				component.AddWhoKillsWho(num, HUDAction.KILL, num2);
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			component.AddWhoKillsWho(num, HUDAction.KILL, num2);
		}
		else
		{
			component.AddWhoKillsWho(num, HUDAction.KILL, num2);
		}
	}
}
