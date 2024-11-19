using UnityEngine;

public class PlayerLoseState : PlayerState
{
	public override void NextState(Player player, float deltaTime)
	{
		if (player.DoVSLose() && player.IsLocal())
		{
			if (!Application.isMobilePlatform)
			{
				Screen.lockCursor = false;
			}
			GameApp.GetInstance().GetGameWorld().State = GameState.GameOverUILose;
		}
	}
}
