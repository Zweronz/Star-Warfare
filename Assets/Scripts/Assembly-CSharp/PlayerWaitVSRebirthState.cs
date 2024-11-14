public class PlayerWaitVSRebirthState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal() && player.DoWaitVSRebirth())
		{
			LocalPlayer localPlayer = player as LocalPlayer;
			if (localPlayer != null)
			{
				localPlayer.SendRebirthRequest();
			}
		}
	}
}
