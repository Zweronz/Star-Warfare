using UnityEngine;

internal class CatchTheFlagResponse : Response
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
		gameWorld.FlagInPlayerID = playerID;
		gameWorld.CatchTheFlag();
		Player playerByUserID = gameWorld.GetPlayerByUserID(playerID);
		if (playerByUserID == null)
		{
			return;
		}
		AudioManager.GetInstance().PlaySound("Audio/pickup/get_flag");
		if (playerByUserID.IsLocal())
		{
			playerByUserID.VSStatistics.AssistFlags++;
		}
		if (gameWorld.FlagDirObj != null)
		{
			Color color = Color.white;
			if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
			{
				color = UIConstant.COLOR_TEAM_PLAYER_ICONS[(int)playerByUserID.Team];
			}
			else if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_FFA)
			{
				color = UIConstant.COLOR_PLAYER_ICONS[playerByUserID.GetSeatID()];
			}
			color = new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a);
			gameWorld.FlagDirObj.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_TintColor", color);
		}
		if (GameApp.GetInstance().GetGameMode().ModePlay == Mode.VS_CTF_TDM)
		{
			if (gameWorld.LastFlagInPlayerID != -1)
			{
				Player playerByUserID2 = gameWorld.GetPlayerByUserID(gameWorld.LastFlagInPlayerID);
				if (playerByUserID2 != null)
				{
					if (playerByUserID2.IsSameTeam(playerByUserID))
					{
						gameWorld.FlagClock.ResumeTime();
					}
					else
					{
						gameWorld.FlagClock.Restart();
					}
				}
				else
				{
					gameWorld.FlagClock.Restart();
				}
			}
			else
			{
				gameWorld.FlagClock.Restart();
			}
		}
		else
		{
			gameWorld.FlagClock.ResumeTime();
			if (gameWorld.FlagClock.GetTimeLeft() < 10f)
			{
				gameWorld.FlagClock.SetTimeLeft(10);
			}
		}
		gameWorld.VSTimeStopResume();
		playerByUserID.CreatePlayerSign();
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null)
			{
				component.AddWhoKillsWho(playerByUserID.GetSeatID(), HUDAction.CATCH_FLAG, 8);
			}
		}
	}
}
