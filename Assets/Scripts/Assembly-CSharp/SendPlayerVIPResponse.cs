using UnityEngine;

internal class SendPlayerVIPResponse : Response
{
	protected int m_vipPlayerId;

	protected int m_vipTime;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		m_vipPlayerId = bytesBuffer.ReadInt();
		m_vipTime = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		gameWorld.VIPClock.Restart();
		gameWorld.VIPClock.SetCurrentTime(m_vipTime / 1000);
		gameWorld.VIPInPlayerID = m_vipPlayerId;
		LocalPlayer player = gameWorld.GetPlayer();
		Debug.Log("Set vip player :" + player.State);
		if (player.GetUserID() == m_vipPlayerId)
		{
			if (player.Hp <= 0 || player.State == Player.DEAD_STATE || player.State == Player.WAIT_VS_REBIRTH_STATE)
			{
				player.SendRebirthRequest();
			}
			else
			{
				PlayerHpRecoveryRequest request = new PlayerHpRecoveryRequest(2, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		Player playerByUserID = gameWorld.GetPlayerByUserID(m_vipPlayerId);
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null && playerByUserID != null)
			{
				component.AddWhoKillsWho(playerByUserID.GetSeatID(), HUDAction.BECOME_VIP, 9);
			}
		}
		Debug.Log("VIP Player is " + m_vipPlayerId + "m_vipTime: " + m_vipTime / 1000);
	}
}
