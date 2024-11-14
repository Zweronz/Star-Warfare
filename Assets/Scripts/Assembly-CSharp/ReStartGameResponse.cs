using System.Collections.Generic;
using UnityEngine;

internal class ReStartGameResponse : Response
{
	public override void ReadData(byte[] data)
	{
	}

	public override void ProcessLogic()
	{
		Lobby.GetInstance().GetVSClock().Restart();
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
		player.VSStatistics.ClearAll();
		gameWorld.BattleInfo.ClearAll();
		List<RemotePlayer> remotePlayers = gameWorld.GetRemotePlayers();
		foreach (RemotePlayer item in remotePlayers)
		{
			if (item != null)
			{
				item.VSStatistics.ClearAll();
			}
		}
		gameWorld.State = GameState.Playing;
		player.SetState(Player.IDLE_STATE);
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null)
			{
				component.RestartGame();
			}
		}
		byte spawnPointIndex = player.GetSeatID();
		if (GameApp.GetInstance().GetGameMode().IsVSMode())
		{
			string text = ObjectNamePrefix.PLAYER_SPAWN_POINT + player.GetSeatID();
			if (GameApp.GetInstance().GetGameMode().IsTeamMode())
			{
				text = ObjectNamePrefix.TEAM_SPAWN_POINT + player.GetSeatID();
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.RESPAWN);
			for (byte b = 0; b < array.Length; b++)
			{
				if (text == array[b].name)
				{
					spawnPointIndex = b;
					break;
				}
			}
		}
		PlayerRebirthRequest request = new PlayerRebirthRequest(spawnPointIndex);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}
}
