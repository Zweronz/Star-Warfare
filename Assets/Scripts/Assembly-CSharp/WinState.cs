public class WinState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.DoWin() && player.IsLocal())
		{
			GameApp.GetInstance().GetGameWorld().State = GameState.GameOverUIWin;
		}
	}
}
