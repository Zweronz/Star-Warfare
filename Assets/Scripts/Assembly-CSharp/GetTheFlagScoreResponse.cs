using UnityEngine;

internal class GetTheFlagScoreResponse : Response
{
	protected int playerID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null || gameWorld.FlagClock == null)
		{
			return;
		}
		gameWorld.HideTheFlag();
		gameWorld.LastFlagInPlayerID = -1;
		gameWorld.FlagInPlayerID = -1;
		gameWorld.FlagClock.Restart();
		gameWorld.FlagClock.StopTime();
		Player playerByUserID = gameWorld.GetPlayerByUserID(playerID);
		if (playerByUserID == null)
		{
			return;
		}
		playerByUserID.CreatePlayerSign();
		if (gameWorld.FlagDirObj != null)
		{
			Color white = Color.white;
			gameWorld.FlagDirObj.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_TintColor", white);
		}
		if (playerByUserID.IsLocal())
		{
			playerByUserID.VSStatistics.SecureFlags++;
			playerByUserID.VSStatistics.AddSecureFlagScore();
			Debug.Log("Get the flag score:" + playerByUserID.VSStatistics.SecureFlags);
		}
		AudioManager.GetInstance().PlaySound("Audio/pickup/win_flag");
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
		{
			if (playerByUserID.IsSameTeam(gameWorld.GetPlayer()))
			{
				gameWorld.GetPlayer().AddTempBuff(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG, 10, 0.5f);
			}
			foreach (RemotePlayer remotePlayer in gameWorld.GetRemotePlayers())
			{
				if (remotePlayer != null && playerByUserID.IsSameTeam(remotePlayer))
				{
					remotePlayer.AddTempBuff(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG, 10, 0.5f);
				}
			}
		}
		else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
		{
			playerByUserID.AddTempBuff(EffectsType.ATTACK_BOOTH_WHEN_SECURE_FLAG, 10, 1f);
		}
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null)
			{
				component.AddWhoKillsWho(playerByUserID.GetSeatID(), HUDAction.SECURE_FLAG, 8);
			}
		}
	}
}
