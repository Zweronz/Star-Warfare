using UnityEngine;

internal class VSGameAutoBalanceResponse : Response
{
	protected int channelID;

	protected byte newSeatID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		channelID = bytesBuffer.ReadInt();
		newSeatID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		int num = Lobby.GetInstance().GetChannelID();
		if (player == null)
		{
			return;
		}
		if (num == channelID)
		{
			if (newSeatID == player.GetSeatID())
			{
				return;
			}
			player.SetSeatID(newSeatID);
			player.Team = (TeamName)(player.GetSeatID() / 4);
			GameObject gameObject = GameObject.Find("GameUI");
			if (gameObject != null)
			{
				InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
				if (component != null)
				{
					component.VSGameAutoBalance();
				}
			}
		}
		else
		{
			RemotePlayer remotePlayerByUserID = gameWorld.GetRemotePlayerByUserID(channelID);
			if (remotePlayerByUserID != null)
			{
				remotePlayerByUserID.SetSeatID(newSeatID);
				remotePlayerByUserID.Team = (TeamName)(remotePlayerByUserID.GetSeatID() / 4);
				remotePlayerByUserID.CreatePlayerSign();
			}
		}
	}
}
