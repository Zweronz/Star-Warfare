using UnityEngine;

internal class SecureVIPPlayerResponse : Response
{
	protected int secureVIPPlayerId;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		secureVIPPlayerId = bytesBuffer.ReadInt();
	}

	public override void ProcessLogic()
	{
		GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
		if (gameWorld == null)
		{
			return;
		}
		Player player = gameWorld.GetPlayer();
		if (player.GetUserID() == secureVIPPlayerId)
		{
			player.VSStatistics.VIPAssist++;
			player.VSStatistics.AddSecureVIPScore();
		}
		Player playerByUserID = gameWorld.GetPlayerByUserID(secureVIPPlayerId);
		GameObject gameObject = GameObject.Find("GameUI");
		if (gameObject != null)
		{
			InGameUIScript component = gameObject.GetComponent<InGameUIScript>();
			if (component != null && playerByUserID != null)
			{
				component.AddWhoKillsWho(playerByUserID.GetSeatID(), HUDAction.SECURE_VIP, 9);
			}
		}
	}
}
