public class PlayerWaitRebirthState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal() && player.CheckLose())
		{
			if (GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				player.OnWaitVSRebirth();
				player.SetState(Player.WAIT_VS_REBIRTH_STATE);
			}
			else
			{
				player.SetState(Player.LOSE_STATE);
				GameApp.GetInstance().GetGameWorld().State = GameState.GameOverUILose;
			}
		}
	}
}
