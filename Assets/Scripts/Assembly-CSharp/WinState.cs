using UnityEngine;

public class WinState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.DoWin() && player.IsLocal())
		{
			if (!Application.isMobilePlatform)
			{
				Screen.lockCursor = false;
			}
			GameApp.GetInstance().GetGameWorld().State = GameState.GameOverUIWin;
		}
	}
}
