using UnityEngine;

internal class PlayerHpRecoveryResponse : Response
{
	protected int playerID;

	protected int m_hp;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		playerID = bytesBuffer.ReadInt();
		m_hp = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		Player player = GameApp.GetInstance().GetGameWorld().GetPlayer();
		int channelID = Lobby.GetInstance().GetChannelID();
		if (playerID == channelID)
		{
			player.Hp = Mathf.Clamp(m_hp, 0, player.MaxHp);
			return;
		}
		RemotePlayer remotePlayerByUserID = GameApp.GetInstance().GetGameWorld().GetRemotePlayerByUserID(playerID);
		if (remotePlayerByUserID != null)
		{
			remotePlayerByUserID.Hp = Mathf.Clamp(m_hp, 0, remotePlayerByUserID.MaxHp);
		}
	}
}
