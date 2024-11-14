public class PlayerDeadState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.IsLocal() && player.DeadAnimationCompleted())
		{
			if (player.StartWaitRebirth())
			{
				player.SetState(Player.WAIT_REBIRTH_STATE);
			}
			else if (GameApp.GetInstance().GetGameMode().IsVSMode())
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
