using System.Collections.Generic;
using UnityEngine;

internal class VSGameEndResponse : Response
{
	protected byte id;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		id = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (player == null)
		{
			return;
		}
		Lobby.GetInstance().GetVSClock().StopAtEnd();
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		TDMState tDMState = (TDMState)GameApp.GetInstance().GetUserState().GetBattleStates()[1];
		FFAState fFAState = (FFAState)GameApp.GetInstance().GetUserState().GetBattleStates()[2];
		VIPState vIPState = (VIPState)GameApp.GetInstance().GetUserState().GetBattleStates()[3];
		CMIState cMIState = (CMIState)GameApp.GetInstance().GetUserState().GetBattleStates()[4];
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_TDM)
		{
			if (player.Team == (TeamName)id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_TDM, true);
				player.OnWin();
				tDMState.AtomicWins();
				tDMState.SetMaxKills(player.VSStatistics.Kills);
				tDMState.AddTotalKills(player.VSStatistics.Kills);
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_TDM, false);
				player.OnLose();
				tDMState.AtomicLoses();
				player.SetState(Player.LOSE_STATE);
			}
			foreach (RemotePlayer item in remotePlayers)
			{
				if (item != null && item.Team == (TeamName)id)
				{
					item.OnWin();
					item.SetState(Player.WIN_STATE);
				}
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_FFA)
		{
			if (player.GetSeatID() == id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_FFA, true);
				player.OnWin();
				fFAState.AtomicWins();
				fFAState.SetMaxKills(player.VSStatistics.Kills);
				fFAState.AddTotalKills(player.VSStatistics.Kills);
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_FFA, false);
				player.OnLose();
				fFAState.AtomicLoses();
				player.SetState(Player.LOSE_STATE);
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
		{
			if (player.Team == (TeamName)id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CTF_TDM, true);
				player.OnWin();
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CTF_TDM, false);
				player.OnLose();
				player.SetState(Player.LOSE_STATE);
			}
			foreach (RemotePlayer item2 in remotePlayers)
			{
				if (item2 != null && item2.Team == (TeamName)id)
				{
					item2.OnWin();
					item2.SetState(Player.WIN_STATE);
				}
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
		{
			if (player.GetSeatID() == id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CTF_FFA, true);
				player.OnWin();
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CTF_FFA, false);
				player.OnLose();
				player.SetState(Player.LOSE_STATE);
			}
			foreach (RemotePlayer item3 in remotePlayers)
			{
				if (item3 != null && item3.GetSeatID() == id)
				{
					item3.OnWin();
					item3.SetState(Player.WIN_STATE);
				}
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_VIP)
		{
			Debug.Log("player.Team ........ " + player.Team);
			Debug.Log("VSGameEndResponse ........ " + (TeamName)id);
			gameWorld.VIPClock.StopAtEnd();
			gameWorld.VIPInPlayerID = -1;
			if (player.Team == (TeamName)id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_VIP, true);
				player.OnWin();
				vIPState.AtomicWins();
				vIPState.SetMaxKills(player.VSStatistics.Kills);
				vIPState.AddTotalKills(player.VSStatistics.Kills);
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_VIP, false);
				player.OnLose();
				vIPState.AtomicLoses();
				player.SetState(Player.LOSE_STATE);
			}
			foreach (RemotePlayer item4 in remotePlayers)
			{
				if (item4 != null && item4.Team == (TeamName)id)
				{
					item4.OnWin();
					item4.SetState(Player.WIN_STATE);
				}
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CMI)
		{
			gameWorld.EndVSCMI();
			if (player.Team == (TeamName)id)
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CMI, true);
				player.OnWin();
				cMIState.AtomicWins();
				cMIState.SetMaxKills(player.VSStatistics.Kills);
				cMIState.AddTotalKills(player.VSStatistics.Kills);
				player.SetState(Player.WIN_STATE);
			}
			else
			{
				player.VSStatistics.CalculateBonus(Mode.VS_CMI, false);
				player.OnLose();
				cMIState.AtomicLoses();
				player.SetState(Player.LOSE_STATE);
			}
			foreach (RemotePlayer item5 in remotePlayers)
			{
				if (item5 != null && item5.Team == (TeamName)id)
				{
					item5.OnWin();
					item5.SetState(Player.WIN_STATE);
				}
			}
		}
		GameApp.GetInstance().GetUserState().AddCash(player.VSStatistics.CashReward);
		Debug.Log("Get PVP Cash Rewards:" + player.VSStatistics.CashReward);
	}
}
